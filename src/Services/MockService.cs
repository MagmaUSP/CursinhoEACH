using CursinhoEACH.DTO;
using Dapper;
using Npgsql;

public class MockService
{
    private readonly NpgsqlConnection _conn;

    public MockService(NpgsqlConnection conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<MockDTO>> GetAllMocksAsync(string codigo, string dataStr, string turma)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sql = @"
            SELECT 
                e.id, 
                p.nome as Codigo, 
                e.data,
                COALESCE(
                    (SELECT CONCAT(t.ano, ' ', CASE t.periodo WHEN 'M' THEN 'Matutino' WHEN 'V' THEN 'Vespertino' ELSE 'Noturno' END)
                     FROM cursinho_each.aluno_evento ae
                     JOIN cursinho_each.aluno a ON ae.aluno_cpf = a.cpf
                     JOIN cursinho_each.turma t ON a.turma_ano = t.ano AND a.turma_periodo = t.periodo
                     WHERE ae.evento_id = e.id
                     LIMIT 1), 
                'Geral') as Turma
            FROM cursinho_each.evento e
            JOIN cursinho_each.prova p ON e.prova_id = p.id
            WHERE 1=1";

        if (!string.IsNullOrEmpty(codigo)) sql += " AND p.nome ILIKE @Codigo";
        
        DateTime? dataFilter = null;
        if (DateTime.TryParse(dataStr, out var d))
        {
            dataFilter = d;
            sql += " AND e.data = @Data";
        }

        sql += " ORDER BY e.data DESC";

        var result = await _conn.QueryAsync<MockDTO>(sql, new { Codigo = $"%{codigo}%", Data = dataFilter });
        
        if (!string.IsNullOrEmpty(turma))
            return result.Where(x => x.Turma.Contains(turma, StringComparison.OrdinalIgnoreCase));
            
        return result;
    }

    public async Task<MockDetailDTO> GetMockDetailsAsync(int id)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sqlEvento = @"
            SELECT 
                e.id, 
                p.nome as Titulo, 
                e.data,
                p.tipo as TipoProva,  -- 'A', 'B', 'C'
                p.fase as FaseProva,  -- 1, 2...
                COALESCE(
                    (SELECT CONCAT(t.ano, ' ', CASE t.periodo WHEN 'M' THEN 'Matutino' WHEN 'V' THEN 'Vespertino' ELSE 'Noturno' END)
                     FROM cursinho_each.aluno_evento ae
                     JOIN cursinho_each.aluno a ON ae.aluno_cpf = a.cpf
                     JOIN cursinho_each.turma t ON a.turma_ano = t.ano AND a.turma_periodo = t.periodo
                     WHERE ae.evento_id = e.id
                     LIMIT 1), 
                'Geral') as Turma
            FROM cursinho_each.evento e
            JOIN cursinho_each.prova p ON e.prova_id = p.id
            WHERE e.id = @Id";

        var rawDetail = await _conn.QuerySingleOrDefaultAsync<dynamic>(sqlEvento, new { Id = id });
        if (rawDetail == null) return null;

        var detail = new MockDetailDTO 
        {
            Id = rawDetail.id,
            Titulo = rawDetail.titulo,
            Data = rawDetail.data,
            Turma = rawDetail.turma
        };
        
        string currentType = rawDetail.tipoprova; 
        int? currentPhase = rawDetail.faseprova;

        var statsAtual = await GetStatsAux(id);
        
        detail.Presentes = statsAtual.Presentes;
        detail.TotalAlunos = Math.Max(detail.Presentes, 40); 
        detail.QtdCertas = statsAtual.Certas;
        detail.QtdErradas = statsAtual.Erradas;
        detail.QtdBranco = statsAtual.Branco;
        detail.TotalQuestoesRespondidas = statsAtual.TotalRespondidas;

        if (detail.Presentes > 0)
            detail.MediaGeral = (double)detail.QtdCertas / detail.Presentes;

        
        var sqlPrevId = @"
            SELECT e.id 
            FROM cursinho_each.evento e
            JOIN cursinho_each.prova p ON e.prova_id = p.id
            WHERE e.data < @DataAtual 
              AND p.tipo = @TipoAtual 
              AND (p.fase = @FaseAtual OR (p.fase IS NULL AND @FaseAtual IS NULL)) -- Garante mesma fase
            ORDER BY e.data DESC 
            LIMIT 1";
        
        var prevId = await _conn.ExecuteScalarAsync<int?>(sqlPrevId, new { 
            DataAtual = detail.Data, 
            TipoAtual = currentType,
            FaseAtual = currentPhase 
        });

        if (prevId.HasValue)
        {
            var statsPrev = await GetStatsAux(prevId.Value);
            
            double mediaAnterior = 0;
            if (statsPrev.Presentes > 0)
                mediaAnterior = (double)statsPrev.Certas / statsPrev.Presentes;

            if (mediaAnterior > 0)
                detail.Evolucao = (detail.MediaGeral - mediaAnterior) / mediaAnterior;
            else if (detail.MediaGeral > 0)
                detail.Evolucao = 1.0; 
            else
                detail.Evolucao = 0;
        }
        else
        {
            detail.Evolucao = 0; 
        }

        var sqlMaterias = @"
            SELECT 
                m.nome as Materia,
                COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as Certas,
                COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as Erradas,
                COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as Branco
            FROM cursinho_each.aluno_evento ae
            JOIN cursinho_each.aluno_questao aq ON ae.aluno_cpf = aq.aluno_cpf
            JOIN cursinho_each.evento e ON ae.evento_id = e.id
            JOIN cursinho_each.questao q ON e.prova_id = q.prova_id AND aq.questao_numero = q.numero
            JOIN cursinho_each.questao_materia qm ON q.prova_id = qm.questao_prova_id AND q.numero = qm.questao_numero
            JOIN cursinho_each.materia m ON qm.materia_nome = m.nome
            WHERE ae.evento_id = @Id
            GROUP BY m.nome
            ORDER BY m.nome";

        detail.Materias = (await _conn.QueryAsync<SubjectStatDTO>(sqlMaterias, new { Id = id })).ToList();

        return detail;
    }

    private async Task<MockStatsResult> GetStatsAux(int eventoId)
    {
        var result = new MockStatsResult();

        result.Presentes = await _conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM cursinho_each.aluno_evento WHERE evento_id = @Id", new { Id = eventoId });

        var sqlStats = @"
            SELECT 
                COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as Certas,
                COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as Erradas,
                COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as Branco
            FROM cursinho_each.aluno_evento ae
            LEFT JOIN cursinho_each.aluno_questao aq ON ae.aluno_cpf = aq.aluno_cpf
            JOIN cursinho_each.evento e ON ae.evento_id = e.id
            WHERE ae.evento_id = @Id";
        // JOIN cursinho_each.questao q ON e.prova_id = q.prova_id AND aq.questao_numero = q.numero
        

        var row = await _conn.QuerySingleOrDefaultAsync<dynamic>(sqlStats, new { Id = eventoId });

        if (row != null)
        {
            result.Certas = (int)row.certas;
            result.Erradas = (int)row.erradas;
            result.Branco = (int)row.branco;
            result.TotalRespondidas = result.Certas + result.Erradas + result.Branco;
        }

        return result;
    }

    private class MockStatsResult 
    {
        public int Presentes { get; set; }
        public int Certas { get; set; }
        public int Erradas { get; set; }
        public int Branco { get; set; }
        public int TotalRespondidas { get; set; }
    }
}
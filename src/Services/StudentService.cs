using CursinhoEACH.DTO;
using Dapper;
using Npgsql;
using System.Text;

public class StudentService
{
    private readonly NpgsqlConnection _conn;

    public StudentService(NpgsqlConnection conn)
    {
        _conn = conn;
    }

    // --- DASHBOARD ---
    public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync(string cpf, string nome, string email, int ano, string periodo)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sb = new StringBuilder();
        sb.Append(@"
            SELECT 
                COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as TotalCertas,
                COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as TotalErradas,
                COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as TotalBranco,
                
                (SELECT COUNT(*) FROM cursinho_each.evento_turma et
                 JOIN cursinho_each.aluno a2 ON et.turma_ano = a2.turma_ano AND et.turma_periodo = a2.turma_periodo
                 JOIN cursinho_each.pessoa p2 ON a2.cpf = p2.cpf
                 WHERE a2.matriculado = TRUE
                 AND (@Cpf IS NULL OR p2.cpf LIKE @Cpf)
                 AND (@Nome IS NULL OR p2.nome ILIKE @Nome)
                 AND (@Email IS NULL OR p2.email ILIKE @Email)
                 AND (@Ano IS NULL OR a2.turma_ano = @Ano)
                 AND (@Periodo IS NULL OR a2.turma_periodo = @Periodo)) as TotalOportunidades,

                (SELECT COUNT(*) FROM cursinho_each.aluno_evento ae
                 JOIN cursinho_each.aluno a3 ON ae.aluno_cpf = a3.cpf
                 JOIN cursinho_each.pessoa p3 ON a3.cpf = p3.cpf
                 WHERE ae.presente = TRUE
                 AND (@Cpf IS NULL OR p3.cpf LIKE @Cpf)
                 AND (@Nome IS NULL OR p3.nome ILIKE @Nome)
                 AND (@Email IS NULL OR p3.email ILIKE @Email)
                 AND (@Ano IS NULL OR a3.turma_ano = @Ano)
                 AND (@Periodo IS NULL OR a3.turma_periodo = @Periodo)) as TotalPresencas

            FROM cursinho_each.aluno a
            JOIN cursinho_each.pessoa p ON a.cpf = p.cpf
            LEFT JOIN cursinho_each.aluno_questao aq ON a.cpf = aq.aluno_cpf
            LEFT JOIN cursinho_each.questao q ON aq.questao_prova_id = q.prova_id AND aq.questao_numero = q.numero
            WHERE a.matriculado = TRUE ");

        if (!string.IsNullOrEmpty(cpf)) sb.Append(" AND p.cpf LIKE @Cpf");
        if (!string.IsNullOrEmpty(nome)) sb.Append(" AND p.nome ILIKE @Nome");
        if (!string.IsNullOrEmpty(email)) sb.Append(" AND p.email ILIKE @Email");
        if (ano != 0) sb.Append(" AND a.turma_ano = @Ano");
        if (!string.IsNullOrEmpty(periodo)) sb.Append(" AND a.turma_periodo = @Periodo");

        var rawParams = new 
        { 
            Cpf = string.IsNullOrEmpty(cpf) ? null : $"%{cpf}%", 
            Nome = string.IsNullOrEmpty(nome) ? null : $"%{nome}%", 
            Email = string.IsNullOrEmpty(email) ? null : $"%{email}%",
            Ano = ano == 0 ? (int?)null : ano,
            Periodo = string.IsNullOrEmpty(periodo) ? null : $"%{periodo}%"
        };

        var result = await _conn.QuerySingleOrDefaultAsync<dynamic>(sb.ToString(), rawParams);

        var summary = new DashboardSummaryDTO();
        if (result != null)
        {
            summary.TotalCertas = (int)result.totalcertas;
            summary.TotalErradas = (int)result.totalerradas;
            summary.TotalBranco = (int)result.totalbranco;
            summary.TotalQuestoesRespondidas = summary.TotalCertas + summary.TotalErradas + summary.TotalBranco;
            summary.TotalOportunidadesPresenca = (int)result.totaloportunidades;
            summary.TotalPresencasRegistradas = (int)result.totalpresencas;
        }
        return summary;
    }

    // --- LISTAGEM (Index) ---
    public async Task<IEnumerable<StudentSummaryDTO>> GetAllStudentsAsync(string cpf, string nome, string email, int ano, string periodo)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sql = new StringBuilder(@"
            SELECT 
                p.cpf, p.nome, p.email,
                COALESCE((SELECT COUNT(*) FROM cursinho_each.evento_turma et 
                          JOIN cursinho_each.evento e ON et.evento_id = e.id 
                          WHERE et.turma_ano = a.turma_ano AND et.turma_periodo = a.turma_periodo AND e.tipo = 'A'), 0) as TotalAulas,
                COALESCE((SELECT COUNT(*) FROM cursinho_each.aluno_evento ae 
                          JOIN cursinho_each.evento e ON ae.evento_id = e.id
                          WHERE ae.aluno_cpf = a.cpf AND e.tipo = 'A' AND ae.presente = TRUE), 0) as PresencasAulas,
                COALESCE((SELECT COUNT(*) FROM cursinho_each.evento_turma et 
                          JOIN cursinho_each.evento e ON et.evento_id = e.id 
                          WHERE et.turma_ano = a.turma_ano AND et.turma_periodo = a.turma_periodo AND e.tipo = 'S'), 0) as TotalSimulados,
                COALESCE((SELECT COUNT(*) FROM cursinho_each.aluno_evento ae 
                          JOIN cursinho_each.evento e ON ae.evento_id = e.id
                          WHERE ae.aluno_cpf = a.cpf AND e.tipo = 'S' AND ae.presente = TRUE), 0) as PresencasSimulados
            FROM cursinho_each.aluno a
            JOIN cursinho_each.pessoa p ON a.cpf = p.cpf
            WHERE a.matriculado = TRUE ");

        if (!string.IsNullOrEmpty(cpf)) sql.Append(" AND p.cpf LIKE @Cpf");
        if (!string.IsNullOrEmpty(nome)) sql.Append(" AND p.nome ILIKE @Nome");
        if (!string.IsNullOrEmpty(email)) sql.Append(" AND p.email ILIKE @Email");
        if (ano != 0) sql.Append(" AND a.turma_ano = @Ano");
        if (!string.IsNullOrEmpty(periodo)) sql.Append(" AND a.turma_periodo = @Periodo");

        sql.Append(" ORDER BY p.nome");

        var rawData = await _conn.QueryAsync<dynamic>(sql.ToString(), new { 
            Cpf = $"%{cpf}%".Replace(".", "").Replace("-", ""),
            Nome = $"%{nome}%", 
            Email = $"%{email}%",
            Ano = ano == 0 ? (int?)null : ano,
            Periodo = periodo
        });

        var list = new List<StudentSummaryDTO>();
        foreach (var row in rawData)
        {
            int tA = (int)row.totalaulas;
            int pA = (int)row.presencasaulas;
            int tS = (int)row.totalsimulados;
            int pS = (int)row.presencassimulados;

            list.Add(new StudentSummaryDTO
            {
                CPF = row.cpf, Nome = row.nome, Email = row.email,
                IAA = tA > 0 ? ((double)pA / tA) * 100 : 0,
                IAS = tS > 0 ? ((double)pS / tS) * 100 : 0
            });
        }
        return list;
    }

    // --- DETALHES ---
    public async Task<StudentProfileDTO> GetStudentProfileAsync(string cpf)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        // 1. Dados Básicos
        var sqlBasic = @"
            SELECT p.nome, p.cpf, p.email, p.telefone,
            CONCAT(t.ano, ' ', CASE t.periodo WHEN 'M' THEN 'Matutino' WHEN 'V' THEN 'Vespertino' ELSE 'Noturno' END) as Turma
            FROM cursinho_each.aluno a
            JOIN cursinho_each.pessoa p ON a.cpf = p.cpf
            LEFT JOIN cursinho_each.turma t ON a.turma_ano = t.ano AND a.turma_periodo = t.periodo
            WHERE a.cpf = @Cpf";

        var profile = await _conn.QuerySingleOrDefaultAsync<StudentProfileDTO>(sqlBasic, new { Cpf = cpf });
        if (profile == null) return null;

        // 2. Presença
        var sqlPresenca = @"SELECT COUNT(*) as Total, COUNT(CASE WHEN ae.presente = TRUE THEN 1 END) as Presentes
                            FROM cursinho_each.aluno_evento ae WHERE ae.aluno_cpf = @Cpf";
        var pRaw = await _conn.QuerySingleOrDefaultAsync<dynamic>(sqlPresenca, new { Cpf = cpf });
        if (pRaw != null && pRaw.total > 0) profile.PercPresenca = ((double)pRaw.presentes / (double)pRaw.total) * 100;
        else profile.PercPresenca = 100;

        // 3. Questões (CORRIGIDO: Filtra apenas se PRESENTE = TRUE)
        var sqlQ = @"
            SELECT 
                COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as Certas,
                COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as Erradas,
                COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as Branco
            FROM cursinho_each.aluno_evento ae
            JOIN cursinho_each.evento e ON ae.evento_id = e.id
            JOIN cursinho_each.questao q ON e.prova_id = q.prova_id
            LEFT JOIN cursinho_each.aluno_questao aq 
                ON q.prova_id = aq.questao_prova_id 
                AND q.numero = aq.questao_numero 
                AND aq.aluno_cpf = ae.aluno_cpf
            WHERE ae.aluno_cpf = @Cpf 
              AND e.tipo = 'S'
              AND ae.presente = TRUE"; // Alteração aqui

        var qRaw = await _conn.QuerySingleOrDefaultAsync<dynamic>(sqlQ, new { Cpf = cpf });
        if (qRaw != null) { profile.QtdCertas = (int)qRaw.certas; profile.QtdErradas = (int)qRaw.erradas; profile.QtdBranco = (int)qRaw.branco; }

        // 4. Tab Materias (Geral) (CORRIGIDO: Filtra apenas se PRESENTE = TRUE)
        var sqlMat = @"
             SELECT 
                m.nome as Materia, 
                COUNT(*) as Total,
                COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as Certas,
                COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as Erradas,
                COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as Branco
            FROM cursinho_each.aluno_evento ae
            JOIN cursinho_each.evento e ON ae.evento_id = e.id
            JOIN cursinho_each.questao q ON e.prova_id = q.prova_id
            JOIN cursinho_each.questao_materia qm ON q.prova_id = qm.questao_prova_id AND q.numero = qm.questao_numero
            JOIN cursinho_each.materia m ON qm.materia_nome = m.nome
            LEFT JOIN cursinho_each.aluno_questao aq 
                ON q.prova_id = aq.questao_prova_id 
                AND q.numero = aq.questao_numero 
                AND aq.aluno_cpf = ae.aluno_cpf
            WHERE ae.aluno_cpf = @Cpf 
              AND e.tipo = 'S'
              AND ae.presente = TRUE -- Alteração aqui
            GROUP BY m.nome ORDER BY m.nome";
        
        profile.DesempenhoPorMateria = (await _conn.QueryAsync<SubjectPerformanceDTO>(sqlMat, new { Cpf = cpf })).ToList();

        // 5. Tab Simulados
        var sqlSimulados = @"
            SELECT DISTINCT e.id as EventoId, p.nome as Titulo, e.data
            FROM cursinho_each.evento e
            JOIN cursinho_each.prova p ON e.prova_id = p.id
            WHERE e.tipo = 'S' 
            AND (
                -- Só mostra se o aluno estava PRESENTE
                EXISTS (SELECT 1 FROM cursinho_each.aluno_evento ae WHERE ae.evento_id = e.id AND ae.aluno_cpf = @Cpf AND ae.presente = TRUE)
            )
            ORDER BY e.data DESC";

        var simulados = await _conn.QueryAsync<MockExamHistoryDTO>(sqlSimulados, new { Cpf = cpf });
        
        foreach (var sim in simulados)
        {
            // Detalhes Simulado
            var sqlSimDetail = @"
                SELECT 
                    COUNT(*) as Total,
                    COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as Acertos,
                    COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as Erros,
                    COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as Branco
                FROM cursinho_each.evento e
                JOIN cursinho_each.questao q ON e.prova_id = q.prova_id
                LEFT JOIN cursinho_each.aluno_questao aq 
                    ON q.prova_id = aq.questao_prova_id 
                    AND q.numero = aq.questao_numero 
                    AND aq.aluno_cpf = @Cpf
                WHERE e.id = @EvId";
            
            var detail = await _conn.QuerySingleOrDefaultAsync<dynamic>(sqlSimDetail, new { Cpf = cpf, EvId = sim.EventoId });
            sim.TotalQuestoes = (int)detail.total;
            sim.Acertos = (int)detail.acertos;
            sim.Erros = (int)detail.erros;
            sim.Branco = (int)detail.branco;

            // Detalhes Matéria do Simulado
            var sqlSimMat = @"
                 SELECT 
                    m.nome as Materia,
                    COUNT(*) as Total,
                    COUNT(CASE WHEN aq.alternativa = q.gabarito THEN 1 END) as Certas,
                    COUNT(CASE WHEN aq.alternativa != q.gabarito AND aq.alternativa IS NOT NULL THEN 1 END) as Erradas,
                    COUNT(CASE WHEN aq.alternativa IS NULL THEN 1 END) as Branco
                FROM cursinho_each.evento e
                JOIN cursinho_each.questao q ON e.prova_id = q.prova_id
                JOIN cursinho_each.questao_materia qm ON q.prova_id = qm.questao_prova_id AND q.numero = qm.questao_numero
                JOIN cursinho_each.materia m ON qm.materia_nome = m.nome
                LEFT JOIN cursinho_each.aluno_questao aq 
                    ON q.prova_id = aq.questao_prova_id 
                    AND q.numero = aq.questao_numero 
                    AND aq.aluno_cpf = @Cpf
                WHERE e.id = @EvId
                GROUP BY m.nome";

            sim.Materias = (await _conn.QueryAsync<SubjectPerformanceDTO>(sqlSimMat, new { Cpf = cpf, EvId = sim.EventoId })).ToList();
            profile.HistoricoSimulados.Add(sim);
        }

        // 6. Tab Assiduidade
        var sqlAssiduidade = @"
            SELECT 
                COALESCE(em.materia_nome, 'Sem Matéria Definida') as Materia,
                COALESCE((SELECT p.nome 
                          FROM cursinho_each.professor_materia_turma pmt
                          JOIN cursinho_each.pessoa p ON pmt.professor_cpf = p.cpf
                          WHERE pmt.materia_nome = em.materia_nome 
                            AND pmt.turma_ano = a.turma_ano 
                            AND pmt.turma_periodo = a.turma_periodo LIMIT 1), 'N/A') as Professor,
                COUNT(ae.evento_id) as AulasDadas,
                COUNT(CASE WHEN ae.presente = TRUE THEN 1 END) as Presencas
            FROM cursinho_each.aluno_evento ae
            JOIN cursinho_each.evento e ON ae.evento_id = e.id
            LEFT JOIN cursinho_each.evento_materia em ON e.id = em.evento_id 
            JOIN cursinho_each.aluno a ON ae.aluno_cpf = a.cpf
            WHERE ae.aluno_cpf = @Cpf AND e.tipo = 'A'
            GROUP BY em.materia_nome, a.turma_ano, a.turma_periodo
            ORDER BY em.materia_nome";

        profile.AssiduidadePorMateria = (await _conn.QueryAsync<SubjectAttendanceDTO>(sqlAssiduidade, new { Cpf = cpf })).ToList();

        return profile;
    }
}
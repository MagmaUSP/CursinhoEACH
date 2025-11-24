using CursinhoEACH.DTO;
using Dapper;
using Npgsql;

public class EvasionService
{
    private readonly NpgsqlConnection _conn;

    public EvasionService(NpgsqlConnection conn)
    {
        _conn = conn;
    }

    public async Task<IEnumerable<EvasionDTO>> GetEvasionReportAsync()
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();
        
        // CORREÇÃO: Adicionado p.email na query
        var sql = @"
            SELECT 
                p.cpf,
                p.nome,
                p.email, 
                CONCAT(t.ano, ' ', CASE t.periodo WHEN 'M' THEN 'Matutino' WHEN 'V' THEN 'Vespertino' ELSE 'Noturno' END) as Turma,
                
                COALESCE((SELECT COUNT(*) FROM cursinho_each.evento_turma et 
                          JOIN cursinho_each.evento e ON et.evento_id = e.id 
                          WHERE et.turma_ano = a.turma_ano AND et.turma_periodo = a.turma_periodo 
                          AND e.tipo = 'A'), 0) as TotalAulas,
                          
                COALESCE((SELECT COUNT(*) FROM cursinho_each.aluno_evento ae 
                          JOIN cursinho_each.evento e ON ae.evento_id = e.id
                          WHERE ae.aluno_cpf = a.cpf AND e.tipo = 'A' AND ae.presente = TRUE), 0) as PresencasAulas,

                COALESCE((SELECT COUNT(*) FROM cursinho_each.evento_turma et 
                          JOIN cursinho_each.evento e ON et.evento_id = e.id 
                          WHERE et.turma_ano = a.turma_ano AND et.turma_periodo = a.turma_periodo 
                          AND e.tipo = 'S'), 0) as TotalSimulados,
                          
                COALESCE((SELECT COUNT(*) FROM cursinho_each.aluno_evento ae 
                          JOIN cursinho_each.evento e ON ae.evento_id = e.id
                          WHERE ae.aluno_cpf = a.cpf AND e.tipo = 'S' AND ae.presente = TRUE), 0) as PresencasSimulados

            FROM cursinho_each.aluno a
            JOIN cursinho_each.pessoa p ON a.cpf = p.cpf
            JOIN cursinho_each.turma t ON a.turma_ano = t.ano AND a.turma_periodo = t.periodo
            WHERE a.matriculado = TRUE";

        var rawData = await _conn.QueryAsync<dynamic>(sql);

        var report = new List<EvasionDTO>();

        foreach (var row in rawData)
        {
            var dto = new EvasionDTO
            {
                CPF = row.cpf,
                Nome = row.nome,
                Email = row.email, // Agora vai vir preenchido
                Turma = row.turma,
                TotalAulas = (int)row.totalaulas,
                PresencasAulas = (int)row.presencasaulas,
                TotalSimulados = (int)row.totalsimulados,
                PresencasSimulados = (int)row.presencassimulados
            };

            dto.IAA = dto.TotalAulas > 0 ? ((double)dto.PresencasAulas / dto.TotalAulas) * 100 : 100;
            dto.IAS = dto.TotalSimulados > 0 ? ((double)dto.PresencasSimulados / dto.TotalSimulados) * 100 : 100;

            dto.Risco = CalculateRisk(dto.IAA, dto.IAS);

            report.Add(dto);
        }

        return report.OrderByDescending(r => r.Risco == "Alto").ThenBy(r => r.Nome);
    }

    private string CalculateRisk(double iaa, double ias)
    {
        if (ias < 50)
        {
            if (iaa > 85) return "Médio";
            return "Alto";
        }
        else if (ias < 80)
        {
            if (iaa >= 85) return "Baixo";
            if (iaa >= 65) return "Médio";
            return "Alto";
        }
        else
        {
            if (iaa >= 85) return "Baixo";
            if (iaa >= 65) return "Médio";
            return "Médio";
        }
    }
}
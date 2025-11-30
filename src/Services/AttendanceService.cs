#nullable enable
using Dapper;
using Npgsql;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Services
{
    public class AttendanceService
    {
        private readonly string _connectionString;

        public AttendanceService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres") 
                ?? throw new InvalidOperationException("Connection string não encontrada.");
        }

        public async Task<IEnumerable<AttendanceDTO>> GetAttendanceRecordsAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? periodo,
            string? nome,
            bool? presente)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT 
                    p.nome AS Nome,
                    t.ano AS AnoTurma,
                    CASE t.periodo 
                        WHEN 'M' THEN 'Matutino'
                        WHEN 'V' THEN 'Vespertino'
                        WHEN 'N' THEN 'Noturno'
                    END AS Periodo,
                    CASE e.tipo 
                        WHEN 'S' THEN 'Simulado'
                        WHEN 'A' THEN 'Aula'
                        ELSE 'Outro'
                    END AS Evento,
                    e.data AS DataEvento,
                    COALESCE(ae.presente, false) AS Presente,
                    e.id AS EventoId,
                    p.cpf AS Cpf
                FROM pessoa p
                INNER JOIN aluno a ON p.cpf = a.cpf
                INNER JOIN turma t ON a.turma_ano = t.ano AND a.turma_periodo = t.periodo
                INNER JOIN evento_turma et ON t.ano = et.turma_ano AND t.periodo = et.turma_periodo
                INNER JOIN evento e ON et.evento_id = e.id
                LEFT JOIN aluno_evento ae ON a.cpf = ae.aluno_cpf AND e.id = ae.evento_id
                WHERE a.matriculado = true
                AND e.data <= CURRENT_DATE";

            var parameters = new DynamicParameters();

            if (startDate.HasValue)
            {
                sql += " AND e.data >= @StartDate";
                parameters.Add("StartDate", startDate.Value);
            }

            if (endDate.HasValue)
            {
                sql += " AND e.data <= @EndDate";
                parameters.Add("EndDate", endDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(periodo))
            {
                var periodoChar = periodo switch
                {
                    "Matutino" => "M",
                    "Vespertino" => "V",
                    "Noturno" => "N",
                    _ => null
                };
                if (periodoChar != null)
                {
                    sql += " AND t.periodo = @Periodo";
                    parameters.Add("Periodo", periodoChar);
                }
            }

            if (!string.IsNullOrWhiteSpace(nome))
            {
                sql += " AND p.nome ILIKE @Nome";
                parameters.Add("Nome", $"%{nome}%");
            }

            if (presente.HasValue)
            {
                sql += " AND COALESCE(ae.presente, false) = @Presente";
                parameters.Add("Presente", presente.Value);
            }

            sql += " ORDER BY p.nome, e.data";

            return await connection.QueryAsync<AttendanceDTO>(sql, parameters);
        }

        public async Task<bool> MarkAttendanceAsync(string cpf, long eventoId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                INSERT INTO aluno_evento (aluno_cpf, evento_id, presente)
                VALUES (@Cpf, @EventoId, true)
                ON CONFLICT (aluno_cpf, evento_id) 
                DO UPDATE SET presente = true";

            var affected = await connection.ExecuteAsync(sql, new { Cpf = cpf, EventoId = eventoId });
            return affected > 0;
        }
    }
}
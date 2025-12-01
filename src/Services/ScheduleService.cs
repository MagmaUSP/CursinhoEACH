using Dapper;
using Npgsql;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Services
{
    public class ScheduleService
    {
        private readonly string _connectionString;

        public ScheduleService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres") 
                ?? throw new InvalidOperationException("Connection string não encontrada.");
        }

        public async Task<List<ProfessorScheduleDTO>> GetProfessoresDisponiveisAsync(int ano, string periodo)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            
            var sql = @"
                SELECT DISTINCT
                    p.cpf AS Cpf,
                    p.nome AS Nome,
                    m.nome AS Materia
                FROM pessoa p
                INNER JOIN professor prof ON p.cpf = prof.cpf
                INNER JOIN professor_materia_turma pmt ON prof.cpf = pmt.professor_cpf
                INNER JOIN materia m ON pmt.materia_nome = m.nome
                INNER JOIN turma t ON pmt.turma_ano = t.ano AND pmt.turma_periodo = t.periodo
                WHERE t.ano = @Ano 
                AND t.periodo = @Periodo
                AND prof.tipo = 'R'
                ORDER BY p.nome, m.nome";

            var result = await connection.QueryAsync<ProfessorScheduleDTO>(
                sql, 
                new { Ano = ano, Periodo = periodo }
            );

            return result.ToList();
        }

        public async Task<List<AulaAgendadaDTO>> GetGradeExistenteAsync(int ano, string periodo, DateTime weekStart)
        {
            Console.WriteLine($"=== GetGradeExistenteAsync CHAMADO ===");
            Console.WriteLine($"Parâmetros: Ano={ano}, Periodo={periodo}, WeekStart={weekStart:yyyy-MM-dd}");
            
            using var connection = new NpgsqlConnection(_connectionString);
            
            // Converter para DateOnly para evitar problemas de timezone
            var weekStartDate = DateOnly.FromDateTime(weekStart);
            var weekEndDate = weekStartDate.AddDays(4); // Sexta-feira
            
            Console.WriteLine($"Buscando eventos entre {weekStartDate:yyyy-MM-dd} e {weekEndDate:yyyy-MM-dd}");
            
            // Primeiro verificar TODOS os eventos da turma sem filtros
            var sqlDebugTodos = @"
                SELECT e.id, e.data, e.hora_inicio, em.materia_nome
                FROM evento e
                INNER JOIN evento_turma et ON e.id = et.evento_id
                LEFT JOIN evento_materia em ON e.id = em.evento_id
                WHERE et.turma_ano = @Ano
                AND et.turma_periodo = @Periodo
                AND e.tipo = 'A'";
            
            var todosDaTurma = await connection.QueryAsync(sqlDebugTodos, new { Ano = ano, Periodo = periodo });
            Console.WriteLine($"Total de eventos tipo 'A' da turma {ano}-{periodo}: {todosDaTurma.Count()}");
            
            // Agora verificar eventos da semana
            var sqlDebugSemana = @"
                SELECT e.id, e.data, e.hora_inicio, em.materia_nome
                FROM evento e
                INNER JOIN evento_turma et ON e.id = et.evento_id
                LEFT JOIN evento_materia em ON e.id = em.evento_id
                WHERE et.turma_ano = @Ano
                AND et.turma_periodo = @Periodo
                AND e.tipo = 'A'
                AND e.data >= @WeekStart
                AND e.data <= @WeekEnd";
            
            var daSemanaSemFiltro = await connection.QueryAsync(
                sqlDebugSemana, 
                new { Ano = ano, Periodo = periodo, WeekStart = weekStartDate, WeekEnd = weekEndDate }
            );
            Console.WriteLine($"Eventos da semana {weekStartDate:yyyy-MM-dd} a {weekEndDate:yyyy-MM-dd}: {daSemanaSemFiltro.Count()}");
            foreach (var ev in daSemanaSemFiltro)
            {
                Console.WriteLine($"  ID {ev.id}: {ev.materia_nome} em {ev.data:yyyy-MM-dd} às {ev.hora_inicio}");
            }
            
            var sql = @"
                SELECT DISTINCT
                    pmt.professor_cpf AS ProfessorCpf,
                    em.materia_nome AS Materia,
                    EXTRACT(DOW FROM e.data)::int AS DiaSemana,
                    e.data AS DataEvento,
                    TO_CHAR(e.hora_inicio, 'HH24:MI') AS HorarioInicio,
                    e.data AS DataOrdenacao,
                    e.hora_inicio AS HoraOrdenacao
                FROM evento e
                INNER JOIN evento_turma et ON e.id = et.evento_id
                INNER JOIN evento_materia em ON e.id = em.evento_id
                INNER JOIN professor_materia_turma pmt ON 
                    em.materia_nome = pmt.materia_nome AND
                    et.turma_ano = pmt.turma_ano AND
                    et.turma_periodo = pmt.turma_periodo
                INNER JOIN professor prof ON pmt.professor_cpf = prof.cpf
                WHERE et.turma_ano = @Ano
                AND et.turma_periodo = @Periodo
                AND e.tipo = 'A'
                AND e.data >= @WeekStart
                AND e.data <= @WeekEnd
                AND prof.tipo = 'R'
                ORDER BY DataOrdenacao, HoraOrdenacao";

            var result = await connection.QueryAsync<AulaAgendadaDTO>(
                sql,
                new { Ano = ano, Periodo = periodo, WeekStart = weekStartDate, WeekEnd = weekEndDate }
            );

            Console.WriteLine($"Resultado final após JOIN com professores: {result.Count()} aulas");
            Console.WriteLine($"=== FIM GetGradeExistenteAsync ===\n");
            
            return result.ToList();
        }

        public async Task<bool> AtualizarGradeHorariaAsync(int ano, string periodo, DateTime weekStart, List<AulaAgendadaDTO> aulas)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Converter para DateOnly para evitar problemas de timezone
                var weekStartDate = DateOnly.FromDateTime(weekStart);
                var weekEndDate = weekStartDate.AddDays(4);
                
                Console.WriteLine($"=== DELETANDO EVENTOS DA SEMANA ===");
                Console.WriteLine($"Período: {weekStartDate:yyyy-MM-dd} a {weekEndDate:yyyy-MM-dd}");

                // 1. Deletar todos os eventos de aula dessa semana/turma
                var sqlDeleteAlunoEvento = @"
                    DELETE FROM aluno_evento
                    WHERE evento_id IN (
                        SELECT e.id
                        FROM evento e
                        INNER JOIN evento_turma et ON e.id = et.evento_id
                        WHERE et.turma_ano = @Ano
                        AND et.turma_periodo = @Periodo
                        AND e.tipo = 'A'
                        AND e.data >= @WeekStart
                        AND e.data <= @WeekEnd
                    )";

                await connection.ExecuteAsync(
                    sqlDeleteAlunoEvento,
                    new { Ano = ano, Periodo = periodo, WeekStart = weekStartDate, WeekEnd = weekEndDate },
                    transaction
                );

                var sqlDeleteEventoMateria = @"
                    DELETE FROM evento_materia
                    WHERE evento_id IN (
                        SELECT e.id
                        FROM evento e
                        INNER JOIN evento_turma et ON e.id = et.evento_id
                        WHERE et.turma_ano = @Ano
                        AND et.turma_periodo = @Periodo
                        AND e.tipo = 'A'
                        AND e.data >= @WeekStart
                        AND e.data <= @WeekEnd
                    )";

                await connection.ExecuteAsync(
                    sqlDeleteEventoMateria,
                    new { Ano = ano, Periodo = periodo, WeekStart = weekStartDate, WeekEnd = weekEndDate },
                    transaction
                );

                var sqlDeleteEventoTurma = @"
                    DELETE FROM evento_turma
                    WHERE evento_id IN (
                        SELECT e.id
                        FROM evento e
                        INNER JOIN evento_turma et ON e.id = et.evento_id
                        WHERE et.turma_ano = @Ano
                        AND et.turma_periodo = @Periodo
                        AND e.tipo = 'A'
                        AND e.data >= @WeekStart
                        AND e.data <= @WeekEnd
                    )";

                await connection.ExecuteAsync(
                    sqlDeleteEventoTurma,
                    new { Ano = ano, Periodo = periodo, WeekStart = weekStartDate, WeekEnd = weekEndDate },
                    transaction
                );

                var sqlDeleteEvento = @"
                    DELETE FROM evento
                    WHERE id IN (
                        SELECT e.id
                        FROM evento e
                        INNER JOIN evento_turma et ON e.id = et.evento_id
                        WHERE et.turma_ano = @Ano
                        AND et.turma_periodo = @Periodo
                        AND e.tipo = 'A'
                        AND e.data >= @WeekStart
                        AND e.data <= @WeekEnd
                    )";

                await connection.ExecuteAsync(
                    sqlDeleteEvento,
                    new { Ano = ano, Periodo = periodo, WeekStart = weekStartDate, WeekEnd = weekEndDate },
                    transaction
                );

                // 2. Corrigir sequência
                var sqlResetSequence = @"
                    SELECT setval('cursinho_each.evento_id_seq', (SELECT COALESCE(MAX(id), 0) FROM cursinho_each.evento))";
                
                await connection.ExecuteAsync(sqlResetSequence, transaction: transaction);

                // 3. Inserir novos eventos
                Console.WriteLine($"=== INICIANDO INSERÇÃO DE {aulas.Count} AULAS ===");
                var eventosInseridos = new List<long>();
                
                foreach (var aula in aulas)
                {
                    // Converter para DateOnly para evitar problemas de timezone
                    var dataEvento = DateOnly.FromDateTime(aula.DataEvento);
                    Console.WriteLine($"Inserindo: {aula.Materia} em {dataEvento:yyyy-MM-dd} (original: {aula.DataEvento:yyyy-MM-dd HH:mm:ss}) às {aula.HorarioInicio}");
                    
                    var sqlEvento = @"
                        INSERT INTO evento (data, hora_inicio, tipo)
                        VALUES (@Data, @HoraInicio, 'A')
                        RETURNING id";

                    var eventoId = await connection.ExecuteScalarAsync<long>(
                        sqlEvento,
                        new { Data = dataEvento, HoraInicio = TimeSpan.Parse(aula.HorarioInicio) },
                        transaction
                    );
                    eventosInseridos.Add(eventoId);
                    Console.WriteLine($"✓ Evento ID {eventoId} criado");

                    var sqlEventoTurma = @"
                        INSERT INTO evento_turma (evento_id, turma_ano, turma_periodo)
                        VALUES (@EventoId, @Ano, @Periodo)";

                    await connection.ExecuteAsync(
                        sqlEventoTurma,
                        new { EventoId = eventoId, Ano = ano, Periodo = periodo },
                        transaction
                    );

                    var sqlEventoMateria = @"
                        INSERT INTO evento_materia (evento_id, materia_nome)
                        VALUES (@EventoId, @Materia)";

                    await connection.ExecuteAsync(
                        sqlEventoMateria,
                        new { EventoId = eventoId, Materia = aula.Materia },
                        transaction
                    );

                    var sqlAlunoEvento = @"
                        INSERT INTO aluno_evento (aluno_cpf, evento_id, presente)
                        SELECT a.cpf, @EventoId, FALSE
                        FROM aluno a
                        WHERE a.turma_ano = @Ano 
                        AND a.turma_periodo = @Periodo
                        AND a.matriculado = TRUE";

                    await connection.ExecuteAsync(
                        sqlAlunoEvento,
                        new { EventoId = eventoId, Ano = ano, Periodo = periodo },
                        transaction
                    );
                }

                Console.WriteLine($"=== COMMIT DA TRANSAÇÃO ===");
                await transaction.CommitAsync();
                Console.WriteLine($"✓ Transação commitada com sucesso");
                
                // Verificar se os eventos foram realmente salvos
                Console.WriteLine($"=== VERIFICANDO EVENTOS NO BANCO APÓS COMMIT ===");
                var sqlVerificar = @"
                    SELECT e.id, e.data, e.hora_inicio, e.tipo, em.materia_nome
                    FROM evento e
                    LEFT JOIN evento_materia em ON e.id = em.evento_id
                    WHERE e.id = ANY(@EventoIds)";
                
                var eventosVerificados = await connection.QueryAsync(
                    sqlVerificar,
                    new { EventoIds = eventosInseridos.ToArray() }
                );
                
                Console.WriteLine($"Eventos encontrados no banco: {eventosVerificados.Count()}");
                foreach (var ev in eventosVerificados)
                {
                    Console.WriteLine($"  ID {ev.id}: {ev.materia_nome} em {ev.data} às {ev.hora_inicio}");
                }
                
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> SalvarGradeHorariaAsync(int ano, string periodo, DateTime weekStart, List<AulaAgendadaDTO> aulas)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Corrigir sequência antes de inserir
                var sqlResetSequence = @"
                    SELECT setval('cursinho_each.evento_id_seq', (SELECT COALESCE(MAX(id), 0) FROM cursinho_each.evento))";
                
                await connection.ExecuteAsync(sqlResetSequence, transaction: transaction);

                foreach (var aula in aulas)
                {
                    // 1. Verificar se já existe evento nessa data/horário para essa turma/matéria
                    var sqlVerificar = @"
                        SELECT e.id
                        FROM evento e
                        INNER JOIN evento_turma et ON e.id = et.evento_id
                        INNER JOIN evento_materia em ON e.id = em.evento_id
                        WHERE e.data = @Data
                        AND et.turma_ano = @Ano
                        AND et.turma_periodo = @Periodo
                        AND em.materia_nome = @Materia
                        LIMIT 1";

                    var eventoExistente = await connection.QueryFirstOrDefaultAsync<long?>(
                        sqlVerificar,
                        new { Data = aula.DataEvento, Ano = ano, Periodo = periodo, Materia = aula.Materia },
                        transaction
                    );

                    long eventoId;

                    if (eventoExistente.HasValue)
                    {
                        // Se já existe, usar o ID existente
                        eventoId = eventoExistente.Value;
                    }
                    else
                    {
                        // 2. Criar novo evento - usar DEFAULT para gerar ID automaticamente
                        var sqlEvento = @"
                            INSERT INTO evento (id, data, tipo)
                            VALUES (DEFAULT, @Data, 'A')
                            RETURNING id";

                        eventoId = await connection.ExecuteScalarAsync<long>(
                            sqlEvento,
                            new { Data = aula.DataEvento },
                            transaction
                        );

                        // 3. Vincular evento à turma
                        var sqlEventoTurma = @"
                            INSERT INTO evento_turma (evento_id, turma_ano, turma_periodo)
                            VALUES (@EventoId, @Ano, @Periodo)";

                        await connection.ExecuteAsync(
                            sqlEventoTurma,
                            new { EventoId = eventoId, Ano = ano, Periodo = periodo },
                            transaction
                        );

                        // 4. Vincular evento à matéria
                        var sqlEventoMateria = @"
                            INSERT INTO evento_materia (evento_id, materia_nome)
                            VALUES (@EventoId, @Materia)";

                        await connection.ExecuteAsync(
                            sqlEventoMateria,
                            new { EventoId = eventoId, Materia = aula.Materia },
                            transaction
                        );

                        // 5. Criar registros de presença para todos os alunos da turma
                        var sqlAlunoEvento = @"
                            INSERT INTO aluno_evento (aluno_cpf, evento_id, presente)
                            SELECT a.cpf, @EventoId, FALSE
                            FROM aluno a
                            WHERE a.turma_ano = @Ano 
                            AND a.turma_periodo = @Periodo
                            AND a.matriculado = TRUE";

                        await connection.ExecuteAsync(
                            sqlAlunoEvento,
                            new { EventoId = eventoId, Ano = ano, Periodo = periodo },
                            transaction
                        );
                    }
                }

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
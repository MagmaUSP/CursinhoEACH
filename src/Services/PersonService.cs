using CursinhoEACH.Models;
using Npgsql;
using Dapper;
using CursinhoEACH.DTO;

public class PersonService
{
    private readonly NpgsqlConnection _conn;

    public PersonService(NpgsqlConnection conn)
    {
        _conn = conn;
    }

    public async Task CreatePersonAsync(PersonDTO person)
    {
        if (_conn.State != System.Data.ConnectionState.Open)
            await _conn.OpenAsync();

        using var transaction = await _conn.BeginTransactionAsync();

        person.CPF = person.CPF.Trim().Replace(".", "").Replace("-", "");
        
        if (!string.IsNullOrEmpty(person.RepresentanteLegalCPF))
            person.RepresentanteLegalCPF = person.RepresentanteLegalCPF.Trim().Replace(".", "").Replace("-", "");

        if (!string.IsNullOrEmpty(person.Telefone))
            person.Telefone = person.Telefone.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");

        try
        {
            DateOnly? dataNascimentoFinal = person.DataNascimento.HasValue 
                ? DateOnly.FromDateTime(person.DataNascimento.Value) 
                : null;

            var sqlPessoa = @"
                INSERT INTO cursinho_each.pessoa (cpf, nome, email, telefone, endereco, data_nascimento)
                VALUES (@CPF, @Nome, @Email, @Telefone, @Endereco, @DataNascimento);";

            await _conn.ExecuteAsync(sqlPessoa, new {
                person.CPF,
                person.Nome,
                person.Email,
                person.Telefone,
                person.Endereco,
                DataNascimento = dataNascimentoFinal 
            }, transaction);

            if (person.Papel == "Aluno")
            {
                if (string.IsNullOrEmpty(person.Turma))
                    throw new Exception("Dados do aluno incompletos (Turma).");
                    
                var parts = person.Turma.Split(' ');
                if (parts.Length < 2) throw new Exception("Formato da turma inválido.");
                
                var anoTurma = int.Parse(parts[0]);
                var periodoTurma = parts[1].Substring(0, 1);

                var sqlAluno = @"
                    INSERT INTO cursinho_each.aluno (cpf, ano_escolar, matriculado, representante_legal, turma_ano, turma_periodo)
                    VALUES (@CPF, @AnoEscolar, @Matriculado, @RepresentanteLegal, @TurmaAno, @TurmaPeriodo);";

                await _conn.ExecuteAsync(sqlAluno, new {
                    Cpf = person.CPF,
                    AnoEscolar = person.AnoEscolar ?? 0,
                    Matriculado = person.Matriculado ?? true,
                    RepresentanteLegal = person.RepresentanteLegalCPF,
                    TurmaAno = anoTurma,
                    TurmaPeriodo = periodoTurma
                }, transaction);
            }
            else if (person.Papel == "Professor")
            {
                if (string.IsNullOrEmpty(person.TipoProfessor))
                    throw new Exception("Tipo do professor obrigatório.");

                var tipoChar = person.TipoProfessor.Substring(0, 1); 

                var sqlProfessor = @"
                    INSERT INTO cursinho_each.professor (cpf, tipo)
                    VALUES (@CPF, @Tipo);";

                await _conn.ExecuteAsync(sqlProfessor, new {
                    Cpf = person.CPF,
                    Tipo = tipoChar
                }, transaction);
            }
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync(string cpf, string nome, string email, string papel)
    {
        if (_conn.State != System.Data.ConnectionState.Open)
            await _conn.OpenAsync();

        var sql = @"
            SELECT 
                p.cpf, 
                p.nome, 
                p.email, 
                p.telefone,
                CASE 
                    WHEN a.cpf IS NOT NULL THEN 'Aluno'
                    WHEN pr.cpf IS NOT NULL THEN 'Professor'
                    ELSE 'Responsável'
                END as Papel
            FROM pessoa p
            LEFT JOIN aluno a ON p.cpf = a.cpf
            LEFT JOIN professor pr ON p.cpf = pr.cpf
            WHERE 1=1 
        ";

        if (!string.IsNullOrEmpty(cpf))
        {
            sql += " AND p.cpf LIKE @Cpf";
        }

        if (!string.IsNullOrEmpty(nome))
        {
            sql += " AND p.nome ILIKE @Nome"; 
        }

        if (!string.IsNullOrEmpty(email))
        {
            sql += " AND p.email ILIKE @Email";
        }

        if (!string.IsNullOrEmpty(papel))
        {
            if (papel == "Aluno") sql += " AND a.cpf IS NOT NULL";
            else if (papel == "Professor") sql += " AND pr.cpf IS NOT NULL";
            else if (papel == "Responsável") sql += " AND a.cpf IS NULL AND pr.cpf IS NULL";
        }

        sql += " ORDER BY p.nome ASC;";

        return await _conn.QueryAsync<PersonDTO>(sql, new { 
            Cpf = $"%{cpf}%",
            Nome = $"%{nome}%", 
            Email = $"%{email}%" 
        });
    }

   public async Task<PersonDTO2> GetPersonByCpfAsync(string cpf)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sql = @"
            SELECT 
                p.cpf, p.nome, p.email, p.telefone, p.endereco, p.data_nascimento,
                CASE 
                    WHEN a.cpf IS NOT NULL THEN 'Aluno'
                    WHEN pr.cpf IS NOT NULL THEN 'Professor'
                    ELSE 'Responsável'
                END as Papel,
                -- Dados de Aluno
                a.ano_escolar, 
                a.matriculado, 
                a.representante_legal, 
                a.desligado_motivo,
                a.turma_ano,
                a.turma_periodo,
                -- Dados de Professor
                pr.tipo
            FROM pessoa p
            LEFT JOIN aluno a ON p.cpf = a.cpf
            LEFT JOIN professor pr ON p.cpf = pr.cpf
            WHERE p.cpf = @cpf";

        var row = await _conn.QuerySingleOrDefaultAsync<dynamic>(sql, new { cpf });

        if (row == null) return null;

        var dto = new PersonDTO2
        {
            CPF = row.cpf,
            Nome = row.nome,
            Email = row.email,
            Telefone = row.telefone,
            Endereco = row.endereco,
            Papel = row.papel,
            DataNascimento = row.data_nascimento != null ? Convert.ToDateTime(row.data_nascimento) : null
        };

        if (dto.Papel == "Aluno")
        {
            dto.Student = new StudentDTO
            {
                AnoEscolar = row.ano_escolar,
                Matriculado = row.matriculado,
                RepresentanteLegal = row.representante_legal,
                DesligadoMotivo = row.desligado_motivo,
                AnoTurma = row.turma_ano,
                PeriodoTurma = row.turma_periodo,
                TurmaSelecionada = (row.turma_ano != null) ? $"{row.turma_ano} {((string)row.turma_periodo == "M" ? "Matutino" : (string)row.turma_periodo == "V" ? "Vespertino" : "Noturno")}" : ""
            };
        }
        else if (dto.Papel == "Professor")
        {
            dto.Teacher = new TeacherDTO
            {
                Tipo = row.tipo
            };
        }

        return dto;
    }

    public async Task UpdatePersonAsync(PersonDTO2 person)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        using var transaction = await _conn.BeginTransactionAsync();

        try
        {
            if (!string.IsNullOrEmpty(person.Telefone))
                person.Telefone = person.Telefone.Trim().Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            
            var sqlPessoa = @"
                UPDATE pessoa 
                SET nome = @Nome, email = @Email, telefone = @Telefone, endereco = @Endereco, data_nascimento = @DataNascimento
                WHERE cpf = @CPF"; 

            await _conn.ExecuteAsync(sqlPessoa, new {
                person.Nome, person.Email, person.Telefone, person.Endereco, person.CPF, person.DataNascimento
            }, transaction);

            if (person.Papel == "Aluno" && person.Student != null)
            {
                if (string.IsNullOrEmpty(person.Student.TurmaSelecionada)) throw new Exception("Turma obrigatória.");
                
                if(!string.IsNullOrEmpty(person.Student.RepresentanteLegal))
                    person.Student.RepresentanteLegal = person.Student.RepresentanteLegal.Trim().Replace(".", "").Replace("-", "");

                var sqlAluno = @"
                    UPDATE aluno 
                    SET ano_escolar = @AnoEscolar, 
                        matriculado = @Matriculado, 
                        representante_legal = @RepresentanteLegal, 
                        turma_ano = @AnoTurma, 
                        turma_periodo = @PeriodoTurma,
                        desligado_motivo = @DesligadoMotivo
                    WHERE cpf = @CPF";

                await _conn.ExecuteAsync(sqlAluno, new {
                    person.Student.AnoEscolar, 
                    person.Student.Matriculado, 
                    person.Student.RepresentanteLegal, 
                    person.Student.AnoTurma, 
                    person.Student.PeriodoTurma, 
                    person.Student.DesligadoMotivo, 
                    person.CPF
                }, transaction);
            }
            else if (person.Papel == "Professor" && person.Teacher != null)
            {
                var tipoChar = person.Teacher.Tipo.Substring(0, 1);
                Console.WriteLine($"Atualizando Professor: CPF={person.CPF}, Tipo={tipoChar}");
                var sqlProf = "UPDATE professor SET tipo = @Tipo WHERE cpf = @CPF";
                await _conn.ExecuteAsync(sqlProf, new { Tipo = tipoChar, person.CPF }, transaction);
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Adicione este método na classe PersonService
    public async Task DeletePersonAsync(string cpf)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();
        
        using var transaction = await _conn.BeginTransactionAsync();

        try 
        {
            // 1. Limpeza de PROFESSOR (se houver vínculos)
            // Remove vínculos com turmas antes de apagar o professor
            await _conn.ExecuteAsync(
                "DELETE FROM cursinho_each.professor_materia_turma WHERE professor_cpf = @cpf", 
                new { cpf }, transaction);

            await _conn.ExecuteAsync(
                "DELETE FROM cursinho_each.professor WHERE cpf = @cpf", 
                new { cpf }, transaction);

            // 2. Limpeza de ALUNO (se houver vínculos)
            // É seguro rodar isso mesmo que o aluno não tenha eventos/questões.
            // Se não tiver nada, o banco apenas ignora e segue em frente.
            await _conn.ExecuteAsync(
                "DELETE FROM cursinho_each.aluno_evento WHERE aluno_cpf = @cpf", 
                new { cpf }, transaction);
                
            await _conn.ExecuteAsync(
                "DELETE FROM cursinho_each.aluno_questao WHERE aluno_cpf = @cpf", 
                new { cpf }, transaction);

            await _conn.ExecuteAsync(
                "DELETE FROM cursinho_each.aluno WHERE cpf = @cpf", 
                new { cpf }, transaction);

            // 3. Limpeza de PESSOA (O pai de todos)
            // Se essa pessoa for Representante Legal de OUTRO aluno ativo, o banco vai travar aqui (Erro 23503).
            // Isso é o comportamento correto: não podemos apagar um responsável que ainda tem dependentes no sistema.
            await _conn.ExecuteAsync(
                "DELETE FROM cursinho_each.pessoa WHERE cpf = @cpf", 
                new { cpf }, transaction);

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw; // O Controller captura a exceção e exibe a mensagem de erro apropriada
        }
    }
}
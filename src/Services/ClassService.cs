using CursinhoEACH.DTO;
using Dapper;
using Npgsql;

public class ClassService
{
    private readonly NpgsqlConnection _conn;

    public ClassService(NpgsqlConnection conn)
    {
        _conn = conn;
    }

    // Listar todas as turmas (Index)
    public async Task<IEnumerable<ClassDTO>> GetAllClassesAsync(int? ano, string periodo)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sql = @"
            SELECT 
                t.ano, 
                t.periodo, 
                t.capacidade,
                (SELECT COUNT(*) FROM cursinho_each.aluno a WHERE a.turma_ano = t.ano AND a.turma_periodo = t.periodo) as Matriculados
            FROM cursinho_each.turma t
            WHERE 1=1";

        if (ano.HasValue) sql += " AND t.ano = @Ano";
        if (!string.IsNullOrEmpty(periodo)) sql += " AND t.periodo = @Periodo";

        sql += " ORDER BY t.ano DESC, t.periodo ASC";

        return await _conn.QueryAsync<ClassDTO>(sql, new { Ano = ano, Periodo = periodo });
    }

    // Criar Turma
    public async Task CreateClassAsync(ClassDTO turma)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        var sql = @"INSERT INTO cursinho_each.turma (ano, periodo, capacidade) VALUES (@Ano, @Periodo, @Capacidade)";
        await _conn.ExecuteAsync(sql, turma);
    }

    // Obter Detalhes Completos (Turma + Alunos + Professores)
    public async Task<ClassDetailsDTO> GetClassDetailsAsync(int ano, string periodo)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();

        // 1. Dados da Turma
        var sqlTurma = @"
            SELECT t.ano, t.periodo, t.capacidade,
            (SELECT COUNT(*) FROM cursinho_each.aluno a WHERE a.turma_ano = t.ano AND a.turma_periodo = t.periodo) as Matriculados
            FROM cursinho_each.turma t
            WHERE t.ano = @Ano AND t.periodo = @Periodo";

        var turma = await _conn.QuerySingleOrDefaultAsync<ClassDetailsDTO>(sqlTurma, new { Ano = ano, Periodo = periodo });

        if (turma == null) return null;

        // 2. Lista de Alunos (Corrigido para evitar ambiguidade de CPF)
        var sqlAlunos = @"
            SELECT p.cpf, p.nome 
            FROM cursinho_each.pessoa p 
            INNER JOIN cursinho_each.aluno a ON p.cpf = a.cpf 
            WHERE a.turma_ano = @Ano AND a.turma_periodo = @Periodo
            ORDER BY p.nome";
        
        turma.Students = (await _conn.QueryAsync<ClassStudentDTO>(sqlAlunos, new { Ano = ano, Periodo = periodo })).ToList();

        // 3. Lista de Professores (CORRIGIDO PARA O SEU SCHEMA REAL)
        // Usa a tabela 'professor_materia_turma' e faz join com 'materia' e 'pessoa'
        var sqlProf = @"
            SELECT 
                p.cpf, 
                p.nome, 
                pmt.materia_nome as Materia, 
                m.area as Area
            FROM cursinho_each.professor_materia_turma pmt
            INNER JOIN cursinho_each.professor pr ON pmt.professor_cpf = pr.cpf
            INNER JOIN cursinho_each.pessoa p ON pr.cpf = p.cpf
            INNER JOIN cursinho_each.materia m ON pmt.materia_nome = m.nome
            WHERE pmt.turma_ano = @Ano AND pmt.turma_periodo = @Periodo
            ORDER BY pmt.materia_nome";
            
        turma.Teachers = (await _conn.QueryAsync<ClassTeacherDTO>(sqlProf, new { Ano = ano, Periodo = periodo })).ToList();

        return turma;
    }

    // Atualizar Capacidade
    public async Task UpdateCapacityAsync(int ano, string periodo, int novaCapacidade)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();
        var sql = "UPDATE cursinho_each.turma SET capacidade = @Cap WHERE ano = @Ano AND periodo = @Periodo";
        await _conn.ExecuteAsync(sql, new { Cap = novaCapacidade, Ano = ano, Periodo = periodo });
    }

    // Adicionar Professor à Turma (CORRIGIDO PARA O SEU SCHEMA)
    public async Task AddTeacherToClassAsync(int ano, string periodo, string cpfProfessor, string materia)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();
        
        cpfProfessor = cpfProfessor.Trim().Replace(".", "").Replace("-", "");

        // 1. Garante que a matéria existe na tabela 'materia' para não dar erro de FK
        // Se a matéria já existe, ele não faz nada (ON CONFLICT DO NOTHING)
        var sqlMateria = @"
            INSERT INTO cursinho_each.materia (nome, area) 
            VALUES (@Materia, 'Geral') 
            ON CONFLICT (nome) DO NOTHING;";
        
        await _conn.ExecuteAsync(sqlMateria, new { Materia = materia });

        // 2. Insere na tabela correta: professor_materia_turma
        var sqlVinculo = @"
            INSERT INTO cursinho_each.professor_materia_turma 
            (turma_ano, turma_periodo, professor_cpf, materia_nome) 
            VALUES (@Ano, @Periodo, @Cpf, @Materia)";
        
        await _conn.ExecuteAsync(sqlVinculo, new { Ano = ano, Periodo = periodo, Cpf = cpfProfessor, Materia = materia });
    }

    // Remover Professor (CORRIGIDO PARA O SEU SCHEMA)
    public async Task RemoveTeacherFromClassAsync(int ano, string periodo, string cpfProfessor)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();
        
        var sql = @"
            DELETE FROM cursinho_each.professor_materia_turma 
            WHERE turma_ano = @Ano 
              AND turma_periodo = @Periodo 
              AND professor_cpf = @Cpf";

        await _conn.ExecuteAsync(sql, new { Ano = ano, Periodo = periodo, Cpf = cpfProfessor });
    }
    
    public async Task DeleteClassAsync(int ano, string periodo)
    {
        if (_conn.State != System.Data.ConnectionState.Open) await _conn.OpenAsync();
        var sql = "DELETE FROM cursinho_each.turma WHERE ano = @Ano AND periodo = @Periodo";
        await _conn.ExecuteAsync(sql, new { Ano = ano, Periodo = periodo });
    }
}
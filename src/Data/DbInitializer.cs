using Dapper;
using Npgsql;

namespace CursinhoEACH.Data;
    
public class DbInitializer
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public DbInitializer(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }

    public async Task InitializeAsync()
    {
        var originalConnString = _configuration.GetConnectionString("Postgres");
        var builder = new NpgsqlConnectionStringBuilder(originalConnString);
        var targetDatabase = builder.Database; // "cursinhoeach"

        // 1. Conectar ao banco 'postgres' para criar o banco se necessário
        builder.Database = "postgres";
        var adminConnString = builder.ToString();

        await using (var adminConn = new NpgsqlConnection(adminConnString))
        {
            await adminConn.OpenAsync();

            // 2. Verificar se o banco existe
            var dbExists = await adminConn.ExecuteScalarAsync<bool>(
                "SELECT EXISTS(SELECT 1 FROM pg_database WHERE datname = @database)",
                new { database = targetDatabase });

            // 3. Criar o banco se não existir
            if (!dbExists)
            {
                Console.WriteLine($"🔧 Criando banco de dados '{targetDatabase}'...");
                await adminConn.ExecuteAsync($"CREATE DATABASE {targetDatabase}");
                Console.WriteLine($"✅ Banco de dados '{targetDatabase}' criado!");
            }
            else
            {
                Console.WriteLine($"✓ Banco de dados '{targetDatabase}' já existe.");
            }
        }

        // 4. Conectar ao banco criado e executar o script das tabelas
        await using (var conn = new NpgsqlConnection(originalConnString))
        {
            await conn.OpenAsync();

            var scriptPath = Path.Combine(_env.ContentRootPath, "Data", "script_bd.sql");

            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"Script SQL não encontrado: {scriptPath}");
            }

            Console.WriteLine("🔧 Executando script de criação de tabelas...");
            var sql = await File.ReadAllTextAsync(scriptPath);
            await conn.ExecuteAsync(sql);
            Console.WriteLine("✅ Tabelas criadas/atualizadas com sucesso!");

            // 5. Verificar se as tabelas estão vazias e popular com dados iniciais
            await PopulateInitialDataIfEmpty(conn);
        }
    }

    private async Task PopulateInitialDataIfEmpty(NpgsqlConnection conn)
    {
        var schema = "cursinho_each";
        
        // Lista de todas as tabelas principais do sistema
        var tables = new[]
        {
            "turma",
            "pessoa",
            "aluno",
            "professor",
            "materia",
            "prova",
            "evento"
        };

        Console.WriteLine("🔍 Verificando se as tabelas estão vazias...");

        // Verificar se TODAS as tabelas estão vazias
        foreach (var table in tables)
        {
            var hasData = await conn.ExecuteScalarAsync<bool>(
                $"SELECT EXISTS(SELECT 1 FROM {schema}.{table} LIMIT 1)");

            if (hasData)
            {
                Console.WriteLine($"✓ Tabela '{table}' já contém dados. Pulando carga inicial.");
                return; // Se qualquer tabela tiver dados, não carrega
            }
        }

        // Se chegou aqui, todas as tabelas estão vazias
        Console.WriteLine("📦 Todas as tabelas estão vazias. Carregando dados iniciais...");

        var initialDataPath = Path.Combine(_env.ContentRootPath, "Data", "script_initial_data_load.sql");

        if (!File.Exists(initialDataPath))
        {
            Console.WriteLine("⚠️  Script de dados iniciais não encontrado. Pulando...");
            return;
        }

        var initialDataSql = await File.ReadAllTextAsync(initialDataPath);
        await conn.ExecuteAsync(initialDataSql);
        
        Console.WriteLine("✅ Dados iniciais carregados com sucesso!");
    }
}
using Dapper;
using Npgsql;

namespace CursinhoEACH.Data;

public class DbInitializer
{
    private readonly NpgsqlConnection _conn;
    private readonly IWebHostEnvironment _env;

    // Injetamos o "env" para saber onde estão os arquivos no disco
    public DbInitializer(NpgsqlConnection conn, IWebHostEnvironment env)
    {
        _conn = conn;
        _env = env;
    }

    public async Task InitializeAsync()
    {
        if (_conn.State != System.Data.ConnectionState.Open)
            await _conn.OpenAsync();

        var filePath = Path.Combine(_env.ContentRootPath, "Data", "script_bd.sql");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"O arquivo de script SQL não foi encontrado em: {filePath}");
        }

        var sql = await File.ReadAllTextAsync(filePath);
        await _conn.ExecuteAsync(sql);
    }
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;
using Npgsql;

namespace CursinhoEACH.Controllers;

public class HomeController : Controller
{
    private readonly NpgsqlConnection _conn;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, NpgsqlConnection conn)
    {
        _logger = logger;
        _conn = conn;
    }

    public async Task<IActionResult> Index()
    {
        string result = "";

        try
        {
            await _conn.OpenAsync();
            await using var cmd = new NpgsqlCommand("SELECT @p AS \"TESTE\"", _conn);
            cmd.Parameters.AddWithValue("p", "Hello, it's me...");
            
            var value = await cmd.ExecuteScalarAsync();
            result = value?.ToString() ?? "";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar dado no banco.");
            result = "Erro ao carregar dado.";
        }
        finally
        {
            await _conn.CloseAsync();
        }

        return View(model: result);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

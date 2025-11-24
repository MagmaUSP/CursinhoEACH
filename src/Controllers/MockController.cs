using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Controllers;

[Route("Mock")]
public class MockController : Controller
{
    private readonly ILogger<MockController> _logger;
    private readonly MockService _mockService; // Injeção do Service

    public MockController(ILogger<MockController> logger, MockService mockService)
    {
        _logger = logger;
        _mockService = mockService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string codigo, string data, string turma)
    {
        try 
        {
            var mocks = await _mockService.GetAllMocksAsync(codigo, data, turma);
            
            // Mantém filtros na tela
            ViewData["FiltroCodigo"] = codigo;
            ViewData["FiltroData"] = data;
            ViewData["FiltroTurma"] = turma;

            return View(mocks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar simulados");
            TempData["Error"] = "Erro ao carregar lista de simulados.";
            return View(new List<MockDTO>());
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var detail = await _mockService.GetMockDetailsAsync(id);
            
            if (detail == null)
            {
                TempData["Error"] = "Simulado não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(detail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao carregar detalhes do simulado");
            TempData["Error"] = "Erro ao carregar estatísticas do simulado.";
            return RedirectToAction(nameof(Index));
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
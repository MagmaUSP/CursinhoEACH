using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Controllers;

[Route("Mock")]
public class MockController : Controller
{
    private readonly ILogger<MockController> _logger;

    public MockController(ILogger<MockController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("{id:int}")]
    public IActionResult Details(int id)
    {
        ViewData["SimuladoId"] = id;
        
        
        var baseDate = id switch
        {
            1 => new DateTime(2025,9,10,8,0,0),
            2 => new DateTime(2025,10,25,8,0,0),
            3 => new DateTime(2025,11,5,8,0,0),
            _ => new DateTime(2025,12,1,8,0,0)
        };
        ViewData["SimuladoData"] = baseDate.ToString("dd/MM/yyyy");
        ViewData["SimuladoHora"] = baseDate.ToString("HH:mm");
        ViewData["SimuladoTurma"] = "2025 Matutino"; 

        double mediaAtual = id switch { 1 => 50.0, 2 => 54.0, 3 => 48.0, _ => 52.0 };
        double? mediaAnterior = id switch { 1 => null, 2 => 50.0, 3 => 54.0, _ => 48.0 };
        string evolucaoStr = mediaAnterior.HasValue
            ? ((mediaAtual - mediaAnterior.Value) / mediaAnterior.Value).ToString("+0.0%;-0.0%")
            : "+0.0%"; 
        ViewData["SimuladoMediaTurma"] = mediaAtual.ToString("0.00");
        ViewData["SimuladoEvolucaoTurma"] = evolucaoStr;
        // Distribuição de questões (corretas, erradas, em branco) - placeholders estáticos
        int totalQuestoes = 120;
        int emBranco = id switch { 1 => 10, 2 => 8, 3 => 11, _ => 9 };
        int corretas = (int)Math.Round(mediaAtual); // assumindo média como acertos médios
        int erradas = Math.Max(0, totalQuestoes - emBranco - corretas);
        ViewData["SimuladoCorretas"] = corretas;
        ViewData["SimuladoErradas"] = erradas;
        ViewData["SimuladoEmBranco"] = emBranco;

        // Presença de alunos (placeholders estáticos)
        int totalAlunos = 40;
        int presentes = id switch { 1 => 37, 2 => 38, 3 => 35, _ => 36 };
        ViewData["SimuladoTotalAlunos"] = totalAlunos;
        ViewData["SimuladoPresentes"] = presentes;

        // Distribuição por matéria (absolutos: certas, erradas, em branco)
        var materias = id switch
        {
            1 => new[]
            {
                new { Materia="Matemática", Certas=8, Erradas=10, Branco=2 },
                new { Materia="Física", Certas=7, Erradas=9, Branco=1 },
                new { Materia="Química", Certas=6, Erradas=8, Branco=1 },
                new { Materia="Biologia", Certas=7, Erradas=7, Branco=1 },
                new { Materia="Português", Certas=9, Erradas=11, Branco=2 },
                new { Materia="História", Certas=6, Erradas=8, Branco=1 },
                new { Materia="Geografia", Certas=7, Erradas=7, Branco=2 }
            },
            2 => new[]
            {
                new { Materia="Matemática", Certas=9, Erradas=9, Branco=1 },
                new { Materia="Física", Certas=8, Erradas=8, Branco=1 },
                new { Materia="Química", Certas=7, Erradas=8, Branco=1 },
                new { Materia="Biologia", Certas=8, Erradas=7, Branco=1 },
                new { Materia="Português", Certas=9, Erradas=10, Branco=1 },
                new { Materia="História", Certas=6, Erradas=8, Branco=1 },
                new { Materia="Geografia", Certas=7, Erradas=8, Branco=2 }
            },
            3 => new[]
            {
                new { Materia="Matemática", Certas=7, Erradas=10, Branco=2 },
                new { Materia="Física", Certas=7, Erradas=9, Branco=2 },
                new { Materia="Química", Certas=6, Erradas=9, Branco=2 },
                new { Materia="Biologia", Certas=6, Erradas=8, Branco=1 },
                new { Materia="Português", Certas=8, Erradas=10, Branco=2 },
                new { Materia="História", Certas=7, Erradas=8, Branco=1 },
                new { Materia="Geografia", Certas=7, Erradas=7, Branco=1 }
            },
            _ => new[]
            {
                new { Materia="Matemática", Certas=8, Erradas=9, Branco=1 },
                new { Materia="Física", Certas=7, Erradas=9, Branco=1 },
                new { Materia="Química", Certas=6, Erradas=8, Branco=1 },
                new { Materia="Biologia", Certas=7, Erradas=7, Branco=1 },
                new { Materia="Português", Certas=9, Erradas=10, Branco=1 },
                new { Materia="História", Certas=7, Erradas=8, Branco=1 },
                new { Materia="Geografia", Certas=8, Erradas=7, Branco=2 }
            }
        };
        ViewData["SimuladoMateriasDist"] = materias;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
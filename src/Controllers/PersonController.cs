using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.Models;

namespace CursinhoEACH.Controllers;

[Route("Person")]
public class PersonController : Controller
{
    private readonly ILogger<PersonController> _logger;

    public PersonController(ILogger<PersonController> logger)
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
        var person = new PersonViewModel
        {
            Id = id,
            CPF = "123.456.789-00",
            Nome = "João Silva",
            Email = "joao.silva@exemplo.com",
            Telefone = "(11) 91234-5678",
            Endereco = "Rua Exemplo, 123",
            DataNascimento = "12/03/2005",
            Papel = id % 2 == 0 ? "Aluno" : "Professor",
            AnoEscolar = id % 2 == 0 ? 2 : null,
            Matriculado = id % 2 == 0 ? true : null,
            RepresentanteLegalCPF = id % 2 == 0 ? "987.654.321-00" : null,
            AnoTurma = id % 2 == 0 ? "2025" : null,
            PeriodoTurma = id % 2 == 0 ? "Matutino" : null,
            TipoProfessor = id % 2 != 0 ? "Regular" : null
        };
        return View(person);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(object _)
    {
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
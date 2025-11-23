using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;
using Npgsql;

namespace CursinhoEACH.Controllers;

[Route("Person")]
public class PersonController : Controller
{
    private readonly ILogger<PersonController> _logger;
    private readonly PersonService _personService;

    public PersonController(ILogger<PersonController> logger, PersonService personService)
    {
        _logger = logger;
        _personService = personService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string cpf, string nome, string email, string papel)
    {
        // 1. Chama o serviço passando os filtros
        var pessoas = await _personService.GetAllPersonsAsync(cpf, nome, email, papel);

        // 2. Repassa os filtros para a View (para os inputs continuarem preenchidos após filtrar)
        ViewData["FiltroCpf"] = cpf;
        ViewData["FiltroNome"] = nome;
        ViewData["FiltroEmail"] = email;
        ViewData["FiltroPapel"] = papel;

        // 3. Retorna a lista para a View
        return View(pessoas);
    }

    [HttpGet("{cpf}")]
    public async Task<IActionResult> Details(string cpf)
    {
        var person = await _personService.GetPersonByCpfAsync(cpf);

        if (person == null)
            return NotFound(); 

        return View(person);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(PersonDTO person)
    {
        try
        {
            await _personService.CreatePersonAsync(person);
            return RedirectToAction(nameof(Index)); 
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == "23505")
            {
                ModelState.AddModelError(string.Empty, $"O CPF {person.CPF} já está cadastrado.");
            }
            else if (ex.SqlState == "23503")
            {
                if (ex.ConstraintName != null && ex.ConstraintName.Contains("turma"))
                    ModelState.AddModelError(string.Empty, "A Turma selecionada (Ano/Período) não está cadastrada no banco de dados.");
                else 
                {
                    ModelState.AddModelError(string.Empty, "O CPF do Representante Legal não foi encontrado.");
                }
            }
            else
            {
                _logger.LogError(ex, "Erro de banco de dados.");
                ModelState.AddModelError(string.Empty, "Erro interno de banco de dados.");
            }

            return View(person);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Erro geral ao criar pessoa.");
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado.");
            return View(person);
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] PersonDTO2 person)
    {
        person.CPF = person.CPF?.Trim().Replace(".", "").Replace("-", "");

        Console.WriteLine($"Recebido no Controller: {person.CPF}, {person.Nome}, {person.Papel}, {person.Teacher?.Tipo}");

        try
        {
            if (person.Papel == "Aluno" && person.Student != null && !string.IsNullOrEmpty(person.Student.TurmaSelecionada))
            {
                var partes = person.Student.TurmaSelecionada.Split(' ');
                if (partes.Length >= 2 && int.TryParse(partes[0], out int ano))
                {
                    person.Student.AnoTurma = ano;
                    person.Student.PeriodoTurma = partes[1].Substring(0, 1);
                }
            }

            await _personService.UpdatePersonAsync(person);
            
            return RedirectToAction(nameof(Index)); 
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == "23503" && ex.ConstraintName.Contains("turma"))
                 ModelState.AddModelError(string.Empty, "A Turma selecionada não existe.");
            else
                 ModelState.AddModelError(string.Empty, "Erro ao atualizar dados: " + ex.Message);
            
            return View("Details", person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar");
            ModelState.AddModelError(string.Empty, "Erro inesperado.");
            return View("Details", person);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current.Id ?? HttpContext.TraceIdentifier });
    }
}
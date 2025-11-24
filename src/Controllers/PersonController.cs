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
        var pessoas = await _personService.GetAllPersonsAsync(cpf, nome, email, papel);

        ViewData["FiltroCpf"] = cpf;
        ViewData["FiltroNome"] = nome;
        ViewData["FiltroEmail"] = email;
        ViewData["FiltroPapel"] = papel;

        return View(pessoas);
    }

    [HttpGet("{cpf}")]
    public async Task<IActionResult> Details(string cpf)
    {
        var person = await _personService.GetPersonByCpfAsync(cpf);
        if (person == null) return NotFound(); 
        return View(person);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

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
                TempData["Error"] = $"O CPF {person.CPF} já está cadastrado no sistema.";
            }
            else if (ex.SqlState == "23503") 
            {
                if (ex.ConstraintName != null && ex.ConstraintName.Contains("turma"))
                    TempData["Error"] = "A Turma selecionada não existe no cadastro de turmas.";
                else 
                    TempData["Error"] = "O CPF do Representante Legal informado não foi encontrado.";
            }
            else
            {
                _logger.LogError(ex, "Erro de banco de dados.");
                TempData["Error"] = "Erro interno de banco de dados ao salvar.";
            }

            return View(person);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Erro geral ao criar pessoa.");
            TempData["Error"] = "Ocorreu um erro inesperado";
            return View(person);
        }
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromForm] PersonDTO2 person)
    {
        person.CPF = person.CPF?.Trim().Replace(".", "").Replace("-", "");

        try
        {
            // Lógica de mapeamento da turma string "2025 M" -> int/char
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
            if (ex.SqlState == "23503") // FK
                 TempData["Error"] = "Erro de relacionamento: Verifique se a Turma ou Representante Legal existem.";
            else
                 TempData["Error"] = "Erro ao atualizar dados no banco: " + ex.Message;
            
            return View("Details", person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar");
            TempData["Error"] = "Erro inesperado ao atualizar.";
            return View("Details", person);
        }
    }

    // --- NOVO MÉTODO DELETE ---
    [HttpGet("delete")]
    public async Task<IActionResult> Delete(string cpf)
    {
        if (string.IsNullOrEmpty(cpf)) return RedirectToAction(nameof(Index));

        try 
        { 
            await _personService.DeletePersonAsync(cpf); 
        }
        catch (PostgresException ex)
        {
            // Erro 23503: Violação de FK (Ex: Professor vinculado a uma turma, Aluno com notas, etc)
            if (ex.SqlState == "23503")
            {
                TempData["Error"] = "Não é possível excluir esta pessoa pois ela possui vínculos (Turmas, Notas, Presenças, etc).";
            }
            else
            {
                TempData["Error"] = "Erro de banco de dados ao excluir pessoa.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir");
            TempData["Error"] = "Erro inesperado ao excluir pessoa.";
        }
        
        return RedirectToAction(nameof(Index));
    }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
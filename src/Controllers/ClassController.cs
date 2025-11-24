using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;
using Npgsql; // Necessário para capturar PostgresException

namespace CursinhoEACH.Controllers;

[Route("Class")]
public class ClassController : Controller
{
    private readonly ILogger<ClassController> _logger;
    private readonly ClassService _classService;

    public ClassController(ILogger<ClassController> logger, ClassService classService)
    {
        _logger = logger;
        _classService = classService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int? ano, string periodo)
    {
        var turmas = await _classService.GetAllClassesAsync(ano, periodo);
        
        ViewData["FiltroAno"] = ano;
        ViewData["FiltroPeriodo"] = periodo;
        
        return View(turmas);
    }

    [HttpGet("create")]
    public IActionResult Create() => View();

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreatePost(ClassDTO turma)
    {
        if (turma.Ano < 2015 || string.IsNullOrEmpty(turma.Periodo))
        {
             ModelState.AddModelError("", "Dados inválidos.");
             return View("Create", turma);
        }

        try 
        {
            await _classService.CreateClassAsync(turma);
            return RedirectToAction(nameof(Index));
        }
        catch (PostgresException ex)
        {
            if (ex.SqlState == "23505")
            {
                ModelState.AddModelError(string.Empty, $"A turma {turma.Ano} ({turma.Periodo}) já está cadastrada.");
            }
            else if (ex.SqlState == "23503")
            {
                ModelState.AddModelError(string.Empty, "Violação de integridade referencial ao criar a turma.");
            }
            else
            {
                _logger.LogError(ex, "Erro de banco de dados ao criar turma.");
                ModelState.AddModelError(string.Empty, "Erro interno de banco de dados.");
            }
            
            return View("Create", turma);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro geral ao criar turma");
            ModelState.AddModelError(string.Empty, "Ocorreu um erro inesperado.");
            return View("Create", turma);
        }
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> Details(string key)
    {
        if (!TryParseKey(key, out int ano, out string periodo)) 
            return RedirectToAction(nameof(Index));

        var details = await _classService.GetClassDetailsAsync(ano, periodo);
        if (details == null) return NotFound();

        return View(details);
    }
    
    [HttpPost("UpdateCapacity")]
    public async Task<IActionResult> UpdateCapacity([FromBody] UpdateCapacityModel model)
    {
        if (!TryParseKey(model.Key, out int ano, out string periodo)) return BadRequest();
        
        try {
            await _classService.UpdateCapacityAsync(ano, periodo, model.Capacidade);
            return Ok();
        } catch { return BadRequest(); }
    }

    [HttpPost("AddTeacher")]
    public async Task<IActionResult> AddTeacher(string Key, string Materia, string ProfessorCPF)
    {
        if (TryParseKey(Key, out int ano, out string periodo))
        {
            try {
                await _classService.AddTeacherToClassAsync(ano, periodo, ProfessorCPF, Materia);
            } 
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23503")
                    TempData["Error"] = "Professor não encontrado ou Matéria inválida.";
                else if (ex.SqlState == "23505")
                    TempData["Error"] = "Este professor já está vinculado a esta matéria nesta turma.";
                else
                    TempData["Error"] = "Erro ao adicionar professor.";
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Erro ao adicionar professor");
                TempData["Error"] = "Erro inesperado ao adicionar professor.";
            }
        }
        return Redirect($"/Class/{Key}");
    }

    [HttpGet("RemoveTeacher")]
    public async Task<IActionResult> RemoveTeacher(string key, string cpf)
    {
        if (TryParseKey(key, out int ano, out string periodo))
        {
            await _classService.RemoveTeacherFromClassAsync(ano, periodo, cpf);
        }
        return Redirect($"/Class/{key}");
    }
    
    [HttpGet("Delete")]
    public async Task<IActionResult> Delete(string key)
    {
        if (TryParseKey(key, out int ano, out string periodo))
        {
            try 
            { 
                await _classService.DeleteClassAsync(ano, periodo); 
            }
            catch (PostgresException ex)
            {
                if (ex.SqlState == "23503")
                {
                    TempData["Error"] = "Não é possível excluir esta turma pois existem alunos ou professores vinculados a ela.";
                }
                else
                {
                    TempData["Error"] = "Erro de banco de dados ao excluir a turma.";
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "Erro inesperado ao excluir a turma.";
            }
        }
        return RedirectToAction(nameof(Index));
    }

    private bool TryParseKey(string key, out int ano, out string periodo)
    {
        ano = 0; periodo = "";
        if (string.IsNullOrEmpty(key) || key.Length < 5) return false;
        
        var anoStr = key.Substring(0, 4);
        periodo = key.Substring(4);

        return int.TryParse(anoStr, out ano);
    }
    
    public class UpdateCapacityModel { public string Key { get; set; } public int Capacidade { get; set; } }

    [Route("Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
#nullable enable
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.Services;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ScheduleService _scheduleService;

        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        public async Task<IActionResult> Index(DateTime? weekStart, string? periodo)
        {
            if (weekStart.HasValue)
            {
                var dayOfWeek = (int)weekStart.Value.DayOfWeek;
                var diff = dayOfWeek == 0 ? -6 : 1 - dayOfWeek;
                weekStart = weekStart.Value.AddDays(diff);
            }

            var model = new ScheduleViewModel
            {
                WeekStart = weekStart,
                Periodo = periodo
            };

            if (!string.IsNullOrEmpty(periodo) && weekStart.HasValue)
            {
                var ano = weekStart.Value.Year;
                model.Professores = await _scheduleService.GetProfessoresDisponiveisAsync(ano, periodo);
                model.GradeExistente = await _scheduleService.GetGradeExistenteAsync(ano, periodo, weekStart.Value);
                
                // DEBUG
                Console.WriteLine($"========== DEBUG SCHEDULE ==========");
                Console.WriteLine($"Ano: {ano}, Periodo: {periodo}, WeekStart: {weekStart.Value:yyyy-MM-dd}");
                Console.WriteLine($"Professores carregados: {model.Professores.Count}");
                Console.WriteLine($"Aulas existentes: {model.GradeExistente.Count}");
                foreach (var aula in model.GradeExistente)
                {
                    Console.WriteLine($"  Aula: Prof={aula.ProfessorCpf}, Materia={aula.Materia}, Data={aula.DataEvento:yyyy-MM-dd}, Horario={aula.HorarioInicio}, DiaSemana={aula.DiaSemana}");
                }
                Console.WriteLine($"====================================");
                
                if (model.Professores.Count == 0)
                {
                    TempData["Warning"] = $"Nenhum professor regular cadastrado para a turma {ano} - {(periodo == "V" ? "Vespertino" : "Noturno")}.";
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveSchedule([FromBody] SaveScheduleRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Periodo) || string.IsNullOrEmpty(request.WeekStart))
                {
                    return BadRequest(new { success = false, message = "Período e semana são obrigatórios." });
                }

                if (request.Aulas == null || request.Aulas.Count == 0)
                {
                    return BadRequest(new { success = false, message = "Adicione pelo menos uma aula na grade." });
                }

                var weekStart = DateTime.Parse(request.WeekStart);
                var ano = weekStart.Year;
                
                await _scheduleService.AtualizarGradeHorariaAsync(ano, request.Periodo, weekStart, request.Aulas);
                return Ok(new { success = true, message = "Grade horária salva com sucesso!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"Erro ao salvar: {ex.Message}" });
            }
        }
    }

    public class SaveScheduleRequest
    {
        public string Periodo { get; set; } = string.Empty;
        public string WeekStart { get; set; } = string.Empty;
        public List<AulaAgendadaDTO> Aulas { get; set; } = new();
    }
}
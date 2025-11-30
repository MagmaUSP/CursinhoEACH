#nullable enable
using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.Services;

namespace CursinhoEACH.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly AttendanceService _attendanceService;

        public AttendanceController(AttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task<IActionResult> Index(
            DateTime? startDate,
            DateTime? endDate,
            string? periodo,
            string? nome,
            string? presenca)
        {
            bool? presente = presenca switch
            {
                "Presentes" => true,
                "Ausentes" => false,
                _ => null
            };

            var records = await _attendanceService.GetAttendanceRecordsAsync(
                startDate, endDate, periodo, nome, presente);

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            ViewBag.Periodo = periodo;
            ViewBag.Nome = nome;
            ViewBag.Presenca = presenca;

            return View(records);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAttendance(string cpf, long eventoId)
        {
            var success = await _attendanceService.MarkAttendanceAsync(cpf, eventoId);
            
            if (success)
                TempData["Success"] = "Presença marcada com sucesso!";
            else
                TempData["Error"] = "Erro ao marcar presença.";

            return RedirectToAction(nameof(Index));
        }
    }
}
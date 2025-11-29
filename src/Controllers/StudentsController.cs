using Microsoft.AspNetCore.Mvc;
using CursinhoEACH.DTO;

namespace CursinhoEACH.Controllers
{
    [Route("Students")]
    public class StudentsController : Controller
    {
        private readonly StudentService _studentService;

        public StudentsController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string cpf, string nome, string email, int ano, string periodo)
        {
            // 1. Busca os dados de sumário (Para os gráficos do topo)
            var dashboardData = await _studentService.GetDashboardSummaryAsync(cpf, nome, email, ano, periodo);

            // 2. Busca a lista de alunos (Para a tabela)
            var studentsList = await _studentService.GetAllStudentsAsync(cpf, nome, email, ano, periodo);
            
            // 3. Monta o ViewModel
            var viewModel = new StudentsIndexViewModel
            {
                Dashboard = dashboardData,
                Students = studentsList
            };

            // Mantém os filtros na View
            ViewData["FiltroCpf"] = cpf;
            ViewData["FiltroNome"] = nome;
            ViewData["FiltroEmail"] = email;
            ViewData["FiltroAno"] = ano;
            ViewData["FiltroPeriodo"] = periodo;

            return View(viewModel);
        }

        [HttpGet("{cpf}")]
        public async Task<IActionResult> Details(string cpf)
        {
            var profile = await _studentService.GetStudentProfileAsync(cpf);
            
            if (profile == null) 
            {
                return NotFound();
            }

            ViewData["Title"] = "Detalhes do Aluno";
            return View(profile);
        }
    }
}
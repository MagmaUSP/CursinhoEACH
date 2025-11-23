using Microsoft.AspNetCore.Mvc;

namespace CursinhoEACH.Controllers
{
    [Route("Students")]
    public class StudentsController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet("{cpf}")]
        public IActionResult Details(string cpf)
        {
            ViewData["Title"] = "Detalhes do Aluno";
                ViewData["AlunoCpf"] = cpf;
                var alunos = new Dictionary<string, (string Nome, string Email, string? Telefone)>
                {
                    {"123.456.789-00", ("João Silva", "joao.silva@exemplo.com", "(11) 91234-5678")},
                    {"987.654.321-11", ("Maria Oliveira", "maria.oliveira@exemplo.com", null)},
                    {"111.222.333-44", ("Pedro Santos", "pedro.santos@exemplo.com", "(11) 99876-5432")},
                    {"222.333.444-55", ("Ana Souza", "ana.souza@exemplo.com", null)}
                };
                if(alunos.TryGetValue(cpf, out var dados))
                {
                    ViewData["AlunoNome"] = dados.Nome;
                    ViewData["AlunoEmail"] = dados.Email;
                    if (!string.IsNullOrWhiteSpace(dados.Telefone)) ViewData["AlunoTelefone"] = dados.Telefone;
                }
                else
                {
                    ViewData["AlunoNome"] = "Aluno não encontrado";
                    ViewData["AlunoEmail"] = "--";
                }
            return View();
        }
    }
}
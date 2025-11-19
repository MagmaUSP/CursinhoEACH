namespace CursinhoEACH.Models;

public class PersonViewModel
{
    public int Id { get; set; }
    public string? CPF { get; set; }
    public string Nome { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Telefone { get; set; }
    public string? Endereco { get; set; }
    public string? DataNascimento { get; set; }
    public string Papel { get; set; } = "Aluno"; // Aluno | Professor | Responsável
    public int? AnoEscolar { get; set; }
    public bool? Matriculado { get; set; }
    public string? MotivoDesligamento { get; set; }
    public string? RepresentanteLegalCPF { get; set; }
    public string? AnoTurma { get; set; }
    public string? PeriodoTurma { get; set; }
    public string? TipoProfessor { get; set; }
}
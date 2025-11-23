namespace CursinhoEACH.DTO;

public class PersonDTO
{
    public int Id { get; set; }
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }

    // CORREÇÃO 1: Adicionado '?' (DateOnly? aceita data vazia)
    public DateTime? DataNascimento { get; set; }

    public string Papel { get; set; } 

    // CORREÇÃO 2: Adicionado '?' (int? aceita vir nulo do formulário)
    public int? AnoEscolar { get; set; }

    // CORREÇÃO 3: Adicionado '?' (bool? evita erro se o checkbox não for enviado)
    public bool? Matriculado { get; set; }

    public string MotivoDesligamento { get; set; }
    public string RepresentanteLegalCPF { get; set; }

    // CORREÇÃO 4: Adicionado '?' (Essenciais para quando for Professor/Responsável)
    public int? AnoTurma { get; set; }
    
    public string PeriodoTurma { get; set; }
    public string TipoProfessor { get; set; }
    public string Turma { get; set; } 
}
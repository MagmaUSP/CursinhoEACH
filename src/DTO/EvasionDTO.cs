namespace CursinhoEACH.DTO;

public class EvasionDTO
{
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; } // NOVO: Necessário para enviar o alerta
    public string Turma { get; set; }
    
    // Métricas
    public double IAA { get; set; } 
    public double IAS { get; set; } 
    
    public string Risco { get; set; } 
    
    // Detalhes para o corpo do e-mail
    public int TotalAulas { get; set; }
    public int PresencasAulas { get; set; }
    public int TotalSimulados { get; set; }
    public int PresencasSimulados { get; set; }
}
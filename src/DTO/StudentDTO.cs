namespace CursinhoEACH.DTO;

public class StudentsIndexViewModel
{
    public DashboardSummaryDTO Dashboard { get; set; }
    public IEnumerable<StudentSummaryDTO> Students { get; set; }
}

public class DashboardSummaryDTO
{
    public int TotalQuestoesRespondidas { get; set; }
    public int TotalCertas { get; set; }
    public int TotalErradas { get; set; }
    public int TotalBranco { get; set; }

    public int TotalOportunidadesPresenca { get; set; } 
    public int TotalPresencasRegistradas { get; set; }
    
    // CORREÇÃO: Retorna 0 se o total for 0 (evita divisão por zero e gráfico cheio)
    public double PercCertas => TotalQuestoesRespondidas > 0 ? (double)TotalCertas / TotalQuestoesRespondidas * 100 : 0;
    public double PercErradas => TotalQuestoesRespondidas > 0 ? (double)TotalErradas / TotalQuestoesRespondidas * 100 : 0;
    public double PercBranco => TotalQuestoesRespondidas > 0 ? (double)TotalBranco / TotalQuestoesRespondidas * 100 : 0;
    
    // CORREÇÃO: Se não tiver aula, a presença é 0% (ou N/A), não 100%
    public double PercPresenca => TotalOportunidadesPresenca > 0 ? (double)TotalPresencasRegistradas / TotalOportunidadesPresenca * 100 : 0;
    public double PercAusencia => TotalOportunidadesPresenca > 0 ? 100 - PercPresenca : 0;
}

public class StudentSummaryDTO
{
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public double IAA { get; set; } 
    public double IAS { get; set; } 
}

public class StudentProfileDTO
{
    public string Nome { get; set; }
    public string CPF { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Turma { get; set; }
    public int QtdCertas { get; set; }
    public int QtdErradas { get; set; }
    public int QtdBranco { get; set; }
    public int TotalQuestoes => QtdCertas + QtdErradas + QtdBranco;
    public double PercPresenca { get; set; }
    public double PercAusencia => 100 - PercPresenca;
    public List<SubjectPerformanceDTO> DesempenhoPorMateria { get; set; } = new();
    public List<MockExamHistoryDTO> HistoricoSimulados { get; set; } = new();
    public List<SubjectAttendanceDTO> AssiduidadePorMateria { get; set; } = new();
}

public class SubjectPerformanceDTO
{
    public string Materia { get; set; }
    public int Total { get; set; }
    public int Certas { get; set; }
    public int Erradas { get; set; }
    public int Branco { get; set; }
    public double PercCertas => Total > 0 ? (double)Certas / Total * 100 : 0;
    public double PercErradas => Total > 0 ? (double)Erradas / Total * 100 : 0;
    public double PercBranco => Total > 0 ? (double)Branco / Total * 100 : 0;
}

public class MockExamHistoryDTO
{
    public int EventoId { get; set; }
    public string Titulo { get; set; }
    public DateTime Data { get; set; }
    public int TotalQuestoes { get; set; }
    public int Acertos { get; set; }
    public int Erros { get; set; }
    public int Branco { get; set; }
    public double Media => TotalQuestoes > 0 ? (double)Acertos / TotalQuestoes * 100 : 0;
    public List<SubjectPerformanceDTO> Materias { get; set; } = new();
}

public class SubjectAttendanceDTO
{
    public string Materia { get; set; }
    public string Professor { get; set; }
    public int AulasDadas { get; set; }
    public int Presencas { get; set; }
    public double Porcentagem => AulasDadas > 0 ? (double)Presencas / AulasDadas * 100 : 0;
}
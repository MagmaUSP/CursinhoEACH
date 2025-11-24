namespace CursinhoEACH.DTO;

public class MockDTO
{
    public long Id { get; set; } 
    public string Codigo { get; set; } 
    public DateTime Data { get; set; }
    public string Turma { get; set; } 
}

public class MockDetailDTO
{
    public long Id { get; set; }
    public string Titulo { get; set; }
    public DateTime Data { get; set; }
    public string Turma { get; set; }
    
    public double MediaGeral { get; set; }
    public double Evolucao { get; set; } 
    public int TotalAlunos { get; set; }
    public int Presentes { get; set; }

    public int TotalQuestoesRespondidas { get; set; }
    public int QtdCertas { get; set; }
    public int QtdErradas { get; set; }
    public int QtdBranco { get; set; }

    // Porcentagens para exibição
    public double PctCertas => TotalQuestoesRespondidas > 0 ? (double)QtdCertas / TotalQuestoesRespondidas * 100 : 0;
    public double PctErradas => TotalQuestoesRespondidas > 0 ? (double)QtdErradas / TotalQuestoesRespondidas * 100 : 0;
    public double PctBranco => TotalQuestoesRespondidas > 0 ? (double)QtdBranco / TotalQuestoesRespondidas * 100 : 0;

    // --- ÂNGULOS PARA O GRÁFICO (CSS conic-gradient) ---
    // O gráfico precisa de pontos de parada (stops) em graus.
    
    // Onde termina o Azul (Certas)
    public int AngleCertas => (int)(PctCertas * 3.6);
    
    // Onde termina o Laranja (Certas + Erradas)
    // Ex: Se acertou 90° e errou 90°, o laranja vai do 90° até o 180°
    public int EndAngleOrange => AngleCertas + (int)(PctErradas * 3.6);

    public List<SubjectStatDTO> Materias { get; set; } = new();
}

public class SubjectStatDTO
{
    public string Materia { get; set; }
    public int Certas { get; set; }
    public int Erradas { get; set; }
    public int Branco { get; set; }
    
    public int Total => Certas + Erradas + Branco;
    public double PctCertas => Total > 0 ? (double)Certas / Total * 100 : 0;
    public double PctErradas => Total > 0 ? (double)Erradas / Total * 100 : 0;
    public double PctBranco => Total > 0 ? (double)Branco / Total * 100 : 0;
}
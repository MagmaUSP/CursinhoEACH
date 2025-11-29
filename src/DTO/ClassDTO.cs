namespace CursinhoEACH.DTO;

public class ClassDTO
{
    public int Ano { get; set; }
    public string Periodo { get; set; } // 'M', 'V', 'N'
    public int Capacidade { get; set; }
    public int Matriculados { get; set; } // Contagem de alunos

    public string PeriodoExtenso => Periodo switch
    {
        "M" => "Matutino",
        "V" => "Vespertino",
        "N" => "Noturno",
        _ => Periodo
    };

    public string Key => $"{Ano}{Periodo}"; // Ex: 2025M

    public int OcupacaoPorcentagem => Capacidade > 0 ? (int)(((double)Matriculados / Capacidade) * 100) : 0;
}

public class ClassDetailsDTO : ClassDTO
{
    public List<ClassStudentDTO> Students { get; set; } = new();
    public List<ClassTeacherDTO> Teachers { get; set; } = new();
}

public class ClassStudentDTO
{
    public string CPF { get; set; }
    public string Nome { get; set; }
}

public class ClassTeacherDTO
{
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Materia { get; set; }
    public string Area { get; set; }
    public string AreaExtenso => Area switch 
    {
        "H" => "Humanas",
        "E" => "Exatas",
        "N" => "Natureza",
        "L" => "Linguagens",
        "O" => "Outros",
        _ => Area
    };
}
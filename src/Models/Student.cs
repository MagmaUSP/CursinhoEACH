namespace CursinhoEACH.Models;

public class Student
{
    public string Cpf { get; set; } // PK e FK para Person
    public int AnoEscolar { get; set; }
    public bool Matriculado { get; set; }
    public bool Desligado { get; set; }
    public string DesligadoMotivo { get; set; }
    public string RepresentanteLegal { get; set; } // FK para Person(cpf)
    public int TurmaAno { get; set; }
    public string TurmaPeriodo { get; set; }
}
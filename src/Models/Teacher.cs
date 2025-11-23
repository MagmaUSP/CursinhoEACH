namespace CursinhoEACH.Models;

public class Teacher 
{
    public string Cpf { get; set; } // PK e FK para Person
    public string Tipo { get; set; } // 'R', 'M', 'P'
}
using System;
using CursinhoEACH.Models;

namespace CursinhoEACH.DTO;

public class PersonDTO2
{
    // Dados Comuns (Tabela Pessoa)
    public string CPF { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public string Endereco { get; set; }
    public string Papel { get; set; } 
    public DateTime? DataNascimento { get; set; }

    // Objetos Aninhados
    public TeacherDTO Teacher { get; set; }
    public StudentDTO Student { get; set; }

    public PersonDTO2()
    {
        // Inicializa para evitar NullReferenceException se tentar acessar propriedades diretas
        Teacher = new TeacherDTO();
        Student = new StudentDTO();
    }
}

public class StudentDTO 
{
    public int? AnoEscolar { get; set; }
    public bool? Matriculado { get; set; } // bool? para aceitar nulo do form
    public string DesligadoMotivo { get; set; } // Ajustei nome para bater com o Controller antigo se precisar
    public string RepresentanteLegal { get; set; } // Ajustei nome
    
    // Campos de Banco
    public int? AnoTurma { get; set; }
    public string PeriodoTurma { get; set; }
    
    // Campo auxiliar para o formulário (aquele select "2025 Matutino")
    public string TurmaSelecionada { get; set; } 
}

public class TeacherDTO
{
    public string Tipo { get; set; } // Ajustei nome para bater com "Tipo" do banco
}
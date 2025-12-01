#nullable enable
namespace CursinhoEACH.DTO
{
    public class ProfessorScheduleDTO
    {
        public string Cpf { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Materia { get; set; } = string.Empty;
    }

    public class ScheduleViewModel
    {
        public List<ProfessorScheduleDTO> Professores { get; set; } = new();
        public List<AulaAgendadaDTO> GradeExistente { get; set; } = new();
        public DateTime? WeekStart { get; set; }
        public string? Periodo { get; set; }
        public int Ano { get; set; }
    }

    public class AulaAgendadaDTO
    {
        public string ProfessorCpf { get; set; } = string.Empty;
        public string Materia { get; set; } = string.Empty;
        public int DiaSemana { get; set; }
        public string HorarioInicio { get; set; } = string.Empty; // MUDOU: era TimeSpan, agora é string
        public DateTime DataEvento { get; set; }
    }
}
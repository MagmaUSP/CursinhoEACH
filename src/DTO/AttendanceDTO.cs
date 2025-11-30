namespace CursinhoEACH.DTO
{
    public class AttendanceDTO
    {
        public string Nome { get; set; } = string.Empty;
        public int AnoTurma { get; set; }
        public string Periodo { get; set; } = string.Empty;
        public string Evento { get; set; } = string.Empty;
        public DateTime DataEvento { get; set; }
        public bool Presente { get; set; }
        public long EventoId { get; set; }
        public string Cpf { get; set; } = string.Empty;
    }
}
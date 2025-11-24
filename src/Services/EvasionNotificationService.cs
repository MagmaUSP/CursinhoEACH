using CursinhoEACH.DTO;
using System.Text;

public class EvasionNotificationService
{
    private readonly EvasionService _evasionService;
    private readonly EmailService _emailService;
    private const string EmailCoordenacao = "x";

    public EvasionNotificationService(EvasionService evasionService, EmailService emailService)
    {
        _evasionService = evasionService;
        _emailService = emailService;
    }

    public async Task<int> ProcessAndSendReportsAsync()
    {
        // 1. Busca dados (já calculados pelo EvasionService conforme sua regra de IAA/IAS)
        var report = await _evasionService.GetEvasionReportAsync();
        int emailsEnviados = 0;

        // 2. Filtra e envia alertas individuais para alunos (Risco Alto)
        var alunosRiscoAlto = report.Where(x => x.Risco == "Alto").ToList();

        foreach (var aluno in alunosRiscoAlto)
        {
            if (!string.IsNullOrEmpty(aluno.Email))
            {
                var corpoAluno = BuildStudentEmailBody(aluno);
                await _emailService.SendEmailAsync(aluno.Email, "Alerta de Frequência - Cursinho EACH", corpoAluno);
                emailsEnviados++;
            }
        }

        // 3. Gera e envia Relatório Geral para a Coordenação
        var corpoCoordenacao = BuildCoordinationEmailBody(report, alunosRiscoAlto.Count);
        await _emailService.SendEmailAsync(EmailCoordenacao, "Relatório Semanal de Evasão", corpoCoordenacao);

        return emailsEnviados;
    }

    private string BuildStudentEmailBody(EvasionDTO aluno)
    {
        return $@"
            <div style='font-family: Arial, sans-serif;'>
                <h3>Olá, {aluno.Nome}.</h3>
                <p>Identificamos que sua frequência está abaixo do esperado nas últimas semanas.</p>
                <ul>
                    <li><strong>Frequência em Aulas (IAA):</strong> {aluno.IAA:F1}%</li>
                    <li><strong>Frequência em Simulados (IAS):</strong> {aluno.IAS:F1}%</li>
                </ul>
                <p style='color: red;'><strong>Situação de Risco: {aluno.Risco}</strong></p>
                <p>Por favor, procure a coordenação pedagógica para conversarmos e montarmos um plano de estudos.</p>
                <p><em>Equipe Cursinho EACH</em></p>
            </div>";
    }

    private string BuildCoordinationEmailBody(IEnumerable<EvasionDTO> report, int countRiscoAlto)
    {
        var sb = new StringBuilder();
        sb.Append("<div style='font-family: Arial, sans-serif;'>");
        sb.Append("<h3>Relatório Semanal de Evasão e Frequência</h3>");
        sb.Append($"<p><strong>Data:</strong> {DateTime.Now:dd/MM/yyyy} | <strong>Alunos em Risco Alto:</strong> {countRiscoAlto}</p>");
        sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse; width: 100%;'>");
        sb.Append("<tr style='background-color:#333; color: white;'><th>Aluno</th><th>Turma</th><th>IAA (Aulas)</th><th>IAS (Simulados)</th><th>Risco</th></tr>");

        foreach (var item in report.OrderByDescending(r => r.Risco == "Alto").ThenBy(r => r.Nome))
        {
            string bg = item.Risco == "Alto" ? "#ffcccc" : (item.Risco == "Médio" ? "#fff4cc" : "#ffffff");
            sb.Append($"<tr style='background-color:{bg};'>");
            sb.Append($"<td>{item.Nome}</td><td>{item.Turma}</td>");
            sb.Append($"<td>{item.IAA:F1}%</td><td>{item.IAS:F1}%</td>");
            sb.Append($"<td><b>{item.Risco}</b></td></tr>");
        }
        sb.Append("</table></div>");
        return sb.ToString();
    }
}
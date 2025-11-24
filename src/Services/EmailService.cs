using System.Net;
using System.Net.Mail;
using System.Text;

public class EmailService
{
    private readonly ILogger<EmailService> _logger;
    
    // Configurações (Idealmente ficariam no appsettings.json) sacs pxqr afkj hrul
    private const string SmtpHost = "smtp.gmail.com";
    private const string SmtpPort = "587";
    private const string SmtpUser = "seu-email@gmail.com"; // ALTERE AQUI
    private const string SmtpPass = "senha";    // ALTERE AQUI

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string para, string assunto, string corpoHtml)
    {
        try
        {
            // Para TESTES: Apenas loga no console se não tiver configuração real
            if (SmtpUser == "seu-email@gmail.com")
            {
                _logger.LogWarning($"[SIMULAÇÃO DE EMAIL] Para: {para} | Assunto: {assunto}");
                // Console.WriteLine(corpoHtml); 
                return;
            }

            using var message = new MailMessage();
            message.From = new MailAddress(SmtpUser, "Cursinho EACH - Pedagógico");
            message.To.Add(new MailAddress(para));
            message.Subject = assunto;
            message.Body = corpoHtml;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(SmtpHost, int.Parse(SmtpPort));
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(SmtpUser, SmtpPass);

            await client.SendMailAsync(message);
            _logger.LogInformation($"Email enviado para {para}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao enviar email para {para}");
            throw; // Repassa o erro para o controller saber
        }
    }
}
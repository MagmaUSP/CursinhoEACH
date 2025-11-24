using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class WeeklyJobService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WeeklyJobService> _logger;
    
    // Configuração do dia e hora do disparo (Ex: Segunda-feira às 08:00)
    private readonly DayOfWeek _dayToSend = DayOfWeek.Monday;
    private readonly int _hourToSend = 8; 

    public WeeklyJobService(IServiceProvider serviceProvider, ILogger<WeeklyJobService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço de E-mails Semanais Iniciado.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = CalculateDelay();
            _logger.LogInformation($"Próximo envio agendado para daqui a {delay.TotalHours:F2} horas.");

            // Espera até a hora do envio
            await Task.Delay(delay, stoppingToken);

            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                _logger.LogInformation("Iniciando envio semanal de relatórios...");

                // IMPORTANTE: Criamos um escopo porque EvasionNotificationService usa banco de dados (Scoped)
                // e este BackgroundService é Singleton.
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetRequiredService<EvasionNotificationService>();
                    
                    int enviados = await notificationService.ProcessAndSendReportsAsync();
                    _logger.LogInformation($"Processo finalizado com sucesso. {enviados} alunos notificados.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro fatal ao tentar enviar relatórios semanais.");
            }
        }
    }

    private TimeSpan CalculateDelay()
    {
        var now = DateTime.Now;
        var target = DateTime.Today.AddHours(_hourToSend);

        // Se já passou do horário de hoje, ou se hoje não é o dia certo
        while (target <= now || target.DayOfWeek != _dayToSend)
        {
            target = target.AddDays(1);
        }

        return target - now;
    }
}
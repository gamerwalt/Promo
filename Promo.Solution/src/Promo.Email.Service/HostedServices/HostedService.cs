using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promo.Email.Service.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Email.Service.HostedServices
{
    public class HostedService : IHostedService
    {
        private readonly IMailService _mailService;
        private readonly ILogger<HostedService> _logger;
        private readonly IHostApplicationLifetime _appLifeTime;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public HostedService(IMailService mailService, ILogger<HostedService> logger, IHostApplicationLifetime appLifeTime)
        {
            _mailService = mailService;
            _logger = logger;
            _appLifeTime = appLifeTime;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifeTime.ApplicationStarted.Register(OnStarted);
            _appLifeTime.ApplicationStopping.Register(OnStopping);
            _appLifeTime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        private void OnStopped() => _logger.LogInformation("OnStopped has been called");

        private void OnStopping()
        {
            _cancellationTokenSource.Cancel();
            _logger.LogInformation("OnStopping has been called.");
        }

        private void OnStarted()
        {
            _logger.LogInformation("OnStarted has been called");

            Task.Run(async () => await _mailService.ProcessAnyUnsentEmails(_cancellationTokenSource.Token)).Wait();

            _appLifeTime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

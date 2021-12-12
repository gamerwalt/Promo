using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promo.Email.Service.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Email.Service.HostedServices
{
    public class PromoHostedService : IHostedService
    {
        private readonly IVerifyAccountService _verifyUserService;
        private readonly IMailService _mailService;
        private readonly ILogger<PromoHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifeTime;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public PromoHostedService(IVerifyAccountService verifyUserService, IMailService mailService, ILogger<PromoHostedService> logger, IHostApplicationLifetime appLifeTime)
        {
            _verifyUserService = verifyUserService;
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

            Task.Run(async () => await _verifyUserService.VerifyAnyUnverifiedAccounts(_cancellationTokenSource.Token)).Wait();
            Task.Run(async () => await _mailService.ProcessAnyUnsentEmails(_cancellationTokenSource.Token)).Wait();

            _appLifeTime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

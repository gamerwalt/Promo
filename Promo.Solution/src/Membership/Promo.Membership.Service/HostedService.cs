using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Membership.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Service
{
    public class HostedService : IHostedService
    {
        private readonly IVerifyAccountService _verifyUserService;
        private readonly IMailClient _mailClient;
        private readonly ILogger<HostedService> _logger;
        private readonly IHostApplicationLifetime _appLifeTime;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public HostedService(
            IVerifyAccountService verifyUserService, 
            IMailClient mailClient, 
            ILogger<HostedService> logger, 
            IHostApplicationLifetime appLifeTime)
        {
            _verifyUserService = verifyUserService;
            _mailClient = mailClient;
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

            _appLifeTime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

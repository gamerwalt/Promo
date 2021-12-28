using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Harvester.Service
{
    public class HarvesterHostedService : IHostedService
    {
        private readonly IStreamService _streamService;
        private readonly ILogger<HarvesterHostedService> _logger;
        private readonly IHostApplicationLifetime _appLifeTime;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public HarvesterHostedService(
            IStreamService streamService,
            ILogger<HarvesterHostedService> logger, 
            IHostApplicationLifetime appLifeTime)
        {
            _streamService = streamService;
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

            Task.Run(async () => await _streamService.StartStreamingAsync(_cancellationTokenSource.Token)).Wait();

            _appLifeTime.StopApplication();
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

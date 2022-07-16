using hrvster.Models.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Twitter.Harvester
{
	public class HostedService : IHostedService
	{
		private readonly ILogger<HostedService> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly IService _service;

		public HostedService(
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime appLifeTime,
			IService services)
		{
			_logger = loggerFactory.CreateLogger<HostedService>();
			_appLifeTime = appLifeTime;
			_cancellationTokenSource = new CancellationTokenSource();
			_service = services;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Startasync started...");
			_appLifeTime.ApplicationStarted.Register(OnStarted);
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private async void OnStarted()
		{
			_logger.LogInformation("OnStarted has been called");

			var tasks = new List<Task>();

			Task.Run(() => _service.ProcessService(_cancellationTokenSource.Token)).Wait();

			_appLifeTime.StopApplication();
		}

		private void OnStopped() => _logger.LogInformation("OnStopped has been called");

		private void OnStopping()
		{
			_cancellationTokenSource.Cancel();
			_logger.LogInformation("OnStopping has been called.");
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}

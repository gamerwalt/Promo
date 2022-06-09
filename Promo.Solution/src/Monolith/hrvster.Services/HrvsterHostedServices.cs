using hrvster.Models.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class HrvsterHostedServices : IHostedService
	{
		private readonly ILogger<HrvsterHostedServices> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly IEnumerable<IService> _services;
		private readonly ServiceContainer _serviceContainer;

		public HrvsterHostedServices(
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime appLifeTime,
			IEnumerable<IService> services,
			ServiceContainer serviceContainer)
		{
			_logger = loggerFactory.CreateLogger<HrvsterHostedServices>();
			_appLifeTime = appLifeTime;
			_cancellationTokenSource = new CancellationTokenSource();
			_services = services;
			_serviceContainer = serviceContainer;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_appLifeTime.ApplicationStarted.Register(OnStarted);
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private void OnStarted()
		{
			_logger.LogInformation("OnStarted has been called");

			foreach(var service in _services)
			{
				var serviceFound = _serviceContainer.Services.FirstOrDefault(s => s.FQN.Equals(service.GetType().FullName));

				if (serviceFound != null && serviceFound.Enabled)
				{
					_logger.LogInformation("Running {serviceName}...", service.GetType().FullName);

					Task.Run(async () => await service.ProcessService(_cancellationTokenSource.Token)).Wait();
				}
				else
				{
					_logger.LogInformation("{serviceName} is not enabled so we will skip it's run...", service.GetType().FullName);
				}
			}

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

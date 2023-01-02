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
	public class HostedServices : IHostedService
	{
		private readonly ILogger<HostedServices> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly IEnumerable<IService> _services;
		private readonly ServiceContainer _serviceContainer;

		public HostedServices(
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime appLifeTime,
			IEnumerable<IService> services,
			ServiceContainer serviceContainer)
		{
			_logger = loggerFactory.CreateLogger<HostedServices>();
			_appLifeTime = appLifeTime;
			_cancellationTokenSource = new CancellationTokenSource();
			_services = services;
			_serviceContainer = serviceContainer;
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

			foreach(var service in _services)
			{
				var serviceFound = _serviceContainer.Services.FirstOrDefault(s => s.FQN.Equals(service.GetType().FullName));

				if (serviceFound != null && serviceFound.Enabled)
				{
					_logger.LogInformation("Adding {serviceName}... to List of Tasks", service.GetType().FullName);

					tasks.Add(service.ProcessService(_cancellationTokenSource.Token));
					//Task.Run(() => service.ProcessService(_cancellationTokenSource.Token));
				}
				else
				{
					_logger.LogInformation("{serviceName} is not enabled so we will skip it's run...", service.GetType().FullName);
				}
			}
			
			await Task.WhenAll(tasks.ToArray());

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

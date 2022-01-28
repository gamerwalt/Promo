using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Promo.Sweeps.Draw.Service;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Service
{
	public class DrawHostedService : IHostedService
	{
		private readonly IDrawService _drawService;
		private readonly ILogger<DrawHostedService> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public DrawHostedService(
			IDrawService drawService,
			ILogger<DrawHostedService> logger,
			IHostApplicationLifetime appLifeTime)
		{
			_drawService = drawService;
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

			Task.Run(
				async () =>
				{
					while(true)
					{
						try
						{
							_logger.LogInformation("Started running draw swervice.");

							await _drawService.DrawAvailableSweepsAsync(_cancellationTokenSource.Token);

							_logger.LogInformation("Delaying for 600 seconds.");

							await Task.Delay(TimeSpan.FromSeconds(600));
						}
						catch(Exception e)
						{
							_logger.LogError("An error occurred... Exception: {message}: StackTrace: {stackTrace}", e.Message, e.StackTrace);
							_logger.LogInformation("An error occurred... delaying for 120 seconds.");
						}
					}
				}
			).Wait();

			_appLifeTime.StopApplication();
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}

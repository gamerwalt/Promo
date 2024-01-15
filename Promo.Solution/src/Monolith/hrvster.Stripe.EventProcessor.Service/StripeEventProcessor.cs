using hrvster.ApplicationServices.Stripe;
using System.Threading;

namespace hrvster.Stripe.EventProcessor.Service
{
	public class StripeEventProcessor : IHostedService
	{
		private readonly ILogger<StripeEventProcessor> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly IStripeEventService _service;

		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public StripeEventProcessor(
			ILoggerFactory loggerFactory, 
			IHostApplicationLifetime appLifeTime,
			IStripeEventService service)
		{
			_logger = loggerFactory.CreateLogger<StripeEventProcessor>();
			_appLifeTime = appLifeTime;
			_service = service;
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

			await _service.ProcessPendingEventsAsync(_cancellationTokenSource.Token);

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
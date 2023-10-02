using hrvster.Models.Options;

namespace hrvster.PaymentPendingChecker.Service
{
	public class PaymentPendingCheckerService : IHostedService
	{
		private readonly ILogger<PaymentPendingCheckerService> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly IAccountSubscriptionCheckerService _service;
		private readonly PaymentPendingOptions _paymentPendingSettings;

		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public PaymentPendingCheckerService(
			ILogger<PaymentPendingCheckerService> logger,
			IHostApplicationLifetime appLifeTime,
			IAccountSubscriptionCheckerService service,
			PaymentPendingOptions paymentPendingOptions)
		{
			_logger = logger;
			_appLifeTime = appLifeTime;
			_service = service;
			_paymentPendingSettings = paymentPendingOptions;
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

			await _service.UnsubscribePaymentPendingSubscriptionsAsync(
				_paymentPendingSettings.NumberOfHoursBeforeRollback,
				_cancellationTokenSource.Token);

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
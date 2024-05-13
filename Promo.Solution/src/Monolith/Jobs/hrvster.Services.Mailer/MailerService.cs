using hrvster.ApplicationServices.JobContracts;

namespace hrvster.Services.Mailer
{
	public class MailerService : IHostedService
	{
		private readonly ILogger<MailerService> _logger;
		private readonly IService _emailNotificationService;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public MailerService(ILoggerFactory loggerFactory, IHostApplicationLifetime appLifeTime, IService emailNotificationService)
		{
			_logger = loggerFactory.CreateLogger<MailerService>();
			_emailNotificationService = emailNotificationService;
			_appLifeTime = appLifeTime;
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Startasync started for hrvster.Services.Mailer ...");
			_appLifeTime.ApplicationStarted.Register(OnStarted);
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private async void OnStarted()
		{
			_logger.LogInformation("OnStarted for hrvster.Services.MailierService has been called");

			await _emailNotificationService.ProcessService(_cancellationTokenSource.Token);

			_appLifeTime.StopApplication();
		}

		private void OnStopped() => _logger.LogInformation("OnStopped has been called for hrvster.Services.Mailer");

		private void OnStopping()
		{
			_cancellationTokenSource.Cancel();
			_logger.LogInformation("OnStopping has been called for hrvster.Services.Mailer.");
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}

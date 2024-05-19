using hrvster.ApplicationServices.JobContracts;

namespace hrvster.Services.BoardAndPrizeSeeder
{
	public class BoardAndPrizeSeeder : IHostedService
	{
		private readonly ILogger<BoardAndPrizeSeeder> _logger;
		private readonly IService _boardAndPrizeGenerator;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public BoardAndPrizeSeeder(ILoggerFactory loggerFactory, IHostApplicationLifetime appLifeTime, IService boardAndPrizeGenerator)
		{
			_logger = loggerFactory.CreateLogger<BoardAndPrizeSeeder>();
			_boardAndPrizeGenerator = boardAndPrizeGenerator;
			_appLifeTime = appLifeTime;
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Startasync started for hrvster.Services.BoardAndPrizeSeeder ...");
			_appLifeTime.ApplicationStarted.Register(OnStarted);
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private async void OnStarted()
		{
			_logger.LogInformation("OnStarted for hrvster.Services.BoardAndPrizeSeeder has been called");

			await _boardAndPrizeGenerator.ProcessService(_cancellationTokenSource.Token);

			_appLifeTime.StopApplication();
		}

		private void OnStopped() => _logger.LogInformation("OnStopped has been called for hrvster.Services.BoardAndPrizeSeeder");

		private void OnStopping()
		{
			_cancellationTokenSource.Cancel();
			_logger.LogInformation("OnStopping has been called for hrvster.Services.BoardAndPrizeSeeder.");
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}

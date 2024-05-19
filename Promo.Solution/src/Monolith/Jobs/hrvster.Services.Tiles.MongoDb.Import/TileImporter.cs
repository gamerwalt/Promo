using hrvster.ApplicationServices.JobContracts;

namespace hrvster.Services.Tiles.MongoDb.Import
{
	public class TileImporter : IHostedService
	{
		private readonly ILogger<TileImporter> _logger;
		private readonly IService _mongoDbTileImporter;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public TileImporter(
			ILoggerFactory loggerFactory,
			IHostApplicationLifetime appLifeTime,
			IService mongoDbTileImporter)
		{
			_logger = loggerFactory.CreateLogger<TileImporter>();
			_mongoDbTileImporter = mongoDbTileImporter;
			_appLifeTime = appLifeTime;
			_cancellationTokenSource = new CancellationTokenSource();
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Startasync started for hrvster.Services.BoardTileSeeder ...");
			_appLifeTime.ApplicationStarted.Register(OnStarted);
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private async void OnStarted()
		{
			_logger.LogInformation("OnStarted for hrvster.Services.BoardTileSeeder has been called");

			await _mongoDbTileImporter.ProcessService(_cancellationTokenSource.Token);

			_appLifeTime.StopApplication();
		}

		private void OnStopped() => _logger.LogInformation("OnStopped has been called for hrvster.Services.BoardTileSeeder");

		private void OnStopping()
		{
			_cancellationTokenSource.Cancel();
			_logger.LogInformation("OnStopping has been called for hrvster.Services.BoardTileSeeder.");
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}

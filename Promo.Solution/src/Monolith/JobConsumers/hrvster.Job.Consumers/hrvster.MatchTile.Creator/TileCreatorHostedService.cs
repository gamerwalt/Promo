namespace hrvster.MatchTile.Creator
{
	public class TileCreatorHostedService : IHostedService
	{
		private readonly ILogger<TileCreatorHostedService> _logger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private readonly TileMongoDbConsumer _tileMongoDbConsumer;

		public TileCreatorHostedService(
			ILogger<TileCreatorHostedService> logger,
			IHostApplicationLifetime appLifeTime,
			TileMongoDbConsumer tileMongoDbConsumer)
		{
			_logger = logger;
			_appLifeTime = appLifeTime;
			_tileMongoDbConsumer = tileMongoDbConsumer;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("StartAsync started for hrvster.Match.Tile.Creator ...");

			// Link cancellation tokens
			CancellationToken linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationTokenSource.Token).Token;

			_appLifeTime.ApplicationStarted.Register(() => OnStarted(linkedToken));
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private async void OnStarted(CancellationToken cancellationToken)
		{
			_logger.LogInformation("OnStarted for hrvster.Match.Tile.Creator has been called.");

			try
			{
				await _tileMongoDbConsumer.RunAsync(cancellationToken);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while running the tile consumer.");
				_appLifeTime.StopApplication();
			}
		}

		private void OnStopped()
		{
			_logger.LogInformation("OnStopped has been called for hrvster.Match.Tile.Creator.");
		}

		private void OnStopping()
		{
			_cancellationTokenSource.Cancel();
			_logger.LogInformation("OnStopping has been called for hrvster.Match.Tile.Creator.");
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("StopAsync called for hrvster.Match.Tile.Creator.");
			await _tileMongoDbConsumer.StopAsync(cancellationToken);
			_cancellationTokenSource.Dispose();
		}
	}
}

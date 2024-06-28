
using hrvster.ApplicationServices.JobContracts;
using log4net;

namespace hrvster.Services.RedisGame.Purge
{
	public class PurgeRedisHostedService : IHostedService
	{
		private readonly ILog _logger;
		private readonly IService _purger;
		private readonly IHostApplicationLifetime _appLifeTime;
		private readonly CancellationTokenSource _cancellationTokenSource;

		public PurgeRedisHostedService(ILog logger, IHostApplicationLifetime appLifeTime, IService purger)
		{
			_logger = logger;
			_appLifeTime = appLifeTime;
			_purger = purger;
			_cancellationTokenSource = new CancellationTokenSource();

			LogicalThreadContext.Properties["CurrentClass"] = nameof(PurgeRedisHostedService);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.Info("Startasync started for hrvster.Services.RedisGame.Purge ...");
			_appLifeTime.ApplicationStarted.Register(OnStarted);
			_appLifeTime.ApplicationStopping.Register(OnStopping);
			_appLifeTime.ApplicationStopped.Register(OnStopped);

			return Task.CompletedTask;
		}

		private async void OnStarted()
		{
			_logger.Info("OnStarted for hrvster.Services.RedisGame.Purge.PurgeRedisHostedService has been called");

			await _purger.ProcessService(_cancellationTokenSource.Token);

			_appLifeTime.StopApplication();
		}

		private void OnStopped() => _logger.Info("OnStopped has been called for hrvster.Services.RedisGame.Purge.PurgeRedisHostedService");

		private void OnStopping()
		{
			_cancellationTokenSource.Cancel();
			_logger.Info("OnStopping has been called for hrvster.Services.RedisGame.Purge.PurgeRedisHostedService.");
			_logger.Logger.Repository.Shutdown();
		}

		public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
	}
}

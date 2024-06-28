using hrvster.ApplicationServices.JobContracts;
using log4net;

namespace hrvster.Services.RedisGame.Purge
{
	public class PurgeRedisService : IService
	{
		private readonly ILog _logger;

		public PurgeRedisService(ILog logger)
		{
			_logger = logger;
			LogicalThreadContext.Properties["CurrentClass"] = nameof(PurgeRedisService);
		}

		public Task ProcessService(CancellationToken token)
		{
			_logger.Info("Starting process service for PurgeRedisService");

			return Task.CompletedTask;
		}
	}
}

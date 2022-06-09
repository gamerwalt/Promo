using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class ScheduledTweetService : IService
	{
		private readonly ILogger<ScheduledTweetService> _logger;

		public ScheduledTweetService(
			ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetService>();
		}

		public Task ProcessService(System.Threading.CancellationToken token)
		{
			_logger.LogInformation("Processing service of {serviceName}", nameof(ScheduledTweetService));

			return Task.CompletedTask;
		}
	}
}

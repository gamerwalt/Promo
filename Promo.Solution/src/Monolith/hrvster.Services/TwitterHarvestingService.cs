using hrvster.ApplicationServices.Harvesters.Twitter;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class TwitterHarvestingService : IService
	{
		private readonly ILogger<ScheduledTweetService> _logger;
		private readonly ITwitterHarvesterService _twitterHrvsterService;

		public TwitterHarvestingService(
			ILoggerFactory loggerFactory,
			ITwitterHarvesterService twitterHrvsterService)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetService>();
			_twitterHrvsterService = twitterHrvsterService;
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(TwitterHarvestingService));

			await _twitterHrvsterService.StartStreamingAsync();
		}
	}
}

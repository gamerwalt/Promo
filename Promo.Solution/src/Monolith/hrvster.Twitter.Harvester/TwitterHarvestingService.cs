using hrvster.ApplicationServices.Harvesters.Twitter;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Twitter.Harvester
{
	public class TwitterHarvestingService : IService
	{
		private readonly ILogger<TwitterHarvestingService> _logger;
		private readonly ITwitterHarvesterService _twitterHrvsterService;

		public TwitterHarvestingService(
			ILoggerFactory loggerFactory,
			ITwitterHarvesterService twitterHrvsterService)
		{
			_logger = loggerFactory.CreateLogger<TwitterHarvestingService>();
			_twitterHrvsterService = twitterHrvsterService;
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(TwitterHarvestingService));

			await _twitterHrvsterService.StartStreamingAsync();
		}
	}
}

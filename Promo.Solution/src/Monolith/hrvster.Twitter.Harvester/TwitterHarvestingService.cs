using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Twitter.Harvester
{
	public class TwitterHarvestingService : IService
	{
		private readonly ILogger<TwitterHarvestingService> _logger;
		private readonly ITwitterHarvesterService _twitterHrvsterService;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public TwitterHarvestingService(
			ILoggerFactory loggerFactory,
			ITwitterHarvesterService twitterHrvsterService,
			HarvesterSettingsOptions harvesterSettings)
		{
			_logger = loggerFactory.CreateLogger<TwitterHarvestingService>();
			_twitterHrvsterService = twitterHrvsterService;
			_hrvsterSettings = harvesterSettings;
		}

		public async Task ProcessService(CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(TwitterHarvestingService));

			await _twitterHrvsterService.StartStreamingAsync();

			_logger.LogInformation($"Starting a delay to restart in {_hrvsterSettings.HrvsterRestartMinutes} minute(s) from now.");

			await Task.Delay(TimeSpan.FromMinutes(_hrvsterSettings.HrvsterRestartMinutes), token);

			_logger.LogInformation("Delay has elapsed. Will shut down now and it should restart soon.");
		}
	}
}

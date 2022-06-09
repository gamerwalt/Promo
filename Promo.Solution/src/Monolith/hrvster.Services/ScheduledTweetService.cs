using hrvster.Infrastructure.Scheduling;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Services
{
	public class ScheduledTweetService : IService
	{
		private readonly ILogger<ScheduledTweetService> _logger;
		private readonly IScheduledSweepRuleTweetsRepository _scheduledSweepRuleTweetsRepository;

		public ScheduledTweetService(
			ILoggerFactory loggerFactory,
			IScheduledSweepRuleTweetsRepository scheduledSweepRuleTweetsRepository)
		{
			_logger = loggerFactory.CreateLogger<ScheduledTweetService>();
			_scheduledSweepRuleTweetsRepository = scheduledSweepRuleTweetsRepository;
		}

		public async Task ProcessService(System.Threading.CancellationToken token)
		{
			_logger.LogInformation("Starting {serviceName}", nameof(ScheduledTweetService));

			var scheduledTweets = await _scheduledSweepRuleTweetsRepository.GetAllToSendAsync(DateTime.UtcNow);
			
			_logger.LogInformation("Pulled {count} scheduled sweep rule tweets to send.", scheduledTweets.Count);

			foreach (var scheduledTweet in scheduledTweets)
			{
				var images = await _scheduledSweepRuleTweetsRepository.GetAllImagesAsync(scheduledTweet.ScheduledId);

				_logger.LogInformation("Pulled {count} images for the scheduled id {scheduledUid} to send.", 
					images.Count, 
					scheduledTweet.ScheduledId);

				scheduledTweet.Images = images;
			}
		}
	}
}

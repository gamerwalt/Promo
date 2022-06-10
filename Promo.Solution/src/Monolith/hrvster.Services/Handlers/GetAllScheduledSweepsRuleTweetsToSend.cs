using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Scheduling;
using hrvster.Services.Requests;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Services.Handlers
{
	public class GetAllScheduledSweepsRuleTweetsToSend
		: IRequestHandler<GetAllScheduledSweepsRuleTweetsToSendRquest, IEnumerable<ScheduledSweepRuleTweet>>
	{
		private readonly ILogger<GetAllScheduledSweepsRuleTweetsToSend> _logger;
		private readonly IScheduledSweepRuleTweetsRepository _repository;

		public GetAllScheduledSweepsRuleTweetsToSend(
			ILoggerFactory loggerFactory,
			IScheduledSweepRuleTweetsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetAllScheduledSweepsRuleTweetsToSend>();
			_repository = repository;
		}

		public async Task<IEnumerable<ScheduledSweepRuleTweet>> Handle(
			GetAllScheduledSweepsRuleTweetsToSendRquest request,
			CancellationToken cancellationToken)
		{
			var scheduledTweets = await _repository.GetAllToSendAsync(request.CurrentDate);

			_logger.LogInformation("Pulled {count} scheduled sweep rule tweets to send.", scheduledTweets.Count);

			foreach (var scheduledTweet in scheduledTweets)
			{
				var images = await _repository.GetAllImagesAsync(scheduledTweet.ScheduledId);

				_logger.LogInformation("Pulled {count} images for the scheduled id {scheduledUid} to send.",
					images.Count,
					scheduledTweet.ScheduledId);

				scheduledTweet.Images = images;
			}

			return scheduledTweets;
		}
	}
}

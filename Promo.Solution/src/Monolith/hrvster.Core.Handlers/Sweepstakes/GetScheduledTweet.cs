using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Extensions;
using hrvster.Models.Scheduling;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetScheduledTweet : IRequestHandler<GetScheduledTweetRequest, ScheduledSweepRuleTweet>
	{
		private readonly ILogger<GetScheduledTweet> _logger;
		private readonly IScheduledSweepRuleTweetsRepository _repository;

		public GetScheduledTweet(ILoggerFactory loggerFactory, IScheduledSweepRuleTweetsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetScheduledTweet>();
			_repository = repository;
		}

		public async Task<ScheduledSweepRuleTweet> Handle(GetScheduledTweetRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting scheduled tweet for {rule}", request.Rule.ToJsonString());

			var scheduledTweet = await _repository.GetScheduledTweetAsync(request.Rule.RuleId);
			var images = await _repository.GetAllImagesAsync(scheduledTweet.ScheduledId);
			scheduledTweet.Images = images;

			return scheduledTweet;
		}
	}
}

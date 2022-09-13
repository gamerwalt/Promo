using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure.Harvester;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class UpdateIncomingTweetAsStopped : INotificationHandler<TweetProcessingStopped>
	{
		private readonly ILogger<UpdateIncomingTweetAsStopped> _logger;
		private readonly IIncomingTweetsRepository _repository;

		public UpdateIncomingTweetAsStopped(
			ILoggerFactory loggerFactory,
			IIncomingTweetsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateIncomingTweetAsStopped>();
			_repository = repository;
		}

		public async Task Handle(TweetProcessingStopped notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating incoming tweet {incomingTweetUid} as completed due to too many tweets by the user.",
				notification.IncomingTweet.IncomingTweetUid);

			await _repository.UpdateStatusAsync(notification.IncomingTweet.IncomingTweetUid, Models.Harvester.IncomingTweetStatus.CompletedTooManyTweets);
		}
	}
}

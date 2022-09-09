using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure.Harvester;
using hrvster.Models.Harvester;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class UpdateIncomingTweetAsProcessed : INotificationHandler<SweepEntryFromTweetRecorded>
	{
		private readonly ILogger<UpdateIncomingTweetAsProcessed> _logger;
		private readonly IIncomingTweetsRepository _repository;

		public UpdateIncomingTweetAsProcessed(ILoggerFactory loggerFactory, IIncomingTweetsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdateIncomingTweetAsProcessed>();
			_repository = repository;
		}

		public Task Handle(SweepEntryFromTweetRecorded notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Updating incoming tweet {incomingTweet} as processed.", notification.IncomingTweet);

			return _repository.UpdateAsProcessedAsync(
				notification.IncomingTweet.IncomingTweetUid, 
				IncomingTweetStatus.Completed);
		}
	}
}

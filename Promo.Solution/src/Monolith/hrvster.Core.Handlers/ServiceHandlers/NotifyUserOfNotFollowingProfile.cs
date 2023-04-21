using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class NotifyUserOfNotFollowingProfile : AsyncRequestHandler<NotifyUserOfNotFollowingProfileRequest>
	{
		private readonly ILogger<NotifyUserOfNotFollowingProfile> _logger;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;
		private readonly IIncomingTweetsRepository _repository;

		public NotifyUserOfNotFollowingProfile(
			ILoggerFactory loggerFactory,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			IIncomingTweetsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<NotifyUserOfNotFollowingProfile>();
			_publisherResolver = publisherResolver;
			_repository = repository;
		}

		protected override async Task Handle(NotifyUserOfNotFollowingProfileRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Notifying user {userName} of not following profile account.", request.Tweet.ScreenName);

			var message = new Message
			{
				Profile = request.Profile,
				Text = $"@{request.Tweet.ScreenName} You need to be following @{request.Profile.ScreenName} to be entered into the sweepstakes."
			};

			var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);

			await twitterPublisher.PublishResponse(message, request.Tweet.TweetId);

			await _repository.UpdateAsProcessedAsync(request.IncomingTweet.IncomingTweetUid, Models.Harvester.IncomingTweetStatus.CompletedUserNotFollowing);
		}
	}
}

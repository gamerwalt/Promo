using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.MatchGames.Harvester;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.MatchGame
{
	public class TwitterUserNotification : AsyncRequestHandler<TwitterUserNotificationRequest>
	{
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;

		private readonly IExternalClient _twitterPublisher;

		public TwitterUserNotification(Func<ExternalClientType, IExternalClient> publisherResolver)
		{
			_publisherResolver = publisherResolver;

			_twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
		}

		protected async override Task Handle(TwitterUserNotificationRequest request, CancellationToken cancellationToken)
		{
			var screenName = request.Tweet.ScreenName;
			var tweetId = request.Tweet.TweetId;

			var message = $"@{screenName} {request.Message} - {DateTime.UtcNow:MMM dd, yyyy hh:mm:fffff}";

			await _twitterPublisher.PublishResponse(new Message
			{
				Text = message,
				Profile = request.Profile
			}, tweetId);
		}
	}
}

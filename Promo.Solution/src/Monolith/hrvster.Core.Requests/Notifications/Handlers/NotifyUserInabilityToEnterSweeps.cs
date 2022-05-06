using hrvster.Core.Requests.Notifications.Harvester;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class NotifyUserInabilityToEnterSweeps : INotificationHandler<UnableToEnterSweeps>
	{
		private readonly ITwitterClient _twitterClient;

		public NotifyUserInabilityToEnterSweeps(ITwitterClient twitterClient)
		{
			_twitterClient = twitterClient;
		}

		public async Task Handle(UnableToEnterSweeps notification, CancellationToken cancellationToken)
		{
			var tweet = await _twitterClient.Tweets.GetTweetAsync(notification.Notification.Tweet.Id);

			var text = $"@{tweet.CreatedBy} Cannot enter sweeps at this time Campaign is either not live or you are past the draw date.";

			await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(text) { InReplyToTweet = tweet });
		}
	}
}

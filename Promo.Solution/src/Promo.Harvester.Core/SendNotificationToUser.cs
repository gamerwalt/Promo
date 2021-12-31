using MediatR;
using Promo.Harvester.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace Promo.Harvester.Core
{
	public class SendNotificationToUser : INotificationHandler<SweepEntryRecorded>
	{
		private readonly ITwitterClient _twitterClient;

		public SendNotificationToUser(ITwitterClient twitterClient)
		{
			_twitterClient = twitterClient;
		}

		public async Task Handle(SweepEntryRecorded notification, CancellationToken cancellationToken)
		{
			var tweet = await _twitterClient.Tweets.GetTweetAsync(long.Parse(notification.TweetId));

			var text = $"@{tweet.CreatedBy} You just got entered into a sweepstakes entry to win (Prize details will be placed here :)).";

			await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(text) { InReplyToTweet = tweet });
		}
	}
}

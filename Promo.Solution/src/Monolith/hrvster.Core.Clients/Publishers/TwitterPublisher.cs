using hrvster.Core.Clients.Common;
using hrvster.Extensions;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace hrvster.Core.Clients.Publishers
{
	public class TwitterPublisher : IPublisher
	{
		private readonly ILogger<TwitterPublisher> _logger;
		private readonly TwitterAccount _twitterAccount;

		public TwitterPublisher(ILoggerFactory loggerFactory, TwitterAccount twitterAccount)
		{
			_logger = loggerFactory.CreateLogger<TwitterPublisher>();
			_twitterAccount = twitterAccount;
		}

		public async Task<string> PublishAsync(Message message)
		{
			var twitterClient = new TwitterClient(_twitterAccount.ConsumerKey, _twitterAccount.ConsumerSecret, message.Profile.AccessToken, message.Profile.AccessTokenSecret);

			var tweet = await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
			{
				Text = message.Text
			});

			if(tweet == null)
			{
				_logger.LogError("Unable to publish the tweet {message} received.", message.AsJson());
			}

			return tweet.IdStr;
		}
	}
}

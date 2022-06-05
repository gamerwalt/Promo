using hrvster.Core.Clients.Common;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public async Task PublishAsync(Message message)
		{
			var twitterClient = new TwitterClient(_twitterAccount.ConsumerKey, _twitterAccount.ConsumerSecret, message.Profile.AccessToken, message.Profile.AccessTokenSecret);

			await twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
			{
				Text = message.Text
			});
		}
	}
}

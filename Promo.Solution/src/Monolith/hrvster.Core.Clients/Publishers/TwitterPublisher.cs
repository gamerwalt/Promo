using hrvster.Core.Clients.Common;
using hrvster.Extensions;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace hrvster.Core.Clients.Publishers
{
	public class TwitterPublisher : IPublisher
	{
		private readonly ILogger<TwitterPublisher> _logger;
		private readonly TwitterAccount _twitterAccount;

		private TwitterClient _twitterClient;

		public TwitterPublisher(ILoggerFactory loggerFactory, TwitterAccount twitterAccount)
		{
			_logger = loggerFactory.CreateLogger<TwitterPublisher>();
			_twitterAccount = twitterAccount;
		}

		public async Task<string> PublishAsync(Message message)
		{
			_twitterClient = new TwitterClient(_twitterAccount.ConsumerKey, _twitterAccount.ConsumerSecret, message.Profile.AccessToken, message.Profile.AccessTokenSecret);

			var tweet = default(ITweet);
			var mediaIds = default(List<long>);

			if (message.Media.Count() > 0)
			{
				mediaIds = new List<long>();
				foreach(var path in message.Media)
				{
					var bytes = File.ReadAllBytes(path);
					var uploadedImage = await _twitterClient.Upload.UploadTweetImageAsync(bytes);

					mediaIds.Add(uploadedImage.Id.GetValueOrDefault());
				}
			}

			if(message.IsScheduled)
			{
				tweet = await PublishScheduledAsync(message);
			}
			else
			{
				tweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
				{
					Text = message.Text,
					MediaIds = message.Media.Count() > 0 ? mediaIds : null,
				});
			}			

			if(tweet == null)
			{
				_logger.LogError("Unable to publish the tweet {message} received.", message.AsJson());
			}

			return tweet.IdStr;
		}

		private async Task<ITweet> PublishScheduledAsync(Message message)
		{
			var url = Constants.ScheduledTweetUrl
				.Replace("{accountId}", message.Profile.ExternalProfileId)
				.Replace("{scheduledAt}", message.ScheduledDate.ToString("O"));

			var response = await _twitterClient.Execute.RequestAsync(request =>
			{
				request.Url = url;
				request.HttpMethod = HttpMethod.POST;
			});

			var jsonResponse = response.Content;

			return default(ITweet);
		}
	}
}

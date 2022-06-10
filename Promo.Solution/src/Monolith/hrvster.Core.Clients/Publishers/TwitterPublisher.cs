using hrvster.Core.Clients.Common;
using hrvster.Extensions;
using hrvster.Infrastructure.Extensions;
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
			_logger.LogInformation("Publishing the tweet {tweet} for Profile with uid {profileUid}",
				message.Text,
				message.Profile.ProfileUid);

			_twitterClient = new TwitterClient(_twitterAccount.ConsumerKey, _twitterAccount.ConsumerSecret, message.Profile.AccessToken, message.Profile.AccessTokenSecret);

			var mediaIds = default(List<long>);

			if (message.Media?.Count() > 0)
			{
				_logger.LogInformation("Uploading the media for the tweet for Profile with uid {profileUid}", message.Profile.ProfileUid);
				
				mediaIds = await UploadMedia(message);
			}

			var tweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
			{
				Text = message.Text,
				MediaIds = message.Media.Count() > 0 ? mediaIds : null,
			});

			if(tweet == null)
			{
				_logger.LogError("Unable to publish the tweet {message} received.", message.AsJson());

				return string.Empty;
			}

			return tweet?.IdStr;
		}

		private async Task<List<long>> UploadMedia(Message message)
		{
			var mediaIds = new List<long>();
			foreach (var path in message.Media)
			{
				_logger.LogInformation("Uploading {path} for the profile with uid {profileUid}",
					path,
					message.Profile.ProfileUid);

				var bytes = File.ReadAllBytes(path);
				var uploadedImage = await _twitterClient.Upload.UploadTweetImageAsync(bytes);

				if (uploadedImage == null)
				{
					_logger.LogError("Was unable to upload the media {path} for the profile with uid {profileUid}",
						path,
						message.Profile.ProfileUid);
				}
				else
				{
					mediaIds.Add(uploadedImage.Id.GetValueOrDefault());
				}
			}

			return mediaIds;
		}
	}
}

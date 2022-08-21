using hrvster.Core.Clients.Common;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace hrvster.Core.Clients.Publishers
{
	public class TwitterClient : IExternalClient
	{
		private readonly ILogger<TwitterClient> _logger;
		private readonly TwitterAccount _twitterAccount;

		private Tweetinvi.TwitterClient _twitterClient;

		public TwitterClient(ILoggerFactory loggerFactory, TwitterAccount twitterAccount)
		{
			_logger = loggerFactory.CreateLogger<TwitterClient>();
			_twitterAccount = twitterAccount;
		}

		public async Task DeleteAsync(string externalId, string accessToken, string accessTokenSecret)
		{
			BuildTwitterClient(accessToken, accessTokenSecret);

			var tweet = await _twitterClient.Tweets.GetTweetAsync(Convert.ToInt64(externalId));

			if (tweet == null)
			{
				_logger.LogInformation("Unable to retrieve the tweet of the external id: {externalId}", externalId);

				return;
			}

			await _twitterClient.Tweets.DestroyTweetAsync(tweet);
		}

		public async Task<string> PublishAsync(Message message)
		{
			_logger.LogInformation("Publishing the tweet {tweet} for Profile with uid {profileUid}",
				message.Text,
				message.Profile.ProfileUid);

			BuildTwitterClient(message.Profile.AccessToken, message.Profile.AccessTokenSecret);

			var mediaIds = await UploadMedia(message);
			var tweet = default(ITweet);

			if(mediaIds.Count == 0)
			{
				tweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
				{
					Text = message.Text
				});
			}
			else
			{
				tweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
				{
					Text = message.Text,
					MediaIds = mediaIds
				});
			}
			
			if (tweet == null)
			{
				_logger.LogError("Unable to publish the tweet {message} received.", message.AsJson());

				return string.Empty;
			}

			return tweet?.IdStr;
		}

		public async Task<string> PublishResponse(Message message, string responseToId)
		{
			_logger.LogInformation("Publishing the tweet {tweet} in response to {tweetId}",
				message.Text,
				responseToId);

			BuildTwitterClient(message.Profile.AccessToken, message.Profile.AccessTokenSecret);

			var tweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters
			{
				Text = message.Text,
				InReplyToTweetId = long.Parse(responseToId)
			});

			if (tweet == null)
			{
				_logger.LogError("Unable to publish the tweet {message} received.", message.AsJson());

				return string.Empty;
			}

			return tweet.IdStr;
		}

		private void BuildTwitterClient(string accessToken, string accessTokenSecret)
		{
			_twitterClient = new Tweetinvi.TwitterClient(
				_twitterAccount.ConsumerKey,
				_twitterAccount.ConsumerSecret,
				accessToken,
				accessTokenSecret);
		}

		private async Task<List<long>> UploadMedia(Message message)
		{
			if (message.Media?.Count() > 0)
			{
				_logger.LogInformation("Uploading the media for the tweet for Profile with uid {profileUid}", message.Profile.ProfileUid);
			}

			var mediaIds = new List<long>();

			if (message.Media != null)
			{
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
			}

			return mediaIds;
		}
	}
}

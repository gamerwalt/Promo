using hrvster.Core.Clients.Twitter.Models;
using hrvster.Models;
using hrvster.Models.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using static System.Net.Mime.MediaTypeNames;

namespace hrvster.Core.Clients
{
	public interface ITwitterHttpService
	{
		Task DeleteTweetAsync(string externalId, string accessToken);
		Task GetAccessTokenAsync(StringValues oauthToken, StringValues oauthVerifier);
		Task<AccessTokenResponse> GetAccessTokenAsyncForOfflineAccess(string code, string scheme, string value, string redirectUrl);
		Task<AccessTokenResponse> GetAccessTokenFromRefreshTokenAsync(string ownerRefreshToken);
		Task<UserProviderDetails> GetAuthenticatedUserAsync(AccessTokenResponse accessTokenObject);
		string GetAuthorizationUrl(string scheme, string value, string redirectUrl);
		Task<IEnumerable<Follower>> GetFollowersAsync(string externalProfileId);
		Task<string> PublishTweetAsync(string text, string accessToken, string refreshToken);
		Task<string> PublishTweetAsync(string text, string accessToken, string refreshToken, List<long> mediaIds);
		Task<string> PublishTweetInResponseToAsync(string tweetText, string tweetId, string accessToken, string accessTokenSecret);
		Task<string> SearchRecentTweets(string hashTag, CancellationToken cancellationToken);
	}

	public class TwitterHttpService : ITwitterHttpService
	{
		private readonly HttpClient _client;
		private readonly TwitterAccount _twitterAccount;
		private readonly ILogger<TwitterHttpService> _logger;

		private const string GetUserFollowers = "users/[id]/followers";
		private const string GetAccessTokenEndpoint = "oauth2/token";
		private const string MeEndpoint = "users/me";
		private const string TweetsEndpoint = "tweets";
		private const string ImageUploadEndpoint = "https://upload.twitter.com/1.1/media/upload.json";

		private const string RandomizedKey = "ABr85mJTMQNZ6Z6x";

		private const string IDReplacer = "[id]";

		public TwitterHttpService(
			ILoggerFactory loggerFactory,
			HttpClient client, 
			TwitterAccount twitterAccount)
		{
			_logger = loggerFactory.CreateLogger<TwitterHttpService>();
			_client = client;
			_twitterAccount = twitterAccount;
		}

		public async Task DeleteTweetAsync(string externalId, string accessToken)
		{
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			await _client.DeleteAsync($"{TweetsEndpoint}/{externalId}");
		}

		public async Task<string> SearchRecentTweets(string hashTag, CancellationToken cancellationToken)
		{
			return string.Empty;
		}

		public async Task<string> PublishTweetAsync(string text, string accessToken, string refreshToken, List<long> mediaIds)
		{
			try
			{
				var tweet = new
				{
					text,
					media = new
					{
						media_ids = mediaIds.Select(l => l.ToString()).ToList()
					}
				};

				var json = JsonConvert.SerializeObject(tweet);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				_client.DefaultRequestHeaders.Clear();
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

				var response = await _client.PostAsync(TweetsEndpoint, content);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var tweetResponse = JsonConvert.DeserializeObject<TweetResponse>(result);

					return tweetResponse.Tweet.Id;
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning("An error occurred while trying to publish a tweet. Message: {message}", ex.Message);

				return string.Empty;
			}
			
			return string.Empty;
		}

		public async Task<string> PublishTweetAsync(string text, string accessToken, string refreshToken)
		{
			try
			{
				var tweet = new
				{
					text
				};

				var json = JsonConvert.SerializeObject(tweet);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				_client.DefaultRequestHeaders.Clear();
				_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

				var response = await _client.PostAsync(TweetsEndpoint, content);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var tweetResponse = JsonConvert.DeserializeObject<TweetResponse>(result);

					return tweetResponse.Tweet.Id;
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning("An error occurred while trying to publish a tweet. Message: {message}", ex.Message);

				return string.Empty;
			}
			
			return string.Empty;
		}

		public async Task<AccessTokenResponse> GetAccessTokenAsyncForOfflineAccess(string code, string scheme, string value, string redirectUrl)
		{
			var redirectPath = $"{scheme}://{value}{redirectUrl}";

			var content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("code", code),
				new KeyValuePair<string, string>("grant_type", "authorization_code"),
				new KeyValuePair<string, string>("client_id", _twitterAccount.ClientId),
				new KeyValuePair<string, string>("redirect_uri", redirectPath),
				new KeyValuePair<string, string>("code_verifier", RandomizedKey)
			});

			var credentials = $"{_twitterAccount.ClientId}:{_twitterAccount.ClientSecret}";
			var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);

			var response = await _client.PostAsync(GetAccessTokenEndpoint, content);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<AccessTokenResponse>(result);
			}

			return null;
		}

		public async Task<AccessTokenResponse> GetAccessTokenFromRefreshTokenAsync(string ownerRefreshToken)
		{
			var content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("refresh_token", ownerRefreshToken),
				new KeyValuePair<string, string>("grant_type", "refresh_token"),
				new KeyValuePair<string, string>("client_id", _twitterAccount.ClientId)
			});

			var credentials = $"{_twitterAccount.ClientId}:{_twitterAccount.ClientSecret}";
			var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));

			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);

			var response = await _client.PostAsync(GetAccessTokenEndpoint, content);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<AccessTokenResponse>(result);
			}

			return null;
		}

		public string GetAuthorizationUrl(string scheme, string value, string redirectUrl)
		{
			var redirectPath = $"{scheme}://{value}{redirectUrl}";

			return $"https://twitter.com/i/oauth2/authorize?response_type=code&client_id={_twitterAccount.ClientId}&redirect_uri={redirectPath}&scope=tweet.read%20tweet.write%20users.read%20follows.read%20offline.access&state=state&code_challenge={RandomizedKey}&code_challenge_method=plain";
		}

		public async Task<IEnumerable<Follower>> GetFollowersAsync(string externalProfileId)
		{
			var followers = new List<Follower>();

			var nextToken = string.Empty;

			do
			{
				var page = await GetFollowersPageAsync(externalProfileId, nextToken);
				followers.AddRange(page.Data);

				nextToken = page.Meta.NextToken;
			} while(!string.IsNullOrWhiteSpace(nextToken));

			return followers;
		}

		private async Task<FollowerReponseObject> GetFollowersPageAsync(string externalProfileId, string nextToken)
		{
			var uri = GetUserFollowers.Replace(IDReplacer, externalProfileId);
			HttpResponseMessage response;

			if (string.IsNullOrWhiteSpace(nextToken))
				response = await _client.GetAsync($"{uri}?max_results=100");
			else
				response = await _client.GetAsync($"{uri}?max_results=100&pagination_token={nextToken}");

			if (!response.IsSuccessStatusCode)
			{
				return null;
			}

			var followerResponseObjectString = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<FollowerReponseObject>(followerResponseObjectString);
		}

		public async Task<string> PublishTweetInResponseToAsync(
			string tweetText,
			string tweetId,
			string accessToken, 
			string accessTokenSecret)
		{
			var tweet = new
			{
				text = $"{tweetText} - {DateTime.Now.Ticks}",
				reply = new
				{
					in_reply_to_tweet_id = tweetId
				}
			};

			_logger.LogInformation("Publishing the tweet {tweet}.", tweet.ToJsonString());

			var json = JsonConvert.SerializeObject(tweet);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

			var response = await _client.PostAsync(TweetsEndpoint, content);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				var tweetResponse = JsonConvert.DeserializeObject<TweetResponse>(result);

				_logger.LogInformation("The tweet id for the published tweet is {tweetId}.", tweetResponse.Tweet.Id);

				return tweetResponse.Tweet.Id;
			}

			return string.Empty;
		}

		public async Task GetAccessTokenAsync(StringValues oauthToken, StringValues oauthVerifier)
		{
			var url = $"https://api.twitter.com/oauth/access_token?oauth_verifier={oauthVerifier}&oauth_token={oauthToken}&oauth_consumer_key={_twitterAccount.ConsumerSecret}";
			var client = new HttpClient();

			var response = await client.PostAsync(url, null);
		}

		public async Task<UserProviderDetails> GetAuthenticatedUserAsync(AccessTokenResponse accessTokenObject)
		{
			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenObject.AccessToken);

			var response = await _client.GetAsync($"{MeEndpoint}?user.fields=created_at,description,entities,id,location,name,pinned_tweet_id,profile_image_url,protected,public_metrics,url,username,verified,withheld");

			if(response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();

				var twitterUserResponse = JsonConvert.DeserializeObject<TwitterUserResponse>(result);

				return new UserProviderDetails
				{
					AccessToken = accessTokenObject.AccessToken,
					RefreshToken = accessTokenObject.RefreshToken,
					Description = twitterUserResponse.Profile.Description,
					ExternalProfileId = twitterUserResponse.Profile.Id,
					Name = twitterUserResponse.Profile.Username,
					ScreenName = twitterUserResponse.Profile.Name,
					ProfileImage = twitterUserResponse.Profile.ProfileImageUrl,
					Location = twitterUserResponse.Profile.Location
				};
			}

			return null;
		}
	}
}

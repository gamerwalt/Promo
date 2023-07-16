using hrvster.Core.Clients;
using hrvster.Core.Requests.ServiceRequests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Harvesters.Twitter
{
	public interface ITwitterHarvesterService
	{
		Task StartStreamingAsync(CancellationToken token);
	}

	public class TwitterHarvesterService : ITwitterHarvesterService
	{
		private readonly ILogger<TwitterHarvesterService> _logger;
		private readonly HttpClient _httpClient;
		private readonly ITwitterHttpService _twitterHttpService;
		private readonly IMediator _mediator;

		public TwitterHarvesterService(
			ILoggerFactory loggerFactory,
			HttpClient httpClient,
			ITwitterHttpService twitterHttpService,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<TwitterHarvesterService>();
			_httpClient = httpClient;
			_twitterHttpService = twitterHttpService;
			_mediator = mediator;
		}

		public async Task StartStreamingAsync(CancellationToken token)
		{
			#region NewCode

			_logger.LogInformation("Retrieving rules to search from database.");

			var hashtagRules = await _mediator.Send(new GetActiveHashtagRulesRequest());
			var ruleList = hashtagRules.ToList();

			_logger.LogInformation("Retrieved a total of {count} hashtag rules. {rules}",
				ruleList.Count,
				string.Join(", ", ruleList.Select(c => c.WatchValue)));

			var tasks = ruleList.Select(rule =>
				Task.Run(async () =>
				{
					var processUid = Guid.NewGuid();

					try
					{
						token.ThrowIfCancellationRequested();

						var tweets = await _twitterHttpService.SearchRecentTweets(rule.WatchValue, token);
						var profileBehindHashtag = "";
						var gameBehindHashtag = "";

						foreach(var tweet in tweets)
						{
							var tweetId = tweet.Id;
							var text = tweet.Text;

							//var actualTweet = _twitterHttpService.GetTweetFromIdAsync(tweetId);
							//1. Get more details about the tweet
						}
					}
					catch(OperationCanceledException ex)
					{
						_logger.LogError("This operation has been cancelled. Message {message},", ex.Message);

						//Update the rule processing table for this rule
					}
					catch (Exception ex)
					{
						_logger.LogError("An error occurred while processing the rule {rule}. Error Message: {message}.",
							rule.WatchValue,
							ex.Message);

						//Update the rule processing table for this rule
					}
				})
			);

			try
			{
				await Task.WhenAll(tasks);
			}
			catch(OperationCanceledException ex)
			{
				_logger.LogError("This operation has been cancelled. Message {message},", ex.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred in one of the tasks. {message}", ex.Message);
			}

			#endregion


			#region NotWorkingYet
			//await GetAndRemoveRulesAsync();

			//var hashtagRules = await _mediator.Send(new GetActiveHashtagRulesRequest());

			//var rules = new List<object>();

			//_logger.LogInformation("Retrieved {count} hash tag rules to add for tracking purposes.", hashtagRules.Count());

			//foreach (var rule in hashtagRules)
			//{
			//	if (rule.ActiveInd)
			//	{
			//		_logger.LogInformation("Adding the Hashtag {hashTag} belonging to Campaign Name {campaignName} and Game Name {gameName} to listen to...",
			//			rule.WatchValue,
			//			rule.Campaign.CampaignName,
			//			rule.Game?.Name ?? rule.Sweep.Name);

			//		rules.Add(new { value = rule.WatchValue, tag = rule.WatchValue });
			//	}
			//}

			//var jsonContent = JsonConvert.SerializeObject(new { add = rules });

			//var addRulesContent = 
			//	new StringContent(jsonContent, Encoding.UTF8, "application/json");
			//var addRulesResponse = await _httpClient.PostAsync(
			//	"https://api.twitter.com/2/tweets/search/stream/rules", addRulesContent);

			//if (!addRulesResponse.IsSuccessStatusCode)
			//{
			//	_logger.LogError("Failed to add rules: " + await addRulesResponse.Content.ReadAsStringAsync());

			//	return;
			//}

			//var streamResponse = await _httpClient.GetAsync(
			//	"https://api.twitter.com/2/tweets/search/stream", HttpCompletionOption.ResponseHeadersRead);

			//if (!streamResponse.IsSuccessStatusCode)
			//{
			//	_logger.LogError("Failed to start stream: " + await streamResponse.Content.ReadAsStringAsync());

			//	return;
			//}

			//_logger.LogInformation("Started streaming from twitter.");

			//using (var streamReader = new StreamReader(await streamResponse.Content.ReadAsStreamAsync()))
			//{
			//	while (!streamReader.EndOfStream)
			//	{
			//		var line = await streamReader.ReadLineAsync();

			//		if (line.StartsWith("{\"data\":"))
			//		{
			//			_logger.LogInformation("Tweet received.");

			//			var tweetId = GetTweetId(line);

			//			var tweet = await GetTweet(tweetId);

			//			if (tweet == null) continue;

			//			var authorId = tweet.Tweet.AuthorId;
			//			var user = tweet.Includes.Users.FirstOrDefault(c => c.Id == authorId);

			//			if(tweet.Tweet.Entities.Hashtags != null)
			//			{
			//				var tweetModel = new Tweet
			//				{
			//					TweetId = tweet.Tweet.Id,
			//					TweetText = tweet.Tweet.Text,
			//					CreatedDate = tweet.Tweet.CreatedAt.DateTime,
			//					Hashtags = tweet.Tweet.Entities.Hashtags.Select(c => c.Tag.Trim()).ToList(),
			//					ScreenName = user.Username,
			//					ExternalUserId = user.Id,
			//					Location = user.Location,
			//					Verified = user.Verified,
			//					UserDescription = user.Description,
			//					InReplyToTweetId = tweet.Tweet.ReferencedTweets.FirstOrDefault().Id,
			//					ProfileImageUrl = user.ProfileImageUrl
			//				};

			//				var incomingTweet = await _mediator.Send(new SaveNewTweetReceivedRequest(line));

			//				_logger.LogInformation("Sent tweet for processing.");

			//				await _mediator.Send(new ProcessNewTweetRequest(tweetModel, line, incomingTweet));
			//			}
			//		}
			//	}
			//}
			#endregion
		}

		//private async Task GetAndRemoveRulesAsync()
		//{
		//	var getRulesResponse = await _httpClient.GetAsync("https://api.twitter.com/2/tweets/search/stream/rules");
		//	var getRulesJson = JObject.Parse(await getRulesResponse.Content.ReadAsStringAsync());

		//	if (!getRulesResponse.IsSuccessStatusCode)
		//	{
		//		_logger.LogError("Failed to get rules: " + await getRulesResponse.Content.ReadAsStringAsync());

		//		return;
		//	}

		//	var ruleIds = new List<string>();

		//	var metaResult = getRulesJson["meta"].ToString();
		//	var meta = JsonConvert.DeserializeObject<Meta>(metaResult);
		//	if (meta != null && meta.ResultCount >= 1)
		//	{
		//		foreach (var rule in getRulesJson["data"])
		//		{
		//			ruleIds.Add(rule["id"].ToString());
		//		}

		//		if (ruleIds.Count > 0)
		//		{
		//			await DeleteRulesAsync(ruleIds);
		//		}
		//		else
		//		{
		//			_logger.LogInformation("No rules to delete.");
		//		}
		//	}


		//}

		//private async Task DeleteRulesAsync(List<string> ruleIds)
		//{
		//	var deleteRulesContent = new StringContent(JsonConvert.SerializeObject(new { delete = new { ids = ruleIds } }), Encoding.UTF8, "application/json");
		//	var deleteRulesResponse = await _httpClient.PostAsync("https://api.twitter.com/2/tweets/search/stream/rules", deleteRulesContent);

		//	if (!deleteRulesResponse.IsSuccessStatusCode)
		//	{
		//		_logger.LogError("Failed to delete rules: " + await deleteRulesResponse.Content.ReadAsStringAsync());
		//	}
		//	else
		//	{
		//		_logger.LogInformation("All rules deleted successfully.");
		//	}
		//}

		//private async Task<TweetV2Response> GetTweet(JToken tweetId)
		//	=> await _twitterClient.TweetsV2.GetTweetAsync(long.Parse(tweetId.ToString()));

		//private static JToken GetTweetId(string line)
		//{
		//	var tweetObject = JObject.Parse(line);
		//	var tweet = tweetObject["data"]["text"].ToString();
		//	var tweetId = tweetObject["data"]["id"];
		//	return tweetId;
		//}
	}

	//public class Meta
	//{
	//	[JsonProperty("sent")]
	//	public DateTimeOffset Sent { get; set; }

	//	[JsonProperty("result_count")]
	//	public long ResultCount { get; set; }
	//}
}

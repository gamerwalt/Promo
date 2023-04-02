using hrvster.Core.Requests.Harvester;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Client.V2;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Parameters.V2;

namespace hrvster.ApplicationServices.Harvesters.Twitter
{
	public interface ITwitterHarvesterService
	{
		Task StartStreamingAsync();
	}

	public class TwitterHarvesterService : ITwitterHarvesterService
	{
		private readonly ILogger<TwitterHarvesterService> _logger;
		private readonly IMediator _mediator;
		private readonly ITwitterClient _twitterClient;
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public TwitterHarvesterService(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			ITwitterClient twitterClient,
			HttpClient httpClient,
			IConfiguration configuration)
		{
			_logger = loggerFactory.CreateLogger<TwitterHarvesterService>();
			_mediator = mediator;
			_twitterClient = twitterClient;
			_httpClient = httpClient;
			_configuration = configuration;

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["TwitterAccount:AuthenticationToken"]);
		}

		public async Task StartStreamingAsync()
		{
			await GetAndRemoveRulesAsync();
			
			var hashtagRules = await _mediator.Send(new GetActiveHashtagRulesRequest());

			var rules = new List<object>();

			_logger.LogInformation("Retrieved {count} hash tag rules to add for tracking purposes.", hashtagRules.Count());

			foreach (var rule in hashtagRules)
			{
				if (rule.ActiveInd)
				{
					_logger.LogInformation("Adding the Hashtag {hashTag} belonging to Campaign Name {campaignName} and Game Name {gameName} to listen to...",
						rule.WatchValue,
						rule.Campaign.CampaignName,
						rule.Game?.Name ?? rule.Sweep.Name);

					rules.Add(new { value = rule.WatchValue, tag = rule.WatchValue });
				}
			}

			var jsonContent = JsonConvert.SerializeObject(new { add = rules });

			var addRulesContent = 
				new StringContent(jsonContent, Encoding.UTF8, "application/json");
			var addRulesResponse = await _httpClient.PostAsync(
				"https://api.twitter.com/2/tweets/search/stream/rules", addRulesContent);

			if (!addRulesResponse.IsSuccessStatusCode)
			{
				_logger.LogError("Failed to add rules: " + await addRulesResponse.Content.ReadAsStringAsync());

				return;
			}

			var streamResponse = await _httpClient.GetAsync(
				"https://api.twitter.com/2/tweets/search/stream", HttpCompletionOption.ResponseHeadersRead);

			if (!streamResponse.IsSuccessStatusCode)
			{
				_logger.LogError("Failed to start stream: " + await streamResponse.Content.ReadAsStringAsync());

				return;
			}

			_logger.LogInformation("Started streaming from twitter.");

			using (var streamReader = new StreamReader(await streamResponse.Content.ReadAsStreamAsync()))
			{
				while (!streamReader.EndOfStream)
				{
					var line = await streamReader.ReadLineAsync();

					if (line.StartsWith("{\"data\":"))
					{
						_logger.LogInformation("Tweet received.");

						var tweetId = GetTweetId(line);

						var tweet = await GetTweet(tweetId);
						var tweetJson = ConvertTweetToJson(tweet);

						var incomingTweet = await _mediator.Send(new SaveNewTweetReceivedRequest(line));

						_logger.LogInformation("Sent tweet for processing.");

						await _mediator.Send(new ProcessNewTweetRequest(tweet, line, incomingTweet));
					}
				}
			}
		}

		private static string ConvertTweetToJson(ITweet tweet)
		{
			var settings = new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
			};

			var tweetJson = JsonConvert.SerializeObject(tweet, settings);
			return tweetJson;
		}

		private async Task GetAndRemoveRulesAsync()
		{
			var getRulesResponse = await _httpClient.GetAsync("https://api.twitter.com/2/tweets/search/stream/rules");
			var getRulesJson = JObject.Parse(await getRulesResponse.Content.ReadAsStringAsync());

			if (!getRulesResponse.IsSuccessStatusCode)
			{
				_logger.LogError("Failed to get rules: " + await getRulesResponse.Content.ReadAsStringAsync());

				return;
			}

			var ruleIds = new List<string>();

			foreach (var rule in getRulesJson["data"])
			{
				ruleIds.Add(rule["id"].ToString());
			}

			if (ruleIds.Count > 0)
			{
				await DeleteRulesAsync(ruleIds);
			}
			else
			{
				_logger.LogInformation("No rules to delete.");
			}
		}

		private async Task DeleteRulesAsync(List<string> ruleIds)
		{
			var deleteRulesContent = new StringContent(JsonConvert.SerializeObject(new { delete = new { ids = ruleIds } }), Encoding.UTF8, "application/json");
			var deleteRulesResponse = await _httpClient.PostAsync("https://api.twitter.com/2/tweets/search/stream/rules", deleteRulesContent);

			if (!deleteRulesResponse.IsSuccessStatusCode)
			{
				_logger.LogError("Failed to delete rules: " + await deleteRulesResponse.Content.ReadAsStringAsync());
			}
			else
			{
				_logger.LogInformation("All rules deleted successfully.");
			}
		}

		private async Task<ITweet> GetTweet(JToken tweetId)
		{
			var legacyTweet = await _twitterClient.Tweets.GetTweetAsync(long.Parse(tweetId.ToString()));

			var hashTags = string.Join(",", legacyTweet.Hashtags.Select(h => h.Text));

			_logger.LogInformation("New Tweet Received with hashtags {hashtags}", hashTags);
			return legacyTweet;
		}

		private static JToken GetTweetId(string line)
		{
			var tweetObject = JObject.Parse(line);
			var tweet = tweetObject["data"]["text"].ToString();
			var tweetId = tweetObject["data"]["id"];
			return tweetId;
		}
	}
}

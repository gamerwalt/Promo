using hrvster.Core.Requests.Harvester;
using hrvster.Core.Requests.ServiceRequests;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Events;

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

		public TwitterHarvesterService(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			ITwitterClient twitterClient)
		{
			_logger = loggerFactory.CreateLogger<TwitterHarvesterService>();
			_mediator = mediator;
			_twitterClient = twitterClient;
		}

		public async Task StartStreamingAsync()
		{
			var stream = _twitterClient.Streams.CreateFilteredStream();
			//Get all the hashtags required to add. 
			//Make sure that the campaigns and the games are all active
			//start streaming
			var hashtagRules = await _mediator.Send(new GetActiveHashtagRulesRequest());

			_logger.LogInformation("Retrieved {count} hash tag rules to add for tracking purposes.", hashtagRules.Count());

			foreach (var rule in hashtagRules)
			{
				if (rule.ActiveInd)
				{
					_logger.LogInformation("Adding the Hashtag {hashTag} belonging to Campaign Name {campaignName} and Sweep Name {sweepName} to listen to...",
						rule.WatchValue,
						rule.Campaign.CampaignName,
						rule.Sweep.Name);

					stream.AddTrack(rule.WatchValue);
				}
			}

			if (stream.TracksCount > 0)
			{
				stream.MatchingTweetReceived += NewTweetReceived;

				_logger.LogInformation("Started streaming and listening from twitter.");

				await stream.StartMatchingAnyConditionAsync();
			}
		}

		private async void NewTweetReceived(object sender, MatchedTweetReceivedEventArgs @event)
		{
			var hashTags = string.Join(",", @event.Tweet.Hashtags.Select(h => h.Text));

			_logger.LogInformation("New Tweet Received with hashtags {hashtags}", hashTags);

			//Task.Run(async () => await _mediator.Send(new ProcessNewTweetRequest(@event.Tweet, @event.Json))).Wait();
			var incomingTweet = await _mediator.Send(new SaveNewTweetReceivedRequest(@event.Json));

			_logger.LogInformation("Tweet saved for further processing with Guid {tweetGuid}",
				incomingTweet.IncomingTweetUid);

			await _mediator.Send(new ProcessNewTweetRequest(@event.Tweet, @event.Json, incomingTweet));
		}
	}
}

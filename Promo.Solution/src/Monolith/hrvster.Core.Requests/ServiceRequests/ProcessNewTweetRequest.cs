using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class ProcessNewTweetRequest : IRequest
	{
		private ITweet _tweet;
		private string _json;

		public ProcessNewTweetRequest(ITweet tweet, string json)
		{
			_tweet = tweet;
			_json = json;
		}

		public ITweet Tweet
		{
			get { return _tweet; }
		}

		public string Json
		{
			get { return _json; }
		}
	}

	public class ProcessNewTweet : AsyncRequestHandler<ProcessNewTweetRequest>
	{
		private readonly ILogger<ProcessNewTweet> _logger;
		private readonly IMediator _mediator;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IHarvesterRepository _harvesterRepository;
		private readonly IReportingRepository _reportingRepository;

		public ProcessNewTweet(
			ILoggerFactory loggerFactory, 
			IMediator mediator, 
			ISweepRulesRepository sweepRulesRepository,
			IHarvesterRepository harvesterRepository,
			IReportingRepository reportingRepository)
		{
			_logger = loggerFactory.CreateLogger<ProcessNewTweet>();
			_mediator = mediator;
			_sweepRulesRepository = sweepRulesRepository;
			_harvesterRepository = harvesterRepository;
			_reportingRepository = reportingRepository;
		}

		protected async override Task Handle(ProcessNewTweetRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Received the request to process the tweet with Id {tweetId} and text {tweetText}. Json Received: {json}", 
				request.Tweet.Id, 
				request.Tweet.Text,
				request.Json);

			//For Optimized approach, we can throw in a queue or
			//Send to harvster table as not processed
			//we should have another cron job periodically check and process unprocessed tweets			
			var rules = new List<Rule>();

			foreach(var hashtag in request.Tweet.Hashtags)
			{
				_logger.LogInformation("Loading the rules for the hashtag #{hashTag}", hashtag.Text);

				var sweepRules = await _sweepRulesRepository.GetRuleByHashTagAsync($"#{hashtag.Text}");

				_logger.LogDebug("Retrieved the sweep rules {sweepRules} for the hashtag {hashTag}.", 
					sweepRules.ToJsonString(), 
					hashtag.Text);

				rules.AddRange(sweepRules);
			}

			var newUserUid = Guid.NewGuid();
			var tweetUid = Guid.NewGuid();

			try
			{
				_logger.LogInformation("Starting a new transaction");
				await _harvesterRepository.BeginTransaction();

				foreach (var rule in rules)
				{
					var harvestedUid = Guid.NewGuid();
					_logger.LogDebug("Creating harvested record for Uid {harvestedUid}, Rule uid {ruleUid}.",
						harvestedUid,
						rule.RuleUid);

					await _harvesterRepository.CreateHarvestedRecordAsync(harvestedUid, rule.RuleUid, request.Json);

					_logger.LogDebug("Creating the tweet for the user with uid {userUid}, actual tweet {tweet}",
						newUserUid,
						request.Tweet.Text);
					await _harvesterRepository.CreateTweetUserAsync(newUserUid, request.Tweet.CreatedBy);

					_logger.LogDebug("Creating the tweet record with tweetUid {tweetUid}, harvestedUid {harvestedUid}, rule Uid {ruleUid}",
						tweetUid,
						harvestedUid,
						rule.RuleUid);
					await _harvesterRepository.CreateTweetRecordAsync(tweetUid, harvestedUid, rule.RuleUid, newUserUid, request.Tweet);

					_logger.LogDebug("Publishing that we have received a tweet {tweet}", request.Tweet.Text);
					await _mediator.Publish(new TweetReceived(rule, harvestedUid, newUserUid, request.Tweet));
				}

				_logger.LogInformation("Committing transaction");
				await _harvesterRepository.CommitTransaction();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred trying to save the Harvested Tweet. Exception Message: {message}",
					ex.Message);

				_logger.LogInformation("Rolling back transaction");

				await _harvesterRepository.RollbackTransaction();
			}
		}
	}
}

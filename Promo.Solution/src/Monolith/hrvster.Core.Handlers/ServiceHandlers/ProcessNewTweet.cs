using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Extensions;
using System.Runtime.CompilerServices;
using hrvster.ApplicationServices.Common;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class ProcessNewTweet : AsyncRequestHandler<ProcessNewTweetRequest>
	{
		private readonly ILogger<ProcessNewTweet> _logger;
		private readonly IMediator _mediator;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly IHarvesterRepository _harvesterRepository;
		private readonly IIncomingTweetsRepository _incomingTweetsRepository;
		private readonly IUserRateLimiterService _userRateLimiterService;

		public ProcessNewTweet(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			ISweepRulesRepository sweepRulesRepository,
			IHarvesterRepository harvesterRepository,
			IIncomingTweetsRepository incomingTweetsRepository,
			IUserRateLimiterService userRateLimiterService)
		{
			_logger = loggerFactory.CreateLogger<ProcessNewTweet>();
			_mediator = mediator;
			_sweepRulesRepository = sweepRulesRepository;
			_harvesterRepository = harvesterRepository;
			_incomingTweetsRepository = incomingTweetsRepository;
			_userRateLimiterService = userRateLimiterService;
		}

		async protected override Task Handle(ProcessNewTweetRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Received the request to process the tweet with Id {tweetId} and text {tweetText}. Json Received: {json}",
				request.Tweet.Id,
				request.Tweet.Text,
				request.Json);

			if(!await _userRateLimiterService.IsAllowedAsync(
				request.Tweet.CreatedBy.IdStr, 
				request.Tweet.Hashtags, 
				UserIdentifierType.TwitterHandle))
			{
				_logger.LogWarning("Cannot continue processing this tweet. User has tweeted to many times.");

				await _mediator.Publish(new TweetProcessingStopped(RateLimiter.UserRateLimiter, request, request.IncomingTweet));

				return;
			}

			await _incomingTweetsRepository.UpdateStatusAsync(
				request.IncomingTweet.IncomingTweetUid, 
				Models.Harvester.IncomingTweetStatus.Processing);

			//For Optimized approach, we can throw in a queue or
			//Send to harvster table as not processed
			//we should have another cron job periodically check and process unprocessed tweets
			var rules = new List<Rule>();
			var tasks = new List<Task>();

			foreach (var hashtag in request.Tweet.Hashtags)
			{
				_logger.LogInformation("Loading the rules for the hashtag #{hashTag}", hashtag.Text);

				var sweepRules = await _sweepRulesRepository.GetRuleByHashTagAsync($"#{hashtag.Text}");

				_logger.LogDebug("Retrieved the sweep rules {sweepRules} for the hashtag {hashTag}.",
					sweepRules.ToJsonString(),
					hashtag.Text);

				rules.AddRange(sweepRules);

				await _userRateLimiterService.UpsertAsync(hashtag, request.Tweet.CreatedBy.IdStr, UserIdentifierType.TwitterHandle);
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
					await _mediator.Publish(new TweetReceived(
						request.IncomingTweet,
						rule,
						harvestedUid,
						newUserUid,
						request.Tweet));
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

				await _incomingTweetsRepository.UpdateStatusAsync(
					request.IncomingTweet.IncomingTweetUid, 
					Models.Harvester.IncomingTweetStatus.Failed);
			}
		}
	}
}

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
using Org.BouncyCastle.Asn1.X509.Qualified;

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
			_logger.LogInformation("Received the request to process the tweet with Id {tweetId} and text {tweetText}. Incoming tweet guid {tweetGuid}",
				request.Tweet.Id,
				request.Tweet.Text,
				request.IncomingTweet.IncomingTweetUid);

			if(!await _userRateLimiterService.IsAllowedAsync(
				request.Tweet.CreatedBy.IdStr, 
				request.Tweet.Hashtags, 
				UserIdentifierType.TwitterHandle))
			{
				_logger.LogWarning("Cannot continue processing this tweet {tweetGuid}. User has tweeted to many times.", request.IncomingTweet.IncomingTweetUid);

				await _mediator.Publish(new TweetProcessingStopped(RateLimiter.UserRateLimiter, request, request.IncomingTweet));

				return;
			}

			_logger.LogInformation("Updating incoming tweet {tweetGuid} status as Processing", request.IncomingTweet.IncomingTweetUid);

			await _incomingTweetsRepository.UpdateStatusAsync(
				request.IncomingTweet.IncomingTweetUid, 
				Models.Harvester.IncomingTweetStatus.Processing);

			//For Optimized approach, we can throw in a queue or
			//Send to harvster table as not processed
			//we should have another cron job periodically check and process unprocessed tweets
			var rules = new List<Rule>();
			var tasks = new List<Task>();

			_logger.LogInformation("Loading rules and other details for tweet {tweetGuid}", request.IncomingTweet.IncomingTweetUid);

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
				_logger.LogInformation("Starting a new transaction for tweet {tweetGuid}", request.IncomingTweet.IncomingTweetUid);

				await _harvesterRepository.BeginTransaction();

				foreach (var rule in rules)
				{
					var harvestedUid = Guid.NewGuid();
					_logger.LogDebug("Creating harvested record for Uid {harvestedUid}, Rule uid {ruleUid}. Tweet Uid {tweetGuid}",
						harvestedUid,
						rule.RuleUid,
						request.IncomingTweet.IncomingTweetUid);

					await _harvesterRepository.CreateHarvestedRecordAsync(harvestedUid, rule.RuleUid, request.Json);

					_logger.LogDebug("Creating the tweet for the user with uid {userUid}, actual tweet {tweet} for tweet uid {tweetGuid}",
						newUserUid,
						request.Tweet.Text,
						request.IncomingTweet.IncomingTweetUid);

					await _harvesterRepository.CreateTweetUserAsync(newUserUid, request.Tweet.CreatedBy);

					_logger.LogDebug("Creating the tweet record for incoming tweet uid {tweetGuid}",
						request.IncomingTweet.IncomingTweetUid);

					await _harvesterRepository.CreateTweetRecordAsync(
						tweetUid, harvestedUid, rule.RuleUid, newUserUid, request.Tweet);

					_logger.LogDebug("Publishing that we have received a tweet {tweet} for tweet uid {tweetGuid}",
						request.Tweet.Text,
						request.IncomingTweet.IncomingTweetUid);

					await _mediator.Publish(new TweetReceived(
						request.IncomingTweet, rule, harvestedUid, newUserUid, request.Tweet));
				}

				_logger.LogInformation("Committing transaction for tweet uid {tweetUid}", request.IncomingTweet.IncomingTweetUid);

				await _harvesterRepository.CommitTransaction();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred trying to save the Harvested Tweet for tweet guid {tweetGuid}. Exception Message: {message}",
					request.IncomingTweet.IncomingTweetUid,
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

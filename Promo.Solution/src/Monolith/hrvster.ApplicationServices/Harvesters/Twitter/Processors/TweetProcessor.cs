using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Harvester;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace hrvster.ApplicationServices.Processors
{
	public interface ITweetProcessor
	{
		Task ProcessTweetAsync(IncomingTweet incomingTweet, ITweet tweet, Rule rule, Guid userUid, Guid harvestedUid);
	}

	public class TweetProcessor : ITweetProcessor
	{
		private readonly ILogger<TweetProcessor> _Logger;
		private readonly ISweepEntryRulesRepository _sweepEntryRulesRepository;
		private readonly IReportingRepository _reportingRepository;
		private readonly IBlacklistedSweepsUsersRepository _blacklistedSweepsUserRepository;
		private readonly IMediator _mediator;

		public TweetProcessor(
			ILoggerFactory loggerFactory,
			ISweepEntryRulesRepository sweepEntryRulesRepository,
			IReportingRepository reportingRepository,
			IBlacklistedSweepsUsersRepository blacklistedSweepsUsersRepository,
			IMediator mediator)
		{
			_Logger = loggerFactory.CreateLogger<TweetProcessor>();
			_sweepEntryRulesRepository = sweepEntryRulesRepository;
			_reportingRepository = reportingRepository;
			_blacklistedSweepsUserRepository = blacklistedSweepsUsersRepository;
			_mediator = mediator;
		}

		public async Task ProcessTweetAsync(
			IncomingTweet incomingTweet, 
			ITweet tweet, 
			Rule rule, 
			Guid userUid, 
			Guid harvestedUid)
		{
			if (rule.Campaign == null)
			{
				_Logger.LogInformation("Unable to continue in the {action} processing the tweet with uid {tweetGuid}, UserId {userId} and HarvestedUid {harvestedUid} because the campaign is null.",
					nameof(TweetProcessor.ProcessTweetAsync),
					incomingTweet.IncomingTweetUid,
					userUid,
					harvestedUid);

				return;
			}

			if(rule.Sweep == null)
			{
				_Logger.LogInformation("Unable to continue in the {action} processing the tweet with uid {tweetGuid}, UserId {userId} and HarvestedUid {harvestedUid} because the sweep is null",
					nameof(TweetProcessor.ProcessTweetAsync),
					incomingTweet.IncomingTweetUid,
					userUid,
					harvestedUid);

				return;
			}

			var blacklisted = await _blacklistedSweepsUserRepository.GetBlacklistedSweepUsersAsync(rule.Sweep.SweepUid);
			var entryRules = await _sweepEntryRulesRepository.GetSweepEntryRuleAsync(rule.Sweep.SweepUid);

			if(entryRules == null)
			{
				_Logger.LogInformation("There are no entry rules for the sweep {sweepName} and will not be able to continue to process the tweet received for hashtag {hashTag}",
					rule.Sweep.Name,
					string.Join(",", tweet.Hashtags.Select(c => c.Text)));

				return;
			}

			var userReport = await _reportingRepository.GetCampaignSweepUserReportAsync(
				tweet.CreatedBy.ScreenName,
				UserIdentifierType.TwitterHandle.ToString(),
				tweet.CreatedBy.IdStr,
				rule.Campaign.CampaignUid,
				rule.Sweep.SweepUid);

			await ValidateCampaignTypeAsync(incomingTweet, blacklisted, entryRules, tweet, rule, userUid, harvestedUid, userReport);
		}

		private async Task ValidateCampaignTypeAsync(
			IncomingTweet incomingTweet,
			IEnumerable<BlacklistedSweepUser> blacklisted, 
			SweepEntryRule entryRules, 
			ITweet tweet, 
			Rule rule, 
			Guid userUid, 
			Guid harvestedUid,
			SweepEntryReport userReport)
		{
			switch(rule.Campaign.CampaignType.CampaignTypeCode)
			{
				case CampaignTypeCode.AutoAll:
					break;
				case CampaignTypeCode.AutoEnterSweepsFromTweetReplyHashtag:
					
					_Logger.LogInformation("Processing the campaign type for the tweet uid {tweetGuid} and campaign name {campaignName}",
						incomingTweet.IncomingTweetUid,
						rule.Campaign.CampaignName);

					await _mediator.Send( 
						new ProcessAutoEnterSweepsFromHashtagReplyRequest
						(
							incomingTweet, blacklisted, entryRules, tweet, rule, userUid, harvestedUid, userReport
						)
					);
					break;
				case CampaignTypeCode.AutoEnterSweepsFromHashtag:
					break;
				case CampaignTypeCode.AutoEnterSweepsFromTweet:
					break;
				case CampaignTypeCode.AutoReplyDM:
					break;
				case CampaignTypeCode.AutoReplyHashtag:
					break;
				case CampaignTypeCode.AutoReplyLikedTweet:
					break;
				case CampaignTypeCode.AutoReplyTweet:
					break;
			}
		}
	}
}

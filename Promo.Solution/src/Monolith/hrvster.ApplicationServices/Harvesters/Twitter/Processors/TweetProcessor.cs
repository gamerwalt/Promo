using hrvster.Core.Requests.MatchGames.Harvester;
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
		Task ProcessTweetAsync(IncomingTweet incomingTweet, ITweet tweet, Models.Rule rule, Guid userUid, Guid harvestedUid);
	}

	public class TweetProcessor : ITweetProcessor
	{
		private readonly ILogger<TweetProcessor> _Logger;
		private readonly ISweepEntryRulesRepository _sweepEntryRulesRepository;
		private readonly IReportingRepository _reportingRepository;
		private readonly IBlacklistedSweepsUsersRepository _blacklistedSweepsUserRepository;
		private readonly IBlacklistedUserRepository _blacklistedUserRepository;
		private readonly IMediator _mediator;

		public TweetProcessor(
			ILoggerFactory loggerFactory,
			ISweepEntryRulesRepository sweepEntryRulesRepository,
			IReportingRepository reportingRepository,
			IBlacklistedSweepsUsersRepository blacklistedSweepsUsersRepository,
			IBlacklistedUserRepository blacklistedUserRepository,
			IMediator mediator)
		{
			_Logger = loggerFactory.CreateLogger<TweetProcessor>();
			_sweepEntryRulesRepository = sweepEntryRulesRepository;
			_reportingRepository = reportingRepository;
			_blacklistedSweepsUserRepository = blacklistedSweepsUsersRepository;
			_blacklistedUserRepository = blacklistedUserRepository;
			_mediator = mediator;
		}

		public async Task ProcessTweetAsync(
			IncomingTweet incomingTweet,
			ITweet tweet,
			Models.Rule rule,
			Guid userUid,
			Guid harvestedUid)
		{
			using 
				(
					_Logger.BeginScope(
					new
					{
						IncomingTweetUid = incomingTweet.IncomingTweetUid,
						GameUid = rule.Game == null ? rule.Sweep.Uid : rule.Game.Uid
					})
				)

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

				if(rule.Game == null)
				{
					_Logger.LogWarning("No game was added to the rule.");

					return;
				}
				
				switch (rule.Game.GameType)
				{
					case GameTypesEnum.Sweeps:
						await ProcessTweetForSweepstakesAsync(incomingTweet, tweet, rule, userUid, harvestedUid);
						break;

					case GameTypesEnum.MatchGames:
						await ProcessTweetForMatchGameAsync(incomingTweet, tweet, rule, userUid, harvestedUid);
						break;

					default:
						await ProcessTweetForSweepstakesAsync(incomingTweet, tweet, rule, userUid, harvestedUid);
						break;
				}
			}
		}

		#region MatchGames
		private async Task ProcessTweetForMatchGameAsync(
			IncomingTweet incomingTweet, ITweet tweet, Models.Rule rule, Guid userUid, Guid harvestedUid)
		{
			if (rule.Game == null)
			{
				_Logger.LogInformation("Unable to continue in the {action} processing the tweet with uid {tweetGuid}, UserId {userId} and HarvestedUid {harvestedUid} because the game is null",
					nameof(TweetProcessor.ProcessTweetAsync),
					incomingTweet.IncomingTweetUid,
					userUid,
					harvestedUid);
			}

			var blacklisted = await _blacklistedUserRepository.GetBlacklistedUsersAsync(rule.Game.Uid);

			//TODO: WE NEED HOW MANY TIMES A USER CAN PLAY A MATCH GAME TO HELP WITH SPAMMING
			await ValidateCampaignTypeForMatchGameAsync(incomingTweet, blacklisted, tweet, rule, userUid, harvestedUid);
		}

		private async Task ValidateCampaignTypeForMatchGameAsync(
			IncomingTweet incomingTweet,
			IEnumerable<BlacklistedUser> blacklisted,
			ITweet tweet,
			Rule rule,
			Guid userUid,
			Guid harvestedUid)
		{
			switch (rule.Campaign.CampaignType.CampaignTypeCode)
			{
				case CampaignTypeCode.AutoAll:
					break;
				case CampaignTypeCode.AutoPlayFromTweetReplyHashtag:

					_Logger.LogInformation("Processing the campaign type for the tweet uid {tweetGuid} and campaign name {campaignName}",
						incomingTweet.IncomingTweetUid,
						rule.Campaign.CampaignName);

					await _mediator.Send(
						new ProcessPlayMatchGameHashtagReplyRequest(
							incomingTweet, blacklisted, tweet, rule, userUid, harvestedUid));
					break;
				case CampaignTypeCode.AutoPlayFromHashtag:
					break;
				case CampaignTypeCode.AutoPlayFromTweet:
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

		#endregion

		#region Sweepstakes
		private async Task ProcessTweetForSweepstakesAsync(
			IncomingTweet incomingTweet,
			ITweet tweet,
			Rule rule,
			Guid userUid,
			Guid harvestedUid)
		{
			if (rule.Sweep == null)
			{
				_Logger.LogInformation("Unable to continue in the {action} processing the tweet with uid {tweetGuid}, UserId {userId} and HarvestedUid {harvestedUid} because the sweep is null",
					nameof(TweetProcessor.ProcessTweetAsync),
					incomingTweet.IncomingTweetUid,
					userUid,
					harvestedUid);

				return;
			}

			var entryRules = await _sweepEntryRulesRepository.GetSweepEntryRuleAsync(rule.Sweep.SweepUid);

			if (entryRules == null)
			{
				_Logger.LogInformation("There are no entry rules for the sweep {sweepName} and will not be able to continue to process the tweet received for hashtag {hashTag}",
					rule.Sweep.Name,
					string.Join(",", tweet.Hashtags.Select(c => c.Text)));

				return;
			}

			var blacklisted = await _blacklistedSweepsUserRepository.GetBlacklistedSweepUsersAsync(rule.Sweep.SweepUid);

			var userReport = await _reportingRepository.GetCampaignSweepUserReportAsync(
									tweet.CreatedBy.ScreenName,
									UserIdentifierType.TwitterHandle.ToString(),
									tweet.CreatedBy.IdStr,
									rule.Campaign.CampaignUid,
									rule.Sweep.SweepUid);

			await ValidateCampaignTypeForSweepsAsync(
				incomingTweet, blacklisted, entryRules, tweet, rule, userUid, harvestedUid, userReport);
		}

		private async Task ValidateCampaignTypeForSweepsAsync(
			IncomingTweet incomingTweet,
			IEnumerable<BlacklistedSweepUser> blacklisted,
			SweepEntryRule entryRules,
			ITweet tweet,
			Rule rule,
			Guid userUid,
			Guid harvestedUid,
			SweepEntryReport userReport)
		{
			switch (rule.Campaign.CampaignType.CampaignTypeCode)
			{
				case CampaignTypeCode.AutoAll:
					break;
				case CampaignTypeCode.AutoPlayFromTweetReplyHashtag:

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
				case CampaignTypeCode.AutoPlayFromHashtag:
					break;
				case CampaignTypeCode.AutoPlayFromTweet:
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

		#endregion
	}
}

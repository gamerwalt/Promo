using hrvster.Core.Requests.Harvester;
using hrvster.Core.Requests.Notifications.Harvester;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	/// <summary>
	/// This will process all hashtags tweets and retweets
	/// </summary>
	public class ProcessReceivedTweetFromHashtagStream : INotificationHandler<NewHashTagTweetReceived>
	{
		private readonly ICampaignRepository _campaignRepository;
		private readonly IEntryRulesRepository _entryRulesRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepRulesRepository _rulesRepository;
		private readonly IReportingRepository _reportingRepository;
		private readonly IMediator _mediator;
		private ILogger<ProcessReceivedTweetFromHashtagStream> _logger;

		public ProcessReceivedTweetFromHashtagStream(
			ICampaignRepository campaignRepository,
			IEntryRulesRepository entryRulesRepository,
			ISweepsRepository sweepsRepository,
			ISweepRulesRepository rulesRepository,
			IReportingRepository reportingRepository,
			IMediator mediator,
			ILoggerFactory loggerFactory)
		{
			_campaignRepository = campaignRepository;
			_entryRulesRepository = entryRulesRepository;
			_sweepsRepository = sweepsRepository;
			_rulesRepository = rulesRepository;
			_reportingRepository = reportingRepository;
			_mediator = mediator;
			_logger = loggerFactory.CreateLogger<ProcessReceivedTweetFromHashtagStream>();
		}

		//Ideally, we should just send this to a queue to handle this process
		public async Task Handle(NewHashTagTweetReceived notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Started handling {action}", nameof(ProcessReceivedTweetFromHashtagStream));

			var campaign = await GetCampaignAsync(notification.Rule.Campaign.CampaignUid);
			var sweeps = await GetCampaignSweepsAsync(notification.Rule.Campaign.CampaignUid);
			var blacklistedUsers = await GetBlacklistedUsersForSweepstakesAsync(notification.Rule.Sweep.SweepUid);

			var sweepStakesToEnter = sweeps.FirstOrDefault(c => notification.Rule.Sweep.SweepUid.Equals(c.SweepUid));

			if (campaign == null || sweepStakesToEnter == null)
				throw new Exception("Campaign or Sweeps is null");

			switch (campaign.CampaignType.CampaignTypeCode)
			{
				case CampaignTypeCode.AutoAll:
					break;
				case CampaignTypeCode.AutoEnterSweepsFromHashtag:
					if (await CanEnterSweepstakes(notification, campaign, sweepStakesToEnter, blacklistedUsers))
					{
						await EnterSweepsAsync(notification, campaign, sweepStakesToEnter);
					}
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

			_logger.LogInformation("Finished handling {action}", nameof(ProcessReceivedTweetFromHashtagStream));
		}

		private async Task<IEnumerable<long>> GetBlacklistedUsersForSweepstakesAsync(Guid sweepUid)
		{
			return await _mediator.Send(new GetSweepsBlacklistedUsers(sweepUid));
		}

		private async Task<bool> CanEnterSweepstakes(NewHashTagTweetReceived notification, Campaign campaign, Sweep sweepStakesToEnter, IEnumerable<long> blacklistedUsers)
		{
			return await UserCanStillEnterSweepsAsync(sweepStakesToEnter, campaign, notification.Tweet.CreatedBy) &&
				TweetIsInReplyToRuleTweetId(notification) &&
				UserIsNotInBlacklistedUsers(notification, blacklistedUsers);
		}

		private bool UserIsNotInBlacklistedUsers(NewHashTagTweetReceived notification, IEnumerable<long> blacklistedUsers)
		{
			//using var isInBlacklistedScope = _logger.IsInBlackLis

			var isInBlacklistedUsers = blacklistedUsers.Any(c => c.Equals(notification.Tweet.CreatedBy.Id));
			return !isInBlacklistedUsers;
		}

		private static bool TweetIsInReplyToRuleTweetId(NewHashTagTweetReceived notification)
			=> notification.Tweet.InReplyToStatusId.Equals(notification.Rule.ExternalId);

		private async Task<IEnumerable<Sweep>> GetCampaignSweepsAsync(Guid campaignUid)
			=> await _sweepsRepository.GetAllForCampaignAsync(campaignUid);

		private async Task EnterSweepsAsync(NewHashTagTweetReceived notification, Campaign campaign, Sweep sweep)
		{
			if (campaign.ActiveInd && CampaignIsRunning(campaign) && SweepsIsRunningAndNotYetDrawn(sweep))
			{
				_logger.LogInformation("{action} called for CampaignName: {campaignName}", nameof(EnterSweepsAsync), campaign.CampaignName);
				_logger.LogInformation("Entering {screenName} into the sweepstakes {sweepName}", notification.Tweet.CreatedBy.ScreenName, sweep.Name);
				await _mediator.Send(new EnterSweeps(notification, campaign, sweep));

				return;
			}

			_logger.LogInformation("Could not enter sweep for Campaign : {campaignName}",
				campaign.CampaignName);

			_logger.LogInformation("Campaign Details - Start Date: {startDate}, End Date: {endDate}",
				campaign.StartDate.ToShortDateString(),
				campaign.EndDate.ToShortDateString());

			_logger.LogInformation("Sweepstakes Details - Start Date: {startDate}, End Date: {endDate}, Draw Date: {drawDate}",
				sweep.StartDate.ToShortDateString(),
				sweep.EndDate.ToShortDateString(),
				sweep.DrawDate.GetValueOrDefault().ToShortDateString());

			//Todo: DO we want to inform the user that the sweeps is not active yet?
			await _mediator.Publish(new UnableToEnterSweeps(notification, campaign, sweep));
		}

		private async Task<bool> UserCanStillEnterSweepsAsync(Sweep sweep, Campaign campaign, IUser createdBy)
		{
			var entryRule = await _entryRulesRepository.GetBySweepAndCampaignUidAsync(campaign.CampaignUid, sweep.SweepUid);
			//check reporting for screenname and user identifier type
			var userReport = await _reportingRepository.GetCampaignSweepUserReportAsync(
				campaignUid: campaign.CampaignUid,
				sweepUid: sweep.SweepUid,
				userIdentifierType: UserIdentifierType.TwitterHandle.ToString(),
				externalUserId: createdBy.IdStr,
				screenName: createdBy.ScreenName);

			if (userReport == null) return true;

			if (userReport.Entries >= entryRule.EntryPerUser) return false;

			return true;
		}

		private bool SweepsIsRunningAndNotYetDrawn(Sweep sweep)
			=> DateTime.Now < sweep.EndDate && DateTime.Now > sweep.StartDate && DateTime.Now < sweep.DrawDate;

		private bool CampaignIsRunning(Campaign campaign)
			=> DateTime.Now < campaign.EndDate && DateTime.Now > campaign.StartDate;

		private async Task<Campaign> GetCampaignAsync(Guid campaignUid)
			=> await _campaignRepository.GetCampaignByUidAsync(campaignUid);
	}
}

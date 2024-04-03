using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Common;
using hrvster.Core.Clients;
using hrvster.Core.Clients.Publishers;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Harvester;
using hrvster.Models.Profiles;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace hrvster.ApplicationServices.Sweepstakes
{
	public interface IPlaySweepstakeService
	{
		Task<Guid> EnterSweepstakeAsync(Sweep sweepGame, Guid ruleUid, string externalProfileId, string displayName, UserIdentifierType userIdentifierType, string tweetMessage, string externalClient, string accessToken, string accessTokenSecret);
	}

	public class PlaySweepstakeService : IPlaySweepstakeService
	{
		private readonly ILogger<PlaySweepstakeService> _logger;
		private readonly ISweepstakesService _sweepstakeService;
		private readonly IAccountsService _accountsService;
		private readonly ISweepNotificationRepository _sweepNotificationRepository;
		private readonly ISweepsEntriesRepository _sweepEntriesRepository;
		private readonly IMailRepository _mailRepository;
		private readonly TwitterAccount _twitterAccount;
		private readonly ICampaignService _campaignService;
		private readonly IReportingRepository _reportingRepository;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;
		private readonly ITwitterHttpService _twitterHttpService;

		private ITwitterClient _twitterClient;
		private IExternalClient _publisher;

		public PlaySweepstakeService(
			ILoggerFactory loggerFactory,
			ISweepstakesService sweepstakesService,
			IAccountsService accountsService,
			ISweepNotificationRepository sweepNotificationRepository,
			IMailRepository mailRepository,
			TwitterAccount twitterAccount,
			ICampaignService campaignService,
			ISweepsEntriesRepository sweepEntriesRepository,
			IReportingRepository reportingRepository,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			ITwitterHttpService twitterHttpService)
		{
			_logger = loggerFactory.CreateLogger<PlaySweepstakeService>();
			_sweepstakeService = sweepstakesService;
			_accountsService = accountsService;
			_sweepNotificationRepository = sweepNotificationRepository;
			_mailRepository = mailRepository;
			_twitterAccount = twitterAccount;
			_campaignService = campaignService;
			_sweepEntriesRepository = sweepEntriesRepository;
			_reportingRepository = reportingRepository;
			_publisherResolver = publisherResolver;
			_twitterHttpService = twitterHttpService;
		}

		public async Task<Guid> EnterSweepstakeAsync(
			Sweep sweepGame,
			Guid ruleUid,
			string externalProfileId,
			string displayName,
			UserIdentifierType userIdentifierType,
			string tweetMessage,
			string externalClient,
			string accessToken, 
			string accessTokenSecret)
		{
			_logger.LogInformation("Getting sweepstake {sweepName} entry rules.", sweepGame.Name);

			var tasks = new List<Task>();

			var rulesTask = _sweepstakeService.GetSweepRulesAsync(sweepGame.SweepUid);
			var entryRulesTask = _sweepstakeService.GetEntryRulesAsync(sweepGame.SweepUid);
			var blackListedUsersTask = _sweepstakeService.GetBlacklistedUsersOfSweepstakes(sweepGame.SweepUid);
			var userReportTask = _sweepstakeService.GetSweepstakeUserReport(displayName, UserIdentifierType.TwitterHandle, externalProfileId, sweepGame.Uid, sweepGame.Campaign.CampaignUid);
			var profileTask = _accountsService.GetProfileBySweepUidAsync(sweepGame.Campaign.CampaignUid, ProfileType.Twitter);
			var campaignTask = _campaignService.GetCampaignAsync(sweepGame.Campaign.CampaignUid, default);

			tasks.Add(rulesTask);
			tasks.Add(entryRulesTask);
			tasks.Add(blackListedUsersTask);
			tasks.Add(userReportTask);
			tasks.Add(profileTask);
			tasks.Add(campaignTask);

			await Task.WhenAll(tasks);

			var rules = rulesTask.Result;
			var entryRules = entryRulesTask.Result;
			var blackListedUsers = blackListedUsersTask.Result.ToList();
			var userReports = userReportTask.Result;
			var profile = profileTask.Result;
			var campaign = campaignTask.Result;

			var userCanEnterSweepstake = UserCanEnterSweepstake(userReports, entryRules);
			var userIsNotInBlacklist = UserIsNotInBlacklist(blackListedUsers, externalProfileId);

			if (!userCanEnterSweepstake || !userIsNotInBlacklist)
			{
				_logger.LogWarning("User cannot enter this sweepstake.");
				return Guid.Empty;
			}

			_logger.LogInformation("Checking if the sweepstake {sweepName} if maxxed out.", sweepGame.Name);

			var isMaxed = await _sweepstakeService.IsMaxedOut(sweepGame, default);
			if (isMaxed)
			{
				_logger.LogWarning("Sweepstake {sweepName} with uid {sweepUid} is already maxed out.",
					sweepGame.Name,
				sweepGame.Uid);

				var sweepNotification = await _sweepNotificationRepository.GetSweepNotificationAsync(sweepGame.Id, default);

				var account = await _accountsService.GetAccountByUidAsync(sweepGame.Account.AccountUid);

				if (sweepNotification == null || !sweepNotification.MaxEntries)
				{
					await _mailRepository.CreateAsync(CreateMaxEntryEmail(sweepGame, account));

					await _sweepNotificationRepository.ToggleMaxEntriesAsync(sweepGame.Id, default);
				}

				return Guid.Empty;
			}

			_logger.LogInformation("User can now enter sweepstake {name}", sweepGame.Name);

			//Todo: Do following here. We need $100.00 per month

			if (!await IsFollowing(rules.ToList(), ruleUid, profile, sweepGame.Campaign, externalProfileId))
			{
				_logger.LogInformation("User {displayName} is not following main account {mainProfile}", 
					displayName, 
					profile.DisplayName);

				return Guid.Empty;
			}

			var rule = rules.ToList().FirstOrDefault(r => r.RuleUid == ruleUid);

			if(externalClient != ExternalClientType.Twitter.ToString() ||
				externalClient != ExternalClientType.Facebook.ToString() ||
				externalClient != ExternalClientType.Instagram.ToString())
			{
				externalClient = ExternalClientType.Client.ToString();
			}

			_publisher = _publisherResolver(Enum.Parse<ExternalClientType>(externalClient));

			return await EnterSweepstakeAsync(_publisher, campaign, sweepGame, rule, profile, displayName, externalProfileId, userIdentifierType, tweetMessage, accessToken, accessTokenSecret, externalClient);
		}

		private async Task<Guid> EnterSweepstakeAsync(
			IExternalClient publisher,
			Campaign campaign,
			Sweep sweepGame,
			Rule rule,
			Profile profile,
			string displayName,
			string externalProfileId,
			UserIdentifierType userIdentifierType,
			string tweetMessage,
			string accessToken, 
			string refreshToken,
			string externalClient)
		{
			if (!campaign.ActiveInd ||
				!CampaignIsRunning(campaign) ||
				SweepIsRunningAndNotYetDrawn(sweepGame))
			{
				_logger.LogInformation("Campaign {campaignName} and Sweepstakes {sweepName} is running and User {screenName} can now be entered into the sweepstakes",
					campaign.CampaignName,
					sweepGame.Name,
					displayName);

				var sweepEntryUid = Guid.NewGuid();

				await _sweepEntriesRepository.CreateAsync(new SweepEntry
				{
					AcceptedTerms = true,
					Sweep = sweepGame,
					Rule = rule,
					UserIdentifier = $"@{displayName}",
					UserIdentifierType = userIdentifierType,
					SweepEntryUid = sweepEntryUid
				});

				var tweetId = "N/A";

				if(externalClient == ExternalClientType.Twitter.ToString())
				{
					//Todo: let's tweet out here
					var hashtag = rule.WatchValue;
					var replyTo = $"@{profile.ScreenName}";
					var tweetText = $"{replyTo} {tweetMessage.Replace("[GameName]", sweepGame.Name)} {Environment.NewLine} {hashtag}";

					tweetId = await _twitterHttpService.PublishTweetInResponseToAsync(
						tweetText,
						rule.ExternalId,
						accessToken,
						refreshToken);
				}

				//enter reporting
				await _reportingRepository.UpsertSweepEntryReportAsync(new SweepEntryReport
				{
					CampaignUid = campaign.CampaignUid,
					SweepUid = sweepGame.SweepUid,
					Entries = 1,
					ScreenName = displayName,
					UserIdentifierType = userIdentifierType.ToString(),
					ExternalId = tweetId,
					ExternalUserId = externalProfileId,
					SweepEntryUid = Guid.NewGuid()
				});

				return sweepEntryUid;
			}

			return Guid.Empty;
		}

		private bool SweepIsRunningAndNotYetDrawn(Sweep sweep)
			=> DateTime.UtcNow < sweep.EndDateUtc &&
				DateTime.UtcNow > sweep.StartDateUtc &&
				DateTime.UtcNow < sweep.DrawDateUtc;

		private bool CampaignIsRunning(Campaign campaign)
			=> DateTime.UtcNow < campaign.EndDateUtc &&
				DateTime.UtcNow > campaign.StartDateUtc;

		private async Task<bool> IsFollowing(
			List<Rule> rules,
			Guid ruleUid,
			Profile profile,
			Campaign campaign,
			string externalUserId)
		{
			if (profile == null)
			{
				_logger.LogError("Unable to retrieve the profile belonging to the campaign uid {campaignUid} and campaign name {campaignName}",
					campaign.CampaignUid,
					campaign.CampaignName);

				return false;
			}

			BuildTwitterClient(profile.AccessToken, profile.AccessTokenSecret);

			var followers = await _twitterHttpService.GetFollowersAsync(profile.ExternalProfileId);
			var followersList = followers.ToList();

			return true;
			//if (followersList.Count == 0) return false;

			//var rule = rules.FirstOrDefault(r => r.RuleUid == ruleUid);

			//if (rule != null)
			//{
			//	//if (rule.MustBeFollowing)
			//	//	return followersList.Any(c => c.Id.Equals(externalUserId));
			//	return true;
			//}

			//return true;
		}

		private void BuildTwitterClient(string accessToken, string accessTokenSecret)
		{
			_twitterClient = new Tweetinvi.TwitterClient(
				_twitterAccount.ConsumerKey,
				_twitterAccount.ConsumerSecret,
				accessToken,
				accessTokenSecret);
		}

		private Models.Email CreateMaxEntryEmail(Sweep sweep, Account account)
		{
			return new Email
			{
				Template = EmailTemplate.SweepstakeMaxEntries,
				EmailAddress = account.EmailAddress,
				EmailUid = Guid.NewGuid(),
				Status = EmailStatus.New,
				Parameters = new System.Collections.Generic.Dictionary<string, string>
				{
					{EmailParameters.SweepstakeName, sweep.Name},
					{EmailParameters.StartDate, sweep.StartDate.ToString("f")},
					{EmailParameters.EndDate, sweep.EndDate.ToString("f")},
					{EmailParameters.Email, account.EmailAddress },
					{EmailParameters.EmailAddress, account.EmailAddress },
				}
			};
		}

		private bool UserIsNotInBlacklist(List<BlacklistedSweepUser> blackLIstedUsers, string externalProfileId)
			=> !blackLIstedUsers.Any(c => c.TweetUserId.ToString() == externalProfileId);

		private bool UserCanEnterSweepstake(SweepEntryReport userReports, SweepEntryRule entryRules)
			=> userReports == null ? true : userReports.Entries < entryRules.EntryPerUser;
	}
}

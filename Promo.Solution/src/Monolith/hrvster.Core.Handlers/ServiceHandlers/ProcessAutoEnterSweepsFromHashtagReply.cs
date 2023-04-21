using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class ProcessAutoEnterSweepsFromHashtagReply : AsyncRequestHandler<ProcessAutoEnterSweepsFromHashtagReplyRequest>
	{
		private readonly ILogger<ProcessAutoEnterSweepsFromHashtagReply> _logger;
		private readonly ISweepsEntriesRepository _sweepsEntriesRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly TwitterAccount _twitterAccount;
		private readonly IMediator _mediator;
		private readonly IIncomingTweetsRepository _incomingTweetsRepository;
		private readonly ISweepstakesService _sweepstakeService;
		private readonly ISweepNotificationRepository _sweepNotificationRepository;
		private readonly IMailRepository _mailRepository;
		private readonly IAccountsRepository _accountsRepository;

		private TwitterClient _twitterClient;

		public ProcessAutoEnterSweepsFromHashtagReply(
			ILoggerFactory loggerFactory,
			ISweepsEntriesRepository sweepsEntriesRepository,
			IProfilesRepository profilesRepository,
			TwitterAccount twitterAccount,
			IMediator mediator,
			IIncomingTweetsRepository incomingTweetsRepository,
			ISweepstakesService sweepstakesService,
			ISweepNotificationRepository sweepNotificationRepository,
			IMailRepository mailRepository,
			IAccountsRepository accountsRepository)
		{
			_logger = loggerFactory.CreateLogger<ProcessAutoEnterSweepsFromHashtagReply>();
			_sweepsEntriesRepository = sweepsEntriesRepository;
			_profilesRepository = profilesRepository;
			_twitterAccount = twitterAccount;
			_mediator = mediator;
			_incomingTweetsRepository = incomingTweetsRepository;
			_sweepstakeService = sweepstakesService;
			_sweepNotificationRepository = sweepNotificationRepository;
			_mailRepository = mailRepository;
			_accountsRepository = accountsRepository;
		}

		protected override async Task Handle(ProcessAutoEnterSweepsFromHashtagReplyRequest request, CancellationToken cancellationToken)
		{
			var profile = await _profilesRepository.GetProfileByCampaignUid(request.Rule.Campaign.CampaignUid, ProfileType.Twitter);

			if (CanEnterSweepstakes(request))
			{
				if (!await _sweepstakeService.IsMaxedOut(request.Rule.Sweep, cancellationToken))
				{
					_logger.LogInformation("User can enter sweeps with tweet uid {tweetGuid}",
					request.IncomingTweet.IncomingTweetUid);

					if (await IsFollowing(profile, request))
					{
						await EnterSweepstakesAsync(request);
					}
					else
					{
						_logger.LogInformation("The user {screenName} is not following Profile {accountScreenName}",
							request.Tweet.ScreenName,
							profile.ScreenName);

						await _mediator.Send(new NotifyUserOfNotFollowingProfileRequest(
							request.Tweet,
							profile,
							request.IncomingTweet));
					}
				}
				else
				{
					_logger.LogWarning("Sweepstake {sweepName} with uid {sweepUid} is already maxed out.",
						request.Rule.Sweep.Name,
						request.Rule.Sweep.SweepUid);

					var sweepNotification = await _sweepNotificationRepository.GetSweepNotificationAsync(request.Rule.Sweep.SweepId, cancellationToken);

					var account = await _accountsRepository.GetAsync(request.Rule.Sweep.Account.AccountUid);

					//notify main account user with this information.
					//if we have notified before, then don't notify
					if (sweepNotification == null || !sweepNotification.MaxEntries)
					{
						await _mailRepository.CreateAsync(CreateMaxEntryEmail(request.Rule.Sweep, account));

						await _sweepNotificationRepository.ToggleMaxEntriesAsync(
							request.Rule.Sweep.SweepId,
							cancellationToken);
					}
				}
			}
		}

		private Models.Email CreateMaxEntryEmail(Sweep sweep, Models.Account account)
		{
			return new Models.Email
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

		private async Task<bool> IsFollowing(Profile profile, ProcessAutoEnterSweepsFromHashtagReplyRequest request)
		{
			if (profile == null)
			{
				_logger.LogError("Unable to retrieve the profile belonging to the campaign uid {campaignUid} and campaign name {campaignName}",
					request.Rule.Campaign.CampaignUid,
					request.Rule.Campaign.CampaignName);

				return false;
			}

			BuildTwitterClient(profile.AccessToken, profile.AccessTokenSecret);

			var followers = await _twitterClient.Users.GetFollowerIdsAsync(profile.ScreenName);

			if (followers.Length == 0) return false;

			if (request.Rule.MustBeFollowing)
				return followers.Any(c => c.ToString().Equals(request.Tweet.ExternalUserId));

			return true;
		}

		private void BuildTwitterClient(string accessToken, string accessTokenSecret)
		{
			_twitterClient = new TwitterClient(
				_twitterAccount.ConsumerKey,
				_twitterAccount.ConsumerSecret,
				accessToken,
				accessTokenSecret);
		}

		private async Task EnterSweepstakesAsync(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
		{
			if (request.Rule.Campaign.ActiveInd &&
				CampaignIsRunning(request.Rule.Campaign) &&
				SweepIsRunningAndNotYetDrawn(request.Rule.Sweep))
			{
				_logger.LogInformation("Campaign {campaignName} and Sweepstakes {sweepName} is running and User {screenName} can now be entered into the sweepstakes with tweet Guid {tweetGuid}",
					request.Rule.Campaign.CampaignName,
					request.Rule.Sweep.Name,
					request.Tweet.ScreenName,
					request.IncomingTweet.IncomingTweetUid);

				var sweepEntryUid = Guid.NewGuid();

				await _sweepsEntriesRepository.CreateAsync(new SweepEntry
				{
					AcceptedTerms = true,
					Sweep = request.Rule.Sweep,
					Rule = request.Rule,
					UserIdentifier = $"@{request.Tweet.ScreenName}",
					UserIdentifierType = UserIdentifierType.TwitterHandle,
					SweepEntryUid = sweepEntryUid
				});

				await _mediator.Publish(new SweepEntryFromTweetRecorded(
					request.IncomingTweet,
					request.Rule.Campaign,
					request.Rule.Sweep,
					request.Tweet,
					UserIdentifierType.TwitterHandle));
			}
			else
			{
				_logger.LogWarning("Campaign {campaignName} or Sweepstakes {sweepName} are probably not running right now. And the tweet with uid {tweetGuid} cannot be entered into the sweepstakes",
				request.Rule.Campaign.CampaignName,
				request.Rule.Sweep.Name,
				request.IncomingTweet.IncomingTweetUid);

				await _incomingTweetsRepository.UpdateStatusAsync(
					request.IncomingTweet.IncomingTweetUid,
					Models.Harvester.IncomingTweetStatus.CompletedCampaignOrGameNotRunning);
			}
		}

		private bool SweepIsRunningAndNotYetDrawn(Sweep sweep)
			=> DateTime.UtcNow < sweep.EndDateUtc &&
				DateTime.UtcNow > sweep.StartDateUtc &&
				DateTime.UtcNow < sweep.DrawDateUtc;

		private bool CampaignIsRunning(Campaign campaign)
			=> DateTime.UtcNow < campaign.EndDateUtc &&
				DateTime.UtcNow > campaign.StartDateUtc;

		private bool CanEnterSweepstakes(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
		{
			var userCanEnterSweeps = UserCanEnterSweepstakes(request);
			_logger.LogInformation("User can enter sweepstakes {sweepName}? {canEnter}", request.Rule.Sweep.Name, userCanEnterSweeps);

			var tweetIsInReplyToMainTweet = TweetIsInReplyToRuleTweetId(request);
			_logger.LogInformation("Tweet is in reply to main tweet? {tweetIsInReply}", tweetIsInReplyToMainTweet);

			var userIsNotInBlacklistedUsers = UserIsNotInBlacklistedUsers(request);
			_logger.LogInformation("User is in blacklisted users? {inBlacklistedUsers}", userIsNotInBlacklistedUsers);

			return userCanEnterSweeps && tweetIsInReplyToMainTweet && !userIsNotInBlacklistedUsers;
		}

		private bool UserIsNotInBlacklistedUsers(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
			=> request.BlackListed.Any(c => c.TweetUserId.Equals(request.Tweet.ExternalUserId));

		private bool TweetIsInReplyToRuleTweetId(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
			=> request.Tweet.InReplyToTweetId == request.Rule.ExternalId;

		private bool UserCanEnterSweepstakes(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
			=> request.SweepEntryReport == null
				? true
				: request.SweepEntryReport.Entries < request.EntryRules.EntryPerUser;
	}
}

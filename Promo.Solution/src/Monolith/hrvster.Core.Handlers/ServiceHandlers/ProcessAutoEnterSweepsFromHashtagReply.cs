using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509.Qualified;
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

		private TwitterClient _twitterClient;

		public ProcessAutoEnterSweepsFromHashtagReply(
			ILoggerFactory loggerFactory,
			ISweepsEntriesRepository sweepsEntriesRepository,
			IProfilesRepository profilesRepository,
			TwitterAccount twitterAccount,
			IMediator mediator,
			IIncomingTweetsRepository incomingTweetsRepository)
		{
			_logger = loggerFactory.CreateLogger<ProcessAutoEnterSweepsFromHashtagReply>();
			_sweepsEntriesRepository = sweepsEntriesRepository;
			_profilesRepository = profilesRepository;
			_twitterAccount = twitterAccount;
			_mediator = mediator;
			_incomingTweetsRepository = incomingTweetsRepository;
		}

		protected override async Task Handle(ProcessAutoEnterSweepsFromHashtagReplyRequest request, CancellationToken cancellationToken)
		{
			var profile = await _profilesRepository.GetProfileByCampaignUid(request.Rule.Campaign.CampaignUid, ProfileType.Twitter);

			if (CanEnterSweepstakes(request))
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
						request.Tweet.CreatedBy.ScreenName,
						profile.ScreenName);

					await _mediator.Send(new NotifyUserOfNotFollowingProfileRequest(
						request.Tweet, 
						profile, 
						request.IncomingTweet));
				}
			}
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

			if(request.Rule.MustBeFollowing)
				return followers.Any(c => c.Equals(request.Tweet.CreatedBy.Id));

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
					request.Tweet.CreatedBy.ScreenName,
					request.IncomingTweet.IncomingTweetUid);

				var sweepEntryUid = Guid.NewGuid();

				await _sweepsEntriesRepository.CreateAsync(new SweepEntry
				{
					AcceptedTerms = true,
					Sweep = request.Rule.Sweep,
					Rule = request.Rule,
					UserIdentifier = $"@{request.Tweet.CreatedBy.ScreenName}",
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
					Models.Harvester.IncomingTweetStatus.CompletedCampaignOrSweepstakeNotRunning);
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
			=> request.BlackListed.Any(c => c.TweetUserId.Equals(request.Tweet.CreatedBy.Id));

		private bool TweetIsInReplyToRuleTweetId(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
			=> request.Tweet.InReplyToStatusId.Equals(request.Rule.ExternalId);

		private bool UserCanEnterSweepstakes(ProcessAutoEnterSweepsFromHashtagReplyRequest request)
			=> request.SweepEntryReport == null
				? true
				: request.SweepEntryReport.Entries < request.EntryRules.EntryPerUser;
	}
}

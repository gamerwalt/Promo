using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class ProcessAutoEnterSweepsFromHashtag : AsyncRequestHandler<ProcessAutoEnterSweepsFromHashtagRequest>
	{
		private readonly ILogger<ProcessAutoEnterSweepsFromHashtag> _logger;
		private readonly ISweepsEntriesRepository _sweepsEntriesRepository;
		private readonly IMediator _mediator;

		public ProcessAutoEnterSweepsFromHashtag(
			ILoggerFactory loggerFactory,
			ISweepsEntriesRepository sweepsEntriesRepository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<ProcessAutoEnterSweepsFromHashtag>();
			_sweepsEntriesRepository = sweepsEntriesRepository;
			_mediator = mediator;
		}

		protected override async Task Handle(ProcessAutoEnterSweepsFromHashtagRequest request, CancellationToken cancellationToken)
		{
			if (CanEnterSweepstakes(request))
			{
				await EnterSweepstakesAsync(request);
			}
		}

		private async Task EnterSweepstakesAsync(ProcessAutoEnterSweepsFromHashtagRequest request)
		{
			if (request.Rule.Campaign.ActiveInd &&
				CampaignIsRunning(request.Rule.Campaign) &&
				SweepIsRunningAndNotYetDrawn(request.Rule.Sweep))
			{
				_logger.LogInformation("Campaign {campaignName} and Sweepstakes {sweepName} is running and User {screenName} can now be entered into the sweepstakes.",
					request.Rule.Campaign.CampaignName,
					request.Rule.Sweep.Name,
					request.Tweet.CreatedBy.ScreenName);

				var sweepEntryUid = Guid.NewGuid();

				await _sweepsEntriesRepository.CreateAsync(new SweepEntry
				{
					AcceptedTerms = true,
					Sweep = request.Rule.Sweep,
					UserIdentifier = $"@{request.Tweet.CreatedBy.ScreenName}",
					UserIdentifierType = UserIdentifierType.TwitterHandle,
					SweepEntryUid = sweepEntryUid
				});

				await _mediator.Publish(new SweepEntryFromTweetRecorded(request.Rule.Campaign, request.Rule.Sweep, request.Tweet, UserIdentifierType.TwitterHandle));
			}
			else
			{
				_logger.LogWarning("Campaign {campaignName} or Sweepstakes {sweepName} are probably not running right now.",
				request.Rule.Campaign.CampaignName,
				request.Rule.Sweep.Name);
			}
		}

		private bool SweepIsRunningAndNotYetDrawn(Sweep sweep)
			=> DateTime.UtcNow < sweep.EndDate &&
				DateTime.UtcNow > sweep.StartDate &&
				DateTime.UtcNow < sweep.DrawDate;

		private bool CampaignIsRunning(Campaign campaign)
			=> DateTime.UtcNow < campaign.EndDate &&
				DateTime.UtcNow > campaign.StartDate;

		private bool CanEnterSweepstakes(ProcessAutoEnterSweepsFromHashtagRequest request)
		{
			var userCanEnterSweeps = UserCanEnterSweepstakes(request);
			_logger.LogInformation("User can enter sweepstakes {sweepName}? {canEnter}", request.Rule.Sweep.Name, userCanEnterSweeps);

			var tweetIsInReplyToMainTweet = TweetIsInReplyToRuleTweetId(request);
			_logger.LogInformation("Tweet is in reply to main tweet? {tweetIsInReply}", tweetIsInReplyToMainTweet);

			var userIsNotInBlacklistedUsers = UserIsNotInBlacklistedUsers(request);
			_logger.LogInformation("User is in blacklisted users? {inBlacklistedUsers}", !userIsNotInBlacklistedUsers);

			return userCanEnterSweeps && tweetIsInReplyToMainTweet && !userIsNotInBlacklistedUsers;
		}

		private bool UserIsNotInBlacklistedUsers(ProcessAutoEnterSweepsFromHashtagRequest request)
			=> request.BlackListed.Any(c => c.Equals(request.Tweet.CreatedBy.Id));

		private bool TweetIsInReplyToRuleTweetId(ProcessAutoEnterSweepsFromHashtagRequest request)
			=> request.Tweet.InReplyToStatusId.Equals(request.Rule.ExternalId);

		private bool UserCanEnterSweepstakes(ProcessAutoEnterSweepsFromHashtagRequest request)
			=> request.SweepEntryReport == null
				? true
				: request.SweepEntryReport.Entries < request.EntryRules.EntryPerUser;
	}
}

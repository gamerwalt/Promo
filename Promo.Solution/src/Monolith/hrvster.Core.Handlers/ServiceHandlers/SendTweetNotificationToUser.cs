using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class SendTweetNotificationToUser : INotificationHandler<SweepEntryFromTweetRecorded>
	{
		private readonly ILogger<SendTweetNotificationToUser> _logger;
		private readonly ISweepPrizesRepository _sweepPrizesRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;

		public SendTweetNotificationToUser(
			ILoggerFactory loggerFactory,
			ISweepPrizesRepository sweepPrizesRepository,
			IProfilesRepository profilesRepository,
			Func<ExternalClientType, Clients.Publishers.IExternalClient> publisherResolver)
		{
			_logger = loggerFactory.CreateLogger<SendTweetNotificationToUser>();
			_sweepPrizesRepository = sweepPrizesRepository;
			_profilesRepository = profilesRepository;
			_publisherResolver = publisherResolver;
		}

		public async Task Handle(SweepEntryFromTweetRecorded notification, CancellationToken cancellationToken)
		{
			var prizes = await _sweepPrizesRepository.GetSweepPrizesAsync(notification.Sweep.SweepUid);
			var totalValue = prizes.Sum(c => c.PrizeValue);

			var tweetText = $"@{notification.Tweet.CreatedBy.ScreenName} {DateTime.UtcNow:MMM dd, yyyy hh:mm:fffff} - You just got entered into a sweepstakes entry. Total value of Ngn{string.Format("{0:n}", totalValue)} to be won.";
			var prizesText = string.Empty;

			if(prizes == null)
			{
				_logger.LogWarning("Harvester does not have information on the prizes for sweeps with uid : {sweepName}",
					notification.Sweep.SweepUid);

				prizesText = $"@{notification.Tweet.CreatedBy} There are no prizes to win at this time.";
			}
			else
			{
				var prizesToWin = string.Join(", ", prizes.ToList().Select(c => c.PrizeName));
				prizesText = $"@{notification.Tweet.CreatedBy.ScreenName} {DateTime.UtcNow:MMM dd, yyyy hh:mm:ss:fffff} - Prizes to win: {prizesToWin}";
				if (prizesText.Length > 135)
				{
					prizesText = $"{prizesText.Substring(0, 140)}...";
				}
			}

			var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);

			var profile = await _profilesRepository.GetProfileByCampaignUid(notification.Campaign.CampaignUid, ProfileType.Twitter);
			if (profile == null)
			{
				_logger.LogError("Unable to retrieve the profile belonging to the campaign uid {campaignUid} and campaign name {campaignName}",
					notification.Campaign.CampaignUid,
					notification.Campaign.CampaignName);

				return;
			}

			var tweetId = await twitterPublisher
				.PublishResponse(
					new Message
					{
						Text = tweetText,
						Profile = profile
					}, 
					notification.Tweet.IdStr);

			var replyToTweet = await twitterPublisher.PublishResponse(new Message { Text = prizesText, Profile = profile }, tweetId);
		}
	}
}

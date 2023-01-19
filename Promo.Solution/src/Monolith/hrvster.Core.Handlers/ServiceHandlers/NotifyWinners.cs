using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class NotifyWinners : INotificationHandler<WinnersSaved>
	{
		private readonly ILogger<NotifyWinners> _logger;
		private readonly ISweepPrizesRepository _sweepPrizesRepository;
		private readonly IProfilesRepository _profilesRepository;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;

		public NotifyWinners(
			ILoggerFactory loggerFactory, 
			ISweepPrizesRepository sweepPrizesRepository,
			IProfilesRepository profilesRepository,
			Func<ExternalClientType, IExternalClient> publisherResolver)
		{
			_logger = loggerFactory.CreateLogger<NotifyWinners>();
			_sweepPrizesRepository = sweepPrizesRepository;
			_profilesRepository = profilesRepository;
			_publisherResolver = publisherResolver;
		}

		public async Task Handle(WinnersSaved notification, CancellationToken cancellationToken)
		{
			var sweep = notification.DrawProperties.SweepDrawPropertiesAndRule.Sweep;
			var campaign = notification.DrawProperties.SweepDrawPropertiesAndRule.Campaign;
			var sweepRule = notification.DrawProperties.SweepDrawPropertiesAndRule.SweepRule;

			var profile = await _profilesRepository.GetProfileByCampaignUid(campaign.CampaignUid, ProfileType.Twitter);

			if(profile == null)
			{
				_logger.LogError("Could not load the twitter account profile for the campaign with Uid {campaignUid}", campaign.CampaignUid);

				return;
			}

			if(sweepRule.NotifyWinners)
			{
				var sweepPrizes = await _sweepPrizesRepository.GetSweepPrizesAsync(sweep.SweepUid);
				if(sweepPrizes == null || sweepPrizes.Count() == 0)
				{
					_logger.LogError("Sweep {sweepName} has no prizes. This needs to be fixed or configured ASAP. Winners has already been saved for sweep uid : {sweepUid}",
					sweep.Name,
					sweep.SweepUid);
				}

				_logger.LogInformation("Notfying winners...");

				foreach(var winner in notification.Winners)
				{
					_logger.LogInformation("Attempting to send a message to {screenName}", winner.UserIdentifier);

					var message = $"{winner.UserIdentifier} - {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss")} - You are a potential winner for the sweepstakes {sweep.Name} ran by @hrvsterHQ for @{profile.ScreenName}";

					var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
					var tweetId = await twitterPublisher.PublishAsync(new Message
					{
						Profile = profile,
						Text = message
					});

					var contactMessage = $"Please contact @{profile.ScreenName} about possible prizes and how they can be claimed.";
					tweetId = await twitterPublisher.PublishResponse(new Message
					{
						Profile = profile,
						Text = contactMessage
					}, tweetId);

					if(!string.IsNullOrWhiteSpace(sweep.RulesUrl))
					{
						var ruleMessage = $"Please visit {sweep.RulesUrl} to know more.";
						await twitterPublisher.PublishResponse(new Message
						{
							Profile = profile,
							Text = ruleMessage
						}, tweetId);
					}
				}
			}

			_logger.LogInformation("Not notifying winners. Rules says not to notify winners for sweeps {name}", sweep.Name);
		}
	}
}

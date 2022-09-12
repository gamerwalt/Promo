using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Requests.ServiceRequests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
		private readonly IConfiguration _config;
		private readonly IExternalClient _twitterPublisher;

		public SendTweetNotificationToUser(
			ILoggerFactory loggerFactory,
			ISweepPrizesRepository sweepPrizesRepository,
			IProfilesRepository profilesRepository,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			IConfiguration config)
		{
			_logger = loggerFactory.CreateLogger<SendTweetNotificationToUser>();
			_sweepPrizesRepository = sweepPrizesRepository;
			_profilesRepository = profilesRepository;
			_publisherResolver = publisherResolver;
			_config = config;

			_twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
		}

		public async Task Handle(SweepEntryFromTweetRecorded notification, CancellationToken cancellationToken)
		{
			var prizes = await GetPrizesAsync(notification);
			var profile = await GetProfileAsync(notification);

			var totalValue = prizes.Sum(c => c.PrizeValue);

			var tweetText = $"@{notification.Tweet.CreatedBy.ScreenName} {DateTime.UtcNow:MMM dd, yyyy hh:mm:fffff} - You just got entered into a sweepstakes entry. Total value of Ngn{string.Format("{0:n}", totalValue)} to be won.";
			
			if (profile == null)
			{
				_logger.LogError("Unable to retrieve the profile belonging to the campaign uid {campaignUid} and campaign name {campaignName}",
					notification.Campaign.CampaignUid,
					notification.Campaign.CampaignName);

				return;
			}

			await PublishTweets(notification, _twitterPublisher, tweetText, profile);
		}

		private async Task<Profile> GetProfileAsync(SweepEntryFromTweetRecorded notification)
			=> await _profilesRepository.GetProfileByCampaignUid(notification.Campaign.CampaignUid, ProfileType.Twitter);

		private async Task<IEnumerable<SweepPrize>> GetPrizesAsync(SweepEntryFromTweetRecorded notification)
			=> await _sweepPrizesRepository.GetSweepPrizesAsync(notification.Sweep.SweepUid);

		private async Task PublishTweets(SweepEntryFromTweetRecorded notification, IExternalClient twitterPublisher, string tweetText, Profile profile)
			=> await SendMainTweet(notification, twitterPublisher, tweetText, profile)
				.ContinueWith(tweetId => SendRuleTweet(notification, twitterPublisher, profile, tweetId.Result));

			//await SendRuleTweet(notification, twitterPublisher, profile, tweetId);

		private async Task SendRuleTweet(SweepEntryFromTweetRecorded notification, IExternalClient twitterPublisher, Profile profile, string tweetId)
			=> await twitterPublisher.PublishResponse(
				new Message
				{
					Text = GetRuleText(notification.Sweep),
					Profile = profile
				},
				tweetId);

		private static async Task<string> SendMainTweet(SweepEntryFromTweetRecorded notification, IExternalClient twitterPublisher, string tweetText, Profile profile)
			=> await twitterPublisher.PublishResponse(
				new Message
				{
					Text = tweetText,
					Profile = profile
				},
				notification.Tweet.IdStr);

		private string GetRuleText(Sweep sweep)
		{
			//Todo: We need to do this better
			var url = _config["hrvsterUrls"];
			var uriString = $"{url}/{Constants.GameRulesPath}/{GameTypes.Sweepstake}/NG/en/{sweep.SweepUid}";

			return $"Visit {uriString} to view the official rules of the sweepstake.";
		}
	}
}

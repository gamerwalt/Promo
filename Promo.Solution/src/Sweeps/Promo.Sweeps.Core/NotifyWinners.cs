using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Core.Models;
using Promo.Sweeps.Core.Notifications;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Models.Properties;
using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Promo.Sweeps.Core
{
	public class NotifyWinners : INotificationHandler<WinnerSaved>
	{
		private readonly ILogger<NotifyWinners> _logger;
		private readonly IRulesClient _rulesClient;
		private readonly ITwitterClient _twitterClient;
		private readonly IHarvesterClient _harvesterClient;

		public NotifyWinners(
			ILoggerFactory loggerFactory, 
			IRulesClient rulesClient, 
			ITwitterClient twitterClient, 
			IHarvesterClient harvesterClient)
		{
			_logger = loggerFactory.CreateLogger<NotifyWinners>();
			_rulesClient = rulesClient;
			_twitterClient = twitterClient;
			_harvesterClient = harvesterClient;
		}

		public async Task Handle(WinnerSaved notification, CancellationToken cancellationToken)
		{
			//Todo: Setup prizes and pull sweep prize information based on winner's position
			_logger.LogInformation("Notifying winners of their win.");
			var message = $"@screenName You are a potential winner for the sweepstakes {notification.SweepDrawRule.SweepName} ran by [Company/Personality].";
			//we need the rule/sweepstakes with it's rules to reference what made them win

			foreach(var winner in notification.Winners)
			{
				try
				{
					_logger.LogInformation($"Sending information to {winner.UserIdentifier}");
					var tweet = await _twitterClient.Tweets.PublishTweetAsync(message.Replace("@screenName", winner.UserIdentifier));

					var newMessage = $"See contest rules {notification.SweepDrawRule.RulesUrl} for more details.";

					await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(newMessage) { InReplyToTweet = tweet });
				}
				catch (TwitterException tex)
				{
					_logger.LogError(tex.Message);
					//Todo: Send this to a queue to be sent later
				}
			}
			
		}

		private async Task<HarvestedUser> GetWinnerIdentifierAsync(string userIdentifier)
		{
			return await _harvesterClient.GetUserAsync(userIdentifier);
		}
	}
}

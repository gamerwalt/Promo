using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Core.Models;
using Promo.Sweeps.Core.Notifications;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
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
		private readonly IPrizesClient _prizesClient;

		public NotifyWinners(
			ILoggerFactory loggerFactory,
			IRulesClient rulesClient,
			ITwitterClient twitterClient,
			IHarvesterClient harvesterClient,
			IPrizesClient prizesClient)
		{
			_logger = loggerFactory.CreateLogger<NotifyWinners>();
			_rulesClient = rulesClient;
			_twitterClient = twitterClient;
			_harvesterClient = harvesterClient;
			_prizesClient = prizesClient;
		}

		public async Task Handle(WinnerSaved notification, CancellationToken cancellationToken)
		{
			var sweepPrizes = await _prizesClient.GetAllSweepPrizesAsync(notification.SweepDrawRule.SweepUid);
			if (sweepPrizes == null || sweepPrizes.Count() == 0)
			{
				_logger.LogError("Unable to continue. Sweep {sweepName} has no prizes. This needs to be fixed or configured ASAP. Winners has already been saved for sweep uid : {sweepUid}",
					notification.SweepDrawRule.SweepName,
					notification.SweepDrawRule.SweepUid);

				var counter = 1;
				foreach(var winner in notification.Winners)
				{
					_logger.LogInformation($"Sending information to {winner.UserIdentifier}");
					
					var winningMessage = "";
					if(counter == 1)
					{
						winningMessage = $"{winner.UserIdentifier} {DateTime.UtcNow:MMM dd, yyyy hh:mm:ss:fffff} -You are potentially the grand prize winner. However since there are no prizes, we have marked you as the grand winner.";
					}
					else
					{
						winningMessage = $"{winner.UserIdentifier} {DateTime.UtcNow:MMM dd, yyyy hh:mm:ss:fffff} -You are potentially a winner. However since there are no prizes, we have marked you as one of the winners.";
					}

					counter++;
					_logger.LogInformation("Sending {message} to {screenName}", winningMessage, winner.UserIdentifier);

					var originalTweet = await _twitterClient.Tweets.PublishTweetAsync(winningMessage);
				}

				return;
			}

			_logger.LogInformation("Notifying winners of their win.");
			var message = $"@screenName - {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss")} - You are a potential winner for the sweepstakes {notification.SweepDrawRule.SweepName} ran by @hrvsterHQ.";

			var prizePosition = 1;

			foreach (var winner in notification.Winners)
			{
				_logger.LogInformation($"Sending information to {winner.UserIdentifier}");
				
				try
				{
					var prizeName = sweepPrizes.FirstOrDefault(c => c.PrizePosition.Equals(prizePosition))?.PrizeName;

					var originalTweet = await _twitterClient.Tweets.PublishTweetAsync(message.Replace("@screenName", winner.UserIdentifier));

					var newMessage = "";
					if (prizePosition == 1)
					{
						newMessage = $"{winner.UserIdentifier} {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss:fffff")} - You have potentially won the grand prize of {prizeName}.";
					}
					else
					{
						newMessage = $"{winner.UserIdentifier} {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss:fffff")} - You have potentially won {prizeName}";
					}
					var prizeTweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(newMessage) { InReplyToTweet = originalTweet });

					newMessage = $"{winner.UserIdentifier} {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss:fffff")} - We will contact you with details on how to claim your prize.";
					await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(newMessage) { InReplyToTweet = prizeTweet });

					//Todo: once we implement the rule pages
					//newMessage = $"{winner.UserIdentifier} {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss")} - See contest rules {notification.SweepDrawRule.RulesUrl} for more details.";
					//await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(newMessage) { InReplyToTweet = prizeTweet });

					prizePosition++;
				}
				catch (TwitterException tex)
				{
					_logger.LogError(tex.Message);
					//Todo: Send this to a queue or a failed send database to be processed or sent later
				}
			}

		}

		private async Task<HarvestedUser> GetWinnerIdentifierAsync(string userIdentifier)
		{
			return await _harvesterClient.GetUserAsync(userIdentifier);
		}
	}
}

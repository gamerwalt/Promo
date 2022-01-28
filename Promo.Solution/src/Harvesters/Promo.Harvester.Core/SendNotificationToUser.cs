using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Harvester.Core.Notifications;
using Promo.Harvester.Infrastructure;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace Promo.Harvester.Core
{
	public class SendNotificationToUser : INotificationHandler<SweepEntryRecorded>
	{
		private readonly ITwitterClient _twitterClient;
		private readonly ISweepPrizeRepository _sweepPrizeRepository;
		private readonly ILogger<SendNotificationToUser> _logger;

		public SendNotificationToUser(ITwitterClient twitterClient, ISweepPrizeRepository sweepPrizeRepository, ILoggerFactory loggerFactory)
		{
			_twitterClient = twitterClient;
			_sweepPrizeRepository = sweepPrizeRepository;
			_logger = loggerFactory.CreateLogger<SendNotificationToUser>();
		}

		public async Task Handle(SweepEntryRecorded notification, CancellationToken cancellationToken)
		{
			var tweet = await _twitterClient.Tweets.GetTweetAsync(long.Parse(notification.TweetId));
			var prizes = await _sweepPrizeRepository.GetSweepPrizesAsync(notification.SweepUid);
			var totalValue = prizes.Sum(c => c.PrizeValue);

			var tweetText = $"@{tweet.CreatedBy} {DateTime.UtcNow:MMM dd, yyyy hh:mm:fffff} - You just got entered into a sweepstakes entry. Total value of Ngn{string.Format("{0:n}", totalValue)} to be won.";
			var prizesText = "";

			if (prizes == null)
			{
				_logger.LogWarning("Harvester does not have information on the prizes for sweeps with uid : {sweepName}", notification.SweepUid);
				prizesText = $"@{tweet.CreatedBy} There are no prizes to win at this time.";
			}
			else
			{
				var prizesToWin = string.Join(", ", prizes.ToList().Select(c => c.PrizeName));
				prizesText = $"@{tweet.CreatedBy} {DateTime.UtcNow:MMM dd, yyyy hh:mm:ss:fffff} - Prizes to win: {prizesToWin}";
				if (prizesText.Length > 135)
				{
					prizesText = $"{prizesText.Substring(0, 140)}...";
				}
			}

			var publishedTweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(tweetText) { InReplyToTweet = tweet });

			var replyToPublishTweet = await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(prizesText) { InReplyToTweet = publishedTweet });

			//var reminderMessage = $"@{tweet.CreatedBy} {DateTime.UtcNow:MMM dd, yyyy hh:mm:fffff} - You have 10 entries. More replies with the hashtag increases your chances of winning.";

			//await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters(reminderMessage) { InReplyToTweet = replyToPublishTweet });
		}
	}
}

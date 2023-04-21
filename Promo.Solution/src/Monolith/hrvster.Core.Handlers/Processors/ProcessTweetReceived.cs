using hrvster.ApplicationServices.Processors;
using hrvster.Core.Requests.ServiceRequests.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Processors
{
	public class ProcessTweetReceived : INotificationHandler<TweetReceived>
	{
		private readonly ILogger<ProcessTweetReceived> _logger;
		private readonly ITweetProcessor _tweetProcessor;

		public ProcessTweetReceived(ILoggerFactory loggerFactory, ITweetProcessor tweetProcessor)
		{
			_logger = loggerFactory.CreateLogger<ProcessTweetReceived>();
			_tweetProcessor = tweetProcessor;
		}

		public async Task Handle(TweetReceived notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation("{action} handling notification to process tweet with uid {tweetGuid} and hashtags {hashtags}.",
				nameof(ProcessTweetReceived),
				notification.IncomingTweet.IncomingTweetUid,
				string.Join(", ", notification.Tweet.Hashtags.Select(c => c)));

			await _tweetProcessor.ProcessTweetAsync(
				notification.IncomingTweet,
				notification.Tweet, 
				notification.Rule, 
				notification.UserUid, 
				notification.HarvestedUid);
		}
	}
}

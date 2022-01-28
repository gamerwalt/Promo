using Microsoft.Extensions.Logging;
using Promo.Clients.Internal;
using Promo.Harvester.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.V2;

namespace Promo.Harvester.Service
{
    public interface IStreamService
    {
        Task StartStreamingAsync(CancellationToken token);
    }

    public class StreamService : IStreamService
    {
        private readonly ITwitterClient _twitterClient;
        private readonly IRulesClient _rulesClient;
        private readonly IHarvesterService _harvesterService;
		private readonly ILogger<StreamService> _logger;

        private IFilteredStream currentStream;

        public StreamService(ITwitterClient twitterClient,
            IRulesClient rulesClient,
            IHarvesterService harvesterService,
			ILoggerFactory loggerFactory)
        {
            _twitterClient = twitterClient;
            _rulesClient = rulesClient;
            _harvesterService = harvesterService;
			_logger = loggerFactory.CreateLogger<StreamService>();

            currentStream = _twitterClient.Streams.CreateFilteredStream();
        }

        public async Task StartStreamingAsync(CancellationToken token)
        {
            var hashTagRules = await _rulesClient.GetHashTagRulesAsync();

            if (hashTagRules != null)
            {
                foreach (var rule in hashTagRules)
                {
                    if (rule.ActiveInd)
                    {
						_logger.LogInformation("Adding {watchValue} to stream service for rules to look out for.", rule.WatchValue);
						currentStream.AddTrack(rule.WatchValue);
                    }
                }
            }

			if(currentStream.TracksCount > 0)
			{
				currentStream.MatchingTweetReceived += CurrentStream_MatchingTweetReceived;

				_logger.LogInformation("Started streaming fromt twitter.");

				await currentStream.StartMatchingAnyConditionAsync();
			}
        }

        private void CurrentStream_MatchingTweetReceived(object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs e)
        {
            Task.Run(async () => await _harvesterService.SaveTweetReceivedAsync(e.Json, e.Tweet)).Wait();
        }
    }
}

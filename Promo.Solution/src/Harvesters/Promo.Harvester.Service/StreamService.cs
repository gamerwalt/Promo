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

        private IFilteredStream currentStream;

        public StreamService(ITwitterClient twitterClient,
            IRulesClient rulesClient,
            IHarvesterService harvesterService)
        {
            _twitterClient = twitterClient;
            _rulesClient = rulesClient;
            _harvesterService = harvesterService;

            currentStream = _twitterClient.Streams.CreateFilteredStream();
        }

        public async Task StartStreamingAsync(CancellationToken token)
        {
            var hashTagRules = await _rulesClient.GetHashTagRulesAsync();

            if (hashTagRules.Any())
            {
                foreach (var rule in hashTagRules)
                {
                    if (rule.ActiveInd)
                    {
                        currentStream.AddTrack(rule.WatchValue);
                    }
                }
            }

            currentStream.MatchingTweetReceived += CurrentStream_MatchingTweetReceived;

            await currentStream.StartMatchingAnyConditionAsync();
        }

        private void CurrentStream_MatchingTweetReceived(object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs e)
        {
            Task.Run(async () => await _harvesterService.SaveTweetReceivedAsync(e.Json, e.Tweet)).Wait();
        }
    }
}

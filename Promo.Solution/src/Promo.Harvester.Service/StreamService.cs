using Promo.Clients.Internal;
using Promo.Harvester.Infrastruture.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Streaming;

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

        public StreamService(
            ITwitterClient twitterClient,
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
            var tracksNeededToBeAdded = await _rulesClient.GetRulesNeededToBeWatchedAsync();
            await FilterRunningCampaignsAsync(tracksNeededToBeAdded);

            if (tracksNeededToBeAdded.Count() > 0)
            {
                foreach (var track in tracksNeededToBeAdded)
                {
                    if (track.ActiveInd)
                    {
                        currentStream.AddTrack(track.WatchValue);
                        await _rulesClient.UpdateRuleAsAddedAsync(track.RuleUid);
                    }
                }
            }

            currentStream.MatchingTweetReceived += CurrentStream_MatchingTweetReceived;

            await currentStream.StartMatchingAnyConditionAsync();
        }

        private void CurrentStream_MatchingTweetReceived(object sender, Tweetinvi.Events.MatchedTweetReceivedEventArgs e)
        {
            System.Console.WriteLine(e.Json);
            Task.Run(async () => await _harvesterService.AddAsync(e.Json)).Wait();
        }

        private async Task FilterRunningCampaignsAsync(IEnumerable<Core.Models.Rule> tracksNeededToBeAdded)
        {
            //campaign client of some sort that is able to tell is if a campaign is currently live or not
        }
    }
}

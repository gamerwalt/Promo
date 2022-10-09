using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class GetSweepForAccountUid : IRequestHandler<GetSweepForAccountUidRequest, SweepstakeDetails>
	{
		private readonly ILogger<GetSweepForAccountUid> _logger;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly IHarvesterRepository _harvesterRepository;
		private readonly ISweepsEntriesRepository _sweepsEntriesRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;

		public GetSweepForAccountUid(
			ILoggerFactory loggerFactory,
			ISweepsRepository sweepsRepository,
			ISweepsEntriesRepository sweepsEntriesRepository,
			IHarvesterRepository harvesterRepository,
			ISweepRulesRepository sweepRulesRepository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepForAccountUid>();
			_sweepsRepository = sweepsRepository;
			_sweepsEntriesRepository = sweepsEntriesRepository;
			_harvesterRepository = harvesterRepository;
			_sweepRulesRepository = sweepRulesRepository;
		}

		public async Task<SweepstakeDetails> Handle(GetSweepForAccountUidRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweeps for account {accountUid}", request.AccountUid);

			var sweeps = await _sweepsRepository.GetAllForAccountAsync(request.AccountUid);
			var sweepsOrdered = sweeps.ToList().OrderBy(c => c.EndDate);

			var sweepstakeDetail = new SweepstakeDetails();
			sweepstakeDetail.SweepDetails = new List<SweepDetail>();

			foreach (var sweep in sweepsOrdered)
			{
				var entries = await GetSweepEntriesFor(sweep);

				var sweepDetail = new SweepDetail
				{
					Sweepstake = sweep,
					SweepEntries = entries.ToList(),
					TweetsHarvested = await GetTweetsHarvestedFor(sweep)
				};

				sweepstakeDetail.SweepDetails.Add(sweepDetail);
			}

			return sweepstakeDetail;
		}

		private async Task<List<SweepTweet>> GetTweetsHarvestedFor(Sweep sweep)
		{
			_logger.LogInformation("Retrieving harvested tweets for sweep {sweepName}", sweep.Name);

			var rules = await _sweepRulesRepository.GetSweepRuleAsync(sweep.SweepUid);
			var sweepTweets = new List<SweepTweet>();
			foreach (var rule in rules)
			{
				var harvesteds = await _harvesterRepository.GetTweetsAsync(rule.RuleUid);

				foreach (var harvested in harvesteds)
				{
					sweepTweets.Add(new SweepTweet
					{
						ExternalId = harvested.ExternalTweetId,
						ExternalUserId = harvested.ExternalUserId,
						ProfileImageUrl = harvested.ProfileImageUrl,
						ScreenName = harvested.ExternalScreenName,
						TweetText = harvested.TweetText
					});
				}
			}

			return sweepTweets;
		}

		private async Task<IEnumerable<SweepEntry>> GetSweepEntriesFor(Sweep sweep)
			=> await _sweepsEntriesRepository.GetSweepEntriesAsync(sweep.SweepUid);

	}
}

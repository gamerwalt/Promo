using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class DrawWinnersForSweepstakes : IRequestHandler<DrawWinnersForSweepstakesRequest, List<SweepWinner>>
	{
		private readonly ILogger<DrawWinnersForSweepstakes> _logger;
		private readonly DrawSettingOptions _drawSettingOptions;
		private readonly IRandomizer _randomizer;

		public DrawWinnersForSweepstakes(
			ILoggerFactory loggerFactory,
			DrawSettingOptions drawSettingOptions,
			IRandomizer randomizer)
		{
			_logger = loggerFactory.CreateLogger<DrawWinnersForSweepstakes>();
			_drawSettingOptions = drawSettingOptions;
			_randomizer = randomizer;
		}

		public async Task<List<SweepWinner>> Handle(DrawWinnersForSweepstakesRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Drawing winners for sweepstake {sweepName} and campaign {campaignName}",
				request.DrawProperties.SweepDrawPropertiesAndRule.Sweep.Name,
				request.DrawProperties.SweepDrawPropertiesAndRule.Campaign.CampaignName);

			var winnerList = new List<SweepWinner>();
			var sweep = request.DrawProperties.SweepDrawPropertiesAndRule.Sweep;
			var sweepEntries = request.DrawProperties.SweepEntries;
			var campaign = request.DrawProperties.SweepDrawPropertiesAndRule.Campaign;
			var drawRule = request.DrawProperties.SweepDrawPropertiesAndRule.DrawRule;

			for (var winnerIndex = 1; winnerIndex <= drawRule.WinnerCount; winnerIndex++)
			{
				_logger.LogInformation("Drawing winner {winnerIndex} for Sweep {sweepName} with uid {sweepUid}",
					winnerIndex,
					sweep.Name,
					sweep.SweepUid);

				var drawWinnerCounter = 0;
				var winnerSweepEntryId = 0;
				var sweepEntry = default(SweepEntry);

				while (drawWinnerCounter < _drawSettingOptions.NumberOfTriesSelectingADistinctWinner)
				{
					winnerSweepEntryId = _randomizer.GetRandomNumberFromSweepEntries(
						request.DrawProperties.FirstEntryId,
						request.DrawProperties.LastEntryId,
						request.DrawProperties.MissingBetween,
						sweepEntries);

					if (!IsInWinnersList(winnerList, sweepEntries, winnerSweepEntryId))
					{
						sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));
						if (sweepEntry != null)
							break;
					}

					drawWinnerCounter++;
				}

				_logger.LogInformation("Winner for {winnerIndex} drawn. SweepEntryUid: {sweepEntryUid} Picked",
					winnerIndex,
					sweepEntry.SweepEntryUid);

				request.DrawProperties.MissingBetween.Add(winnerSweepEntryId);

				_logger.LogInformation("Winner picked for sweepstake {sweepName} and campaign {campaignName} is {userIdentifier}.",
					sweep.Name,
					campaign.CampaignName,
					sweepEntry.UserIdentifier);

				winnerList.Add(new SweepWinner
				{
					SweepWinnerId = winnerIndex,
					SweepEntry = new SweepEntry
					{
						SweepEntryId = winnerSweepEntryId,
						SweepEntryUid = sweepEntry.SweepEntryUid,
						UserIdentifier = sweepEntry.UserIdentifier,
						UserIdentifierType = sweepEntry.UserIdentifierType
					},
					UserIdentifier = sweepEntry.UserIdentifier,
					UserIdentifierType = sweepEntry.UserIdentifierType
				});

				sweepEntries.RemoveAll(c => c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.ToString().Equals(sweepEntry.UserIdentifierType.ToString()));
			}

			_logger.LogInformation("Finished drawing winners for sweepstake {sweepName} and campaign {campaignName}",
				request.DrawProperties.SweepDrawPropertiesAndRule.Sweep.Name,
				request.DrawProperties.SweepDrawPropertiesAndRule.Campaign.CampaignName);

			return winnerList;
		}

		private bool IsInWinnersList(List<SweepWinner> winnerList, IEnumerable<SweepEntry> sweepEntries, int winnerSweepEntryId)
		{
			var sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));

			if (sweepEntry == null) return false;

			return winnerList.Any(c => c.SweepEntry.SweepEntryId.Equals(winnerSweepEntryId) ||
				(c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.Equals(sweepEntry.UserIdentifierType.ToString()))
			);
		}
	}
}

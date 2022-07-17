using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class DrawBackupWinnersForSweepstakes : IRequestHandler<DrawBackupWinnersForSweepstakesRequest, List<SweepWinner>>
	{
		private readonly ILogger<DrawBackupWinnersForSweepstakes> _logger;
		private readonly DrawSettingOptions _drawSettingOptions;
		private readonly IRandomizer _randomizer;

		public DrawBackupWinnersForSweepstakes(
			ILoggerFactory loggerFactory, 
			DrawSettingOptions drawSettingOptions,
			IRandomizer randomizer)
		{
			_logger = loggerFactory.CreateLogger<DrawBackupWinnersForSweepstakes>();
			_drawSettingOptions = drawSettingOptions;
			_randomizer = randomizer;
		}

		public async Task<List<SweepWinner>> Handle(DrawBackupWinnersForSweepstakesRequest request, CancellationToken cancellationToken)
		{
			var sweep = request.DrawProperties.SweepDrawPropertiesAndRule.Sweep;
			var sweepEntries = request.DrawProperties.SweepEntries;
			var campaign = request.DrawProperties.SweepDrawPropertiesAndRule.Campaign;
			var drawRule = request.DrawProperties.SweepDrawPropertiesAndRule.DrawRule;

			_logger.LogInformation("Drawing backup winners for sweepstake {sweepName} and campaign {campaignName}",
				sweep.Name,
				campaign.CampaignName);

			foreach(var winner in request.Winners)
			{
				for(var backupWinnerIndex = 1; backupWinnerIndex <= drawRule.WinnerBackupCount; backupWinnerIndex++)
				{
					_logger.LogInformation("Drawing backup winner {backupWinnerIndex} for Winner-SweepEntryUid {winnerSweepEntryUid} for Sweep {sweepName} and campaign {campaignName}", 
						backupWinnerIndex, 
						winner.SweepEntry.SweepEntryUid, 
						sweep.Name,
						campaign.CampaignName);

					var drawWinnerCounter = 0;
					var backupWinnerSweepEntryId = 0;
					var sweepEntry = default(SweepEntry);

					while (drawWinnerCounter < _drawSettingOptions.NumberOfTriesSelectingADistinctWinner)
					{
						backupWinnerSweepEntryId = _randomizer.GetRandomNumberFromSweepEntries(
							request.DrawProperties.FirstEntryId,
							request.DrawProperties.LastEntryId,
							request.DrawProperties.MissingBetween,
							sweepEntries);

						if (!IsInWinnersAndBackupWinnersList(request.Winners, sweepEntries, backupWinnerSweepEntryId))
						{
							sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(backupWinnerSweepEntryId));
							if (sweepEntry != null)
								break;
						}

						drawWinnerCounter++;
					}

					_logger.LogInformation("Backup Winner for {backupWinnerIndex} drawn. SweepEntryUid: {sweepEntryUid} Picked", backupWinnerIndex, sweepEntry.SweepEntryUid);

					request.DrawProperties.MissingBetween.Add(backupWinnerSweepEntryId);

					winner.BackupWinners.Add(new SweepBackupWinner
					{
						SweepWinnerId = backupWinnerIndex,
						SweepEntry = new SweepEntry
						{
							SweepEntryId = backupWinnerSweepEntryId,
							SweepEntryUid = sweepEntry.SweepEntryUid,
							UserIdentifier = sweepEntry.UserIdentifier,
							UserIdentifierType = sweepEntry.UserIdentifierType
						},
						UserIdentifier = sweepEntry.UserIdentifier,
						UserIdentifierType = sweepEntry.UserIdentifierType
					});

					request.DrawProperties.SweepEntries.RemoveAll(c => c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.ToString().Equals(sweepEntry.UserIdentifierType.ToString()));
				}
			}

			_logger.LogInformation("Finished drawing backup winners for sweepstake {sweepName} and campaign {campaignName}",
				sweep.Name,
				campaign.CampaignName);

			return request.Winners;
		}

		private bool IsInWinnersAndBackupWinnersList(List<SweepWinner> winnerList, List<SweepEntry> sweepEntries, int winnerSweepEntryId)
		{
			var sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));

			var inWinnerList = DrawnWinnerInWinnersList(winnerList, sweepEntries, winnerSweepEntryId);

			return inWinnerList &&
				winnerList.Any(
					winner => winner.BackupWinners.Any(backupWinner => backupWinner.SweepEntry.SweepEntryId.Equals(winnerSweepEntryId)) ||
					WinnerIsABackupWinnerViaUserIdentifierType(winner, sweepEntry)
			);
		}

		private bool DrawnWinnerInWinnersList(List<SweepWinner> winnerList, List<SweepEntry> sweepEntries, int winnerSweepEntryId)
		{
			var sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));

			if (sweepEntry == null) return false;

			return winnerList.Any(
				c => c.SweepEntry.SweepEntryId.Equals(winnerSweepEntryId) ||
				(c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.Equals(sweepEntry.UserIdentifierType.ToString()))
			);
		}

		private static bool WinnerIsABackupWinnerViaUserIdentifierType(SweepWinner winner, SweepEntry sweepEntry)
			=> winner.BackupWinners.Any(backupWinner => backupWinner.UserIdentifier.Equals(sweepEntry.UserIdentifier) &&
					backupWinner.UserIdentifierType.Equals(sweepEntry.UserIdentifierType.ToString()));
		
	}
}

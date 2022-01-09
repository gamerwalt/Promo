using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promo.Core.Models;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
	public class DrawBackupWinnersForSweeps : IRequest<IEnumerable<SweepWinner>>
	{
		private int firstEntryId;
		private int lastEntryId;
		private List<int> missingBetween;
		private List<SweepEntry> sweepEntries;
		private SweepDrawRule sweepDrawRule;
		private IEnumerable<SweepWinner> winnerList;

		public DrawBackupWinnersForSweeps(int firstEntryId, int lastEntryId, List<int> missingBetween, List<SweepEntry> sweepEntries, SweepDrawRule sweepDrawRule, IEnumerable<SweepWinner> winnerList)
		{
			this.firstEntryId = firstEntryId;
			this.lastEntryId = lastEntryId;
			this.missingBetween = missingBetween;
			this.sweepEntries = sweepEntries;
			this.sweepDrawRule = sweepDrawRule;
			this.winnerList = winnerList;
		}

		public int FirstEntryId
		{
			get { return firstEntryId; }
		}

		public int LastEntryId
		{
			get { return lastEntryId; }
		}

		public List<int> MissingBetween
		{
			get { return missingBetween; }
		}

		public List<SweepEntry> SweepEntries
		{
			get { return sweepEntries; }
		}

		public SweepDrawRule SweepDrawRule
		{
			get { return sweepDrawRule; }
		}

		public IEnumerable<SweepWinner> WinnerList
		{
			get { return winnerList; }
		}
	}

	public class DrawBackupWinnersForSweepsHandler : IRequestHandler<DrawBackupWinnersForSweeps, IEnumerable<SweepWinner>>
	{
		private readonly DrawSettingOptions _drawSettingOptions;
		private readonly ILogger<DrawBackupWinnersForSweepsHandler> _logger;

		public DrawBackupWinnersForSweepsHandler(IOptions<DrawSettingOptions> options, ILoggerFactory loggerFactory)
		{
			_drawSettingOptions = options.Value;
			_logger = loggerFactory.CreateLogger<DrawBackupWinnersForSweepsHandler>();
		}

		public async Task<IEnumerable<SweepWinner>> Handle(DrawBackupWinnersForSweeps request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {handlerName} for Sweep: {sweepName} and Campaign: {campaignName}",
				nameof(DrawWinnersForSweepsHandler),
				request.SweepDrawRule.SweepName,
				request.SweepDrawRule.CampaignName);

			foreach (var winner in request.WinnerList)
			{
				for (var backupWinnerIndex = 1; backupWinnerIndex <= request.SweepDrawRule.WinnerBackupCount; backupWinnerIndex++)
				{
					_logger.LogInformation("Drawing backup winner {backupWinnerIndex} for Winner-SweepEntryUid {winnerSweepEntryUid} for Sweep {}", backupWinnerIndex, winner.SweepEntryUid, request.SweepDrawRule.SweepName);

					var drawWinnerCounter = 0;
					var backupWinnerSweepEntryId = 0;
					var sweepEntry = default(SweepEntry);

					while (drawWinnerCounter < _drawSettingOptions.NumberOfTriesSelectingADistinctWinner)
					{
						backupWinnerSweepEntryId = GetRamdonNumber(request.FirstEntryId, request.LastEntryId, request.MissingBetween, request.SweepEntries);

						if (!DrawnWinnerInWinnersAndBackupList(request.WinnerList.ToList(), request.SweepEntries, backupWinnerSweepEntryId))
						{
							sweepEntry = request.SweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(backupWinnerSweepEntryId));
							if (sweepEntry != null)
								break;
						}

						drawWinnerCounter++;
					}

					_logger.LogInformation("Backup Winner for {backupWinnerIndex} drawn. SweepEntryUid: {sweepEntryUid} Picked", backupWinnerIndex, sweepEntry.SweepEntryUid);

					request.MissingBetween.Add(backupWinnerSweepEntryId);

					winner.BackupWinners.Add(new SweepWinner
					{
						SweepEntryId = backupWinnerSweepEntryId,
						SweepEntryUid = sweepEntry.SweepEntryUid,
						UserIdentifier = sweepEntry.UserIdentifier,
						UserIdentifierType = sweepEntry.UserIdentifierType.ToString()
					});

					request.SweepEntries.RemoveAll(c => c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.Equals(sweepEntry.UserIdentifierType));
				}
			}

			_logger.LogInformation("Finished handling {handlerName} for Sweep: {sweepName} and Campaign: {campaignName}",
				nameof(DrawWinnersForSweepsHandler),
				request.SweepDrawRule.SweepName,
				request.SweepDrawRule.CampaignName);

			return request.WinnerList;
		}

		private bool DrawnWinnerInWinnersAndBackupList(List<SweepWinner> winnerList, List<Infrastructure.Models.SweepEntry> sweepEntries, int winnerSweepEntryId)
		{
			var sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));

			var inWinnerList = DrawnWinnerInWinnersList(winnerList, sweepEntries, winnerSweepEntryId);

			return inWinnerList &&
				winnerList.Any(
					winner => winner.BackupWinners.Any(backupWinner => backupWinner.SweepEntryId.Equals(winnerSweepEntryId)) ||
					WinnerIsABackupWinnerViaUserIdentifierType(winner, sweepEntry)
			);
		}

		private bool DrawnWinnerInWinnersList(List<SweepWinner> winnerList, List<Infrastructure.Models.SweepEntry> sweepEntries, int winnerSweepEntryId)
		{
			var sweepEntry = sweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));

			if (sweepEntry == null) return false;

			return winnerList.Any(
				c => c.SweepEntryId.Equals(winnerSweepEntryId) ||
				(c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.Equals(sweepEntry.UserIdentifierType.ToString()))
			);
		}

		private static bool WinnerIsABackupWinnerViaUserIdentifierType(SweepWinner winner, Infrastructure.Models.SweepEntry sweepEntry)
		{
			return winner.BackupWinners.Any(backupWinner => backupWinner.UserIdentifier.Equals(sweepEntry.UserIdentifier) &&
									backupWinner.UserIdentifierType.Equals(sweepEntry.UserIdentifierType.ToString()));
		}

		private int GetRamdonNumber(int min, int max, List<int> missingBetween, List<Infrastructure.Models.SweepEntry> sweepEntries)
		{
			if (min >= max) throw new ArgumentOutOfRangeException();

			var result = missingBetween.First();
			while (missingBetween.Contains(result) && !sweepEntries.Any(s => s.SweepEntryId.Equals(result)))
			{
				result = RandomNumberGenerator.GetInt32(min, max + 1);
			}

			return result;
		}
	}
}

using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promo.Core.Models;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
	public class DrawWinnersForSweeps : IRequest<IEnumerable<SweepWinner>>
	{
		private int firstEntryId;
		private int lastEntryId;
		private List<int> missingBetween;
		private List<SweepEntry> sweepEntries;
		private SweepDrawRule sweepDrawRule;

		public DrawWinnersForSweeps(int firstEntryId, int lastEntryId, List<int> missingBetween, List<SweepEntry> sweepEntries, SweepDrawRule sweepDrawRule)
		{
			this.firstEntryId = firstEntryId;
			this.lastEntryId = lastEntryId;
			this.missingBetween = missingBetween;
			this.sweepEntries = sweepEntries;
			this.sweepDrawRule = sweepDrawRule;
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
	}

	public class DrawWinnersForSweepsHandler : IRequestHandler<DrawWinnersForSweeps, IEnumerable<SweepWinner>>
	{
		private readonly DrawSettingOptions _drawSettingOptions;
		private readonly ILogger<DrawWinnersForSweepsHandler> _logger;

		public DrawWinnersForSweepsHandler(IOptions<DrawSettingOptions> options, ILoggerFactory loggerFactory)
		{
			_drawSettingOptions = options.Value;
			_logger = loggerFactory.CreateLogger<DrawWinnersForSweepsHandler>();
		}

		public async Task<IEnumerable<SweepWinner>> Handle(DrawWinnersForSweeps request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {handlerName} for Sweep: {sweepName} and Campaign: {campaignName}",
				nameof(DrawWinnersForSweepsHandler),
				request.SweepDrawRule.SweepName,
				request.SweepDrawRule.CampaignName);

			var winnerList = new List<SweepWinner>();

			for (var winnerIndex = 1; winnerIndex <= request.SweepDrawRule.WinnerCount; winnerIndex++)
			{
				_logger.LogInformation("Drawing winner {winnerIndex} for Sweep {sweepName}", winnerIndex, request.SweepDrawRule.SweepName);

				var drawWinnerCounter = 0;
				var winnerSweepEntryId = 0;
				var sweepEntry = default(SweepEntry);

				while (drawWinnerCounter < _drawSettingOptions.NumberOfTriesSelectingADistinctWinner)
				{
					winnerSweepEntryId = GetRamdonNumber(request.FirstEntryId, request.LastEntryId, request.MissingBetween, request.SweepEntries);

					if (!DrawnWinnerInWinnersList(winnerList, request.SweepEntries, winnerSweepEntryId))
					{
						sweepEntry = request.SweepEntries.FirstOrDefault(c => c.SweepEntryId.Equals(winnerSweepEntryId));
						if (sweepEntry != null)
							break;
					}

					drawWinnerCounter++;
				}

				_logger.LogInformation("Winner for {winnerIndex} drawn. SweepEntryUid: {sweepEntryUid} Picked", winnerIndex, sweepEntry.SweepEntryUid);

				request.MissingBetween.Add(winnerSweepEntryId);

				_logger.LogInformation("Winner picked for {sweepName} is {userIdentifier}.", request.SweepDrawRule.SweepName, sweepEntry.UserIdentifier);

				winnerList.Add(new SweepWinner
				{
					SweepWinnerId = winnerIndex,
					SweepEntryId = winnerSweepEntryId,
					SweepEntryUid = sweepEntry.SweepEntryUid,
					UserIdentifier = sweepEntry.UserIdentifier,
					UserIdentifierType = sweepEntry.UserIdentifierType.ToString()
				});

				request.SweepEntries.RemoveAll(c => c.UserIdentifier.Equals(sweepEntry.UserIdentifier) && c.UserIdentifierType.Equals(sweepEntry.UserIdentifierType));
			}

			_logger.LogInformation("Finished handling {handlerName} for Sweep: {sweepName} and Campaign: {campaignName}",
				nameof(DrawWinnersForSweepsHandler),
				request.SweepDrawRule.SweepName,
				request.SweepDrawRule.CampaignName);

			return winnerList;
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

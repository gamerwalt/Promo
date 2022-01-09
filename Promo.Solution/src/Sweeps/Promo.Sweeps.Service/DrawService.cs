using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promo.Core;
using Promo.Core.Models;
using Promo.Sweeps.Core;
using Promo.Sweeps.Core.RequestHandlers;
using Promo.Sweeps.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Draw.Service
{
	public interface IDrawService
	{
		Task DrawAvailableSweepsAsync(CancellationToken token);
	}

	public class DrawService : IDrawService
	{
		private readonly IMediator _mediator;
		private readonly DrawSettingOptions _drawSettingOptions;
		private readonly ILogger<DrawService> _logger;

		public DrawService(
			IMediator mediator,
			IOptions<DrawSettingOptions> options,
			ILoggerFactory loggerFactory)
		{
			_mediator = mediator;
			_drawSettingOptions = options.Value;
			_logger = loggerFactory.CreateLogger<DrawService>();
		}

		public async Task DrawAvailableSweepsAsync(CancellationToken token)
		{
			//1. Get the sweeps and the draw rules that have a draw now or that has passed and has not been drawn
			var sweepDrawRules = await _mediator.Send(new GetSweepDrawRules(), token);

			//2. Perform a draw on each of the entries for the sweeps
			var drawTasks = new List<Task>();

			foreach (var sweepDrawRule in sweepDrawRules)
			{
				//perform draw and save the winners and backups
				//drawTasks.Add(PerformDrawAsync(sweepDrawRule));
				await PerformDrawAsync(sweepDrawRule);
			}

			await Task.WhenAll(drawTasks);
		}

		private async Task PerformDrawAsync(SweepDrawRule sweepDrawRule)
		{
			var entries = await _mediator.Send(new GetSweepEntries(sweepDrawRule.SweepUid));

			var distinctUsers = from e in entries
								group e by new
								{
									e.UserIdentifier,
									e.UserIdentifierType
								} into distincts
								select new SweepEntryUsers
								{
									UserIdentifier = distincts.Key.UserIdentifier,
									UserIdentifierType = distincts.Key.UserIdentifierType
								};

			if (distinctUsers.Count() <= (sweepDrawRule.WinnerCount + sweepDrawRule.WinnerBackupCount))
			{
				_logger.LogInformation($"Rule: Winner Count: {sweepDrawRule.WinnerCount} + Winner Backup Count: {sweepDrawRule.WinnerBackupCount} is more than the distinct number of users {distinctUsers.Count()} who entered the promo");
				_logger.LogInformation($"Cannot continue. Please perform this draw manually or update the Winner count and back up winner count for Sweep: {sweepDrawRule.SweepName} under Campaign: {sweepDrawRule.CampaignName}");
			}
			else
			{
				await PerformActualDrawAsync(sweepDrawRule, entries);
			}
		}

		private async Task PerformActualDrawAsync(SweepDrawRule sweepDrawRule, IEnumerable<SweepEntry> entries)
		{
			var entriesOrdered = entries.OrderBy(c => c.SweepEntryId).ToList();
			var firstEntryId = entriesOrdered.First().SweepEntryId;
			var lastEntryId = entriesOrdered.Last().SweepEntryId;
			var missingBetween = GetMissingNumbers(entriesOrdered, firstEntryId, lastEntryId);

			var winners = await DoDraw(firstEntryId, lastEntryId, missingBetween, entries.ToList(), sweepDrawRule);
			_logger.LogInformation("Winners determined for Sweep: {sweepName} and Campaign: {campaignName}", sweepDrawRule.SweepName, sweepDrawRule.CampaignName);

			//Save Winners
			await _mediator.Send(new SaveSweepDrawWinners(winners, sweepDrawRule));
		}

		private async Task<IEnumerable<SweepWinner>> DoDraw(int firstEntryId,
			int lastEntryId,
			List<int> missingBetween,
			List<SweepEntry> sweepEntries,
			SweepDrawRule sweepDrawRule)
		{
			var winnerList = await _mediator.Send(new DrawWinnersForSweeps(firstEntryId, lastEntryId, missingBetween, sweepEntries, sweepDrawRule));
			foreach (var winner in winnerList)
			{
				sweepEntries.RemoveAll(c => c.UserIdentifier.Equals(winner.UserIdentifier) && c.UserIdentifierType.ToString().Equals(winner.UserIdentifierType));
			}

			winnerList = await _mediator.Send(new DrawBackupWinnersForSweeps(firstEntryId, lastEntryId, missingBetween, sweepEntries, sweepDrawRule, winnerList));

			return winnerList;
		}

		private List<int> GetMissingNumbers(List<Infrastructure.Models.SweepEntry> entries, int firstEntryId, int lastEntryId)
		{
			var missing = new List<int>();

			for (var i = firstEntryId; i < lastEntryId + 1; i++)
			{
				if (!entries.Any(c => c.SweepEntryId.Equals(i)))
				{
					missing.Add(i);
				}
			}

			return missing;
		}
	}

	public class SweepEntryUsers
	{
		public string UserIdentifier { get; internal set; }
		public UserIdentifierType UserIdentifierType { get; internal set; }
	}
}

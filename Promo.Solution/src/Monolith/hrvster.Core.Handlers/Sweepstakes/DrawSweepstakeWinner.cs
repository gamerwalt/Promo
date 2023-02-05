using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.Sweepstakes;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class DrawSweepstakeWinner : AsyncRequestHandler<DrawSweepstakesWinnerRequest>
	{
		private readonly ILogger<DrawSweepstakeWinner> _logger;
		private readonly IMediator _mediator;
		private readonly IRandomizer _randomizer;
		private readonly ISweepWinnersRepository _sweepWinnersRepository;
		private readonly ISweepsRepository _sweepsRepository;

		public DrawSweepstakeWinner(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IRandomizer randomizer,
			ISweepWinnersRepository sweepWinnersRepository,
			ISweepsRepository sweepsRepository)
		{
			_logger = loggerFactory.CreateLogger<DrawSweepstakeWinner>();
			_mediator = mediator;
			_randomizer = randomizer;
			_sweepWinnersRepository = sweepWinnersRepository;
			_sweepsRepository = sweepsRepository;
		}

		protected async override Task Handle(DrawSweepstakesWinnerRequest request, CancellationToken cancellationToken)
		{
			if (await HasNoDrawHappenedNotificationsAsync(request.Sweepstake))
			{
				var entries = await GetSweepEntriesAsync(request.Sweepstake);
				var drawRule = await GetSweepDrawRuleAsync(request.Sweepstake);

				await PerformDrawAsync(request.Sweepstake, entries, drawRule, request.DrawProperties);
			}
		}

		private async Task PerformDrawAsync(Sweep sweep, List<SweepEntry> entries, Models.WebModels.Sweeps.DrawRule drawRule, SweepstakesDrawDetails drawProperties)
		{
			if (await SatisfiesDrawCondition(sweep, entries, drawRule))
			{
				var distinctUsers = GetDistinctEntries(entries).ToList();
				var drawObject = GetDrawObject(distinctUsers, entries);

				var winners = DrawWinners(sweep, entries, drawRule, drawObject);
				DrawBackupWinners(winners, sweep, entries, drawRule, drawObject);

				_logger.LogInformation("Saving winners for sweep {sweepName} with uid {sweepUid}",
					sweep.Name,
					sweep.SweepUid);
				var index = 1;

				foreach (var winner in winners)
				{
					await _sweepWinnersRepository.SaveSweepWinnerAsync(winner);
					index = 1;
					foreach (var backupWinner in winner.BackupWinners)
					{
						await _sweepWinnersRepository.SaveSweepBackupWinnerAsync(new SweepWinner
						{
							SweepWinnerUid = winner.SweepWinnerUid,
							AssignedId = index,
							Sweep = sweep,
							SweepEntry = backupWinner.SweepEntry,
							UserIdentifier = backupWinner.UserIdentifier,
							UserIdentifierType = backupWinner.UserIdentifierType
						});
						index++;
					}
				}

				await _mediator.Publish(new WinnersSaved(winners, drawProperties));
			}
		}

		private void DrawBackupWinners(
			List<SweepWinner> winners,
			Sweep sweep,
			List<SweepEntry> entries,
			Models.WebModels.Sweeps.DrawRule drawRule,
			List<SweepstakeDrawObject> drawObject)
		{
			_logger.LogInformation("Drawing backup winners for sweepstake {sweepName} with uid {sweepUid}.",
				sweep.Name,
				sweep.SweepUid);

			var backups = new List<SweepBackupWinner>();

			try
			{
				for (var winnerIndex = 1; winnerIndex <= winners.Count; winnerIndex++)
				{
					var min = 1;
					var max = entries.Count;
					for (var index = 1; index <= drawRule.WinnerBackupCount; index++)
					{
						_logger.LogInformation("Drawing backup winner # {number} for winner {winnerNumber} for sweepstake {sweepName} with uid {sweepUid}",
							index,
							winnerIndex,
							sweep.Name,
							sweep.SweepUid);

						var backupWinnerIndex = 0;
						var backupWinnerDrawObject = default(SweepstakeDrawObject);

						if (drawObject.Count() < 0 || drawObject.Count() == 0)
						{
							_logger.LogWarning("There are no more entries to draw backup winners from for sweepstake {sweepName} with uid {sweepUid}",
								sweep.Name,
								sweep.SweepUid);
							_logger.LogWarning("Backup Draw object count {drawObjectCount}. Entry Count {entryCount}",
								drawObject.Count(),
								entries.Count());

							break;
						}
						else
						{
							backupWinnerIndex = GetRandomNumber(min, max);

							backupWinnerDrawObject = drawObject[backupWinnerIndex];
						}

						_logger.LogInformation("Backup winner # {number} for winner {winnerNumber} for sweepstake {sweepName} with uid {sweepUid} is {userIdentifier}.",
						index,
						winnerIndex,
						sweep.Name,
						sweep.SweepUid,
						backupWinnerDrawObject.UserIdentifier);

						var backupWinner = GetBackupWinner(sweep, entries, index, backupWinnerDrawObject);
						var winner = winners[winnerIndex - 1];
						backupWinner.SweepWinnerUid = winner.SweepWinnerUid;

						winner.BackupWinners.Add(backupWinner);

						max = RemoveBackupWInnerFromDrawObject(drawObject, max, backupWinnerDrawObject);
						RemoveFromEntries(backupWinnerDrawObject, entries);

						_logger.LogInformation("Finished drawing backup winner # {number} for winner {winnerNumber} for sweepstake {sweepName} with uid {sweepUid}",
							index,
							winnerIndex,
							sweep.Name,
							sweep.SweepUid);
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}			

			_logger.LogInformation("Finished drawing backup winners for sweepstake {sweepName} with uid {sweepUid}.",
				sweep.Name,
				sweep.SweepUid);
		}

		private int GetRandomNumber(int min, int max)
		{
			if (min != max)
				return _randomizer.GetRandomNumber(min, max) - 1;

			return 0;
		}

		private List<SweepWinner> DrawWinners(
			Sweep sweep,
			List<SweepEntry> entries,
			Models.WebModels.Sweeps.DrawRule drawRule,
			List<SweepstakeDrawObject> drawObject)
		{
			_logger.LogInformation("Drawing winners for sweepstake {sweepName}", sweep.Name);

			var winners = new List<SweepWinner>();
			var min = 1;
			var max = entries.Count;

			for (var index = 1; index <= drawRule.WinnerCount; index++)
			{
				_logger.LogInformation("Drawing winner # {number} for sweepstake {sweepName} with uid {sweepUid}",
					index,
					sweep.Name,
					sweep.SweepUid);

				var winnerIndex = 0;
				var winnerDrawObject = default(SweepstakeDrawObject);

				if (drawObject.Count() < 0 || drawObject.Count() == 0)
				{
					_logger.LogWarning("There are no more entries to draw winners from for sweepstake {sweepName} with uid {sweepUid}",
						sweep.Name,
						sweep.SweepUid);
					_logger.LogWarning("Winners Draw object count {drawObjectCount}. Entry Count {entryCount}",
						drawObject.Count(),
						entries.Count());

					break;
				}
				else
				{
					winnerIndex = GetRandomNumber(min, max);
					winnerDrawObject = drawObject[winnerIndex];
				}

				_logger.LogInformation("Winner # {number} for sweepstake {sweepName} with uid {sweepUid} is {userIdentifier}.",
					index,
					sweep.Name,
					sweep.SweepUid,
					winnerDrawObject.UserIdentifier);

				var winner = GetWinner(sweep, entries, index, winnerDrawObject);

				winners.Add(winner);

				max = RemoveWinnerFromDrawObject(drawObject, max, winnerDrawObject);
				RemoveFromEntries(winnerDrawObject, entries);
			}

			_logger.LogInformation("Finished drawing {count} winners for sweepstake {sweepName}", winners.Count, sweep.Name);

			return winners;
		}

		private void RemoveFromEntries(SweepstakeDrawObject drawObject, List<SweepEntry> entries)
		{
			var sweepEntryToRemove = entries.FirstOrDefault(c => c.UserIdentifier == drawObject.UserIdentifier);

			entries.Remove(sweepEntryToRemove);
		}

		private static int RemoveWinnerFromDrawObject(
			List<SweepstakeDrawObject> drawObject,
			int max,
			SweepstakeDrawObject winnerDrawObject)
		{
			drawObject.Remove(winnerDrawObject);

			return drawObject.Count;
		}

		private int RemoveBackupWInnerFromDrawObject(List<SweepstakeDrawObject> drawObject, int max, SweepstakeDrawObject backupWinnerDrawObject)
		{
			drawObject.Remove(backupWinnerDrawObject);

			return drawObject.Count;
		}

		private SweepBackupWinner GetBackupWinner(Sweep sweep, List<SweepEntry> entries, int index, SweepstakeDrawObject backupWinnerDrawObject)
		{
			var backupWinnerSweepEntry = entries.FirstOrDefault(e => e.SweepEntryUid == backupWinnerDrawObject.SweepEntryUid);

			var backupWinner = GetBackupWinnerSweepEntry(sweep, backupWinnerSweepEntry, index);

			return backupWinner;
		}

		private SweepBackupWinner GetBackupWinnerSweepEntry(Sweep sweep, SweepEntry backupWinnerSweepEntry, int index)
		{
			return new SweepBackupWinner
			{
				UserIdentifier = backupWinnerSweepEntry.UserIdentifier,
				UserIdentifierType = backupWinnerSweepEntry.UserIdentifierType,
				Sweep = sweep,
				SweepEntry = backupWinnerSweepEntry,
				Selected = false
			};
		}

		private SweepWinner GetWinner(Sweep sweep, List<SweepEntry> entries, int index, SweepstakeDrawObject winnerDrawObject)
		{
			var winnerSweepEntry = entries.FirstOrDefault(e => e.SweepEntryUid == winnerDrawObject.SweepEntryUid);

			var winner = GetWinnerFromWinnerSweepEntry(sweep, winnerSweepEntry, index);
			return winner;
		}

		private SweepWinner GetWinnerFromWinnerSweepEntry(Sweep sweep, SweepEntry winnerSweepEntry, int assignedId)
			=> new SweepWinner
			{
				UserIdentifier = winnerSweepEntry.UserIdentifier,
				UserIdentifierType = winnerSweepEntry.UserIdentifierType,
				AssignedId = assignedId,
				SweepWinnerUid = Guid.NewGuid(),
				Sweep = sweep,
				FulfilledInd = false,
				SweepEntry = winnerSweepEntry
			};

		private List<SweepstakeDrawObject> GetDrawObject(List<SweepEntryUser> distinctUsers, List<SweepEntry> entries)
		{
			var drawObjects = new List<SweepstakeDrawObject>();

			foreach (var user in distinctUsers.Select((value, index) => new { index, value }))
			{
				drawObjects.Add(new SweepstakeDrawObject
				{
					Index = user.index + 1,
					UserIdentifier = user.value.UserIdentifier,
					SweepEntryUid = entries.FirstOrDefault(e => e.UserIdentifier == user.value.UserIdentifier).SweepEntryUid
				});
			}

			return drawObjects;
		}

		private async Task<bool> SatisfiesDrawCondition(Sweep sweep, List<SweepEntry> entries, Models.WebModels.Sweeps.DrawRule drawRule)
		{
			var distinctUsers = GetDistinctEntries(entries);

			var distinctUserCount = distinctUsers.ToList().Count;
			var winnerAndBackupCount = drawRule.WinnerCount + drawRule.WinnerBackupCount;

			if (distinctUserCount < winnerAndBackupCount)
			{
				_logger.LogInformation($"Rule: Total winners count {winnerAndBackupCount} is more than the distinct number of users {distinctUserCount} who entered the sweepstakes.");
				_logger.LogInformation($"Cannot continue. Please perform this draw manually or update the Winner count and back up winner count for Sweep: {sweep.Name}.");

				if (sweep.Account == null)
					sweep.Account = await GetSweepAccountAsync(sweep);

				var account = await _mediator.Send(new GetAccountRequest(sweep.Account.AccountUid));

				await _mediator.Send(new SweepsDistinctUsersLessThanWinnerCountMailRequest(account.EmailAddress, sweep.Name));

				await _mediator.Send(new UpsertSweepNotificationRequest(sweep, SweepNotificationType.NotEnoughUniqueEntrants));

				return false;
			}

			return true;
		}

		private async Task<Models.Account> GetSweepAccountAsync(Sweep sweep)
		{
			var retrievedSweep = await _sweepsRepository.GetAsync(sweep.SweepUid);

			return retrievedSweep.Account;
		}

		private static IEnumerable<SweepEntryUser> GetDistinctEntries(List<SweepEntry> entries)
			=> from e in entries
			   group e by new
			   {
				   e.UserIdentifier,
				   e.UserIdentifierType
			   } into distincts
			   select new SweepEntryUser
			   {
				   UserIdentifier = distincts.Key.UserIdentifier,
				   UserIdentifierType = distincts.Key.UserIdentifierType
			   };

		private async Task<Models.WebModels.Sweeps.DrawRule> GetSweepDrawRuleAsync(Sweep sweepstake)
			=> await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepstake.SweepUid));

		private async Task<bool> HasNoDrawHappenedNotificationsAsync(Sweep sweepstake)
		{
			var sweepNotifications = await _mediator.Send(new GetSweepNotificationRequest(sweepstake));

			if (sweepNotifications != null && sweepNotifications.DrawHappened)
			{
				_logger.LogWarning("It looks like the sweepstake {sweepName}'s draw has happened.",
					sweepstake.Name);

				return false;
			}

			return true;
		}

		private async Task<List<SweepEntry>> GetSweepEntriesAsync(Sweep sweepstake)
		{
			_logger.LogInformation("Retrieving the sweep entries for sweepstake {sweepName} with uid {sweepUid}",
				sweepstake.Name,
				sweepstake.SweepUid);

			var entries = await _mediator.Send(new GetSweepstakeEntriesRequest(sweepstake));

			if (entries.Count() == 0)
			{
				_logger.LogWarning("Cannot continue with performing draw for the sweepstakes {sweepName}.",
					sweepstake.Name);

				return null;
			}

			return entries.ToList();
		}
	}
}

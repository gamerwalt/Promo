using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class SaveSweepWinner : IRequest<IEnumerable<SweepWinner>>
	{
		private List<Guid> sweepWinnerGuids;
		private Guid sweepUid;

		public SaveSweepWinner(List<Guid> sweepWinnerGuids, Guid sweepUid)
		{
			this.sweepWinnerGuids = sweepWinnerGuids;
			this.sweepUid = sweepUid;
		}

		public List<Guid> SweepWinnersGuids
		{
			get { return sweepWinnerGuids; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class SaveSweepWinnerRequestHandler : IRequestHandler<SaveSweepWinner, IEnumerable<SweepWinner>>
	{
		private readonly ISweepWinnersRepository _repository;

		public SaveSweepWinnerRequestHandler(ISweepWinnersRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<SweepWinner>> Handle(SaveSweepWinner request, CancellationToken cancellationToken)
		{
			var sweepWinners = new List<SweepWinner>();

			foreach (var guid in request.SweepWinnersGuids)
			{
				var winner = await _repository.GetSweepWinnerAsync(guid);

				var sweepWinnerUid = Guid.NewGuid();
				await _repository.SaveSweepWinnerAsync(new SweepWinner
				{
					SweepWinnerUid = sweepWinnerUid,
					SweepEntry = new SweepEntry
					{
						SweepEntryUid = winner.SweepEntry.SweepEntryUid
					},
					Sweep = new Sweep
					{
						SweepUid = request.SweepUid
					},
					UserIdentifier = winner.UserIdentifier,
					UserIdentifierType = winner.UserIdentifierType,
					AssignedId = winner.SweepWinnerId
				});

				foreach (var backupWinner in winner.BackupWinners)
				{
					await _repository.SaveSweepBackupWinnerAsync(new SweepWinner
					{
						SweepWinnerUid = sweepWinnerUid,
						SweepEntry = new SweepEntry
						{
							SweepEntryUid = backupWinner.SweepEntry.SweepEntryUid,
						},
						Sweep = new Sweep
						{
							SweepUid = request.SweepUid,
						},
						UserIdentifier = backupWinner.UserIdentifier,
						UserIdentifierType = backupWinner.UserIdentifierType,
						AssignedId = backupWinner.SweepWinnerId
					});
				}

				var backupWinners = await _repository.GetSweepBackupWinnersAsync(sweepWinnerUid);
				var sweepWinner = await _repository.GetSweepWinnerAsync(sweepWinnerUid);
				sweepWinner.BackupWinners = backupWinners.ToList();

				sweepWinners.Add(sweepWinner);
			}

			return sweepWinners;
		}
	}
}

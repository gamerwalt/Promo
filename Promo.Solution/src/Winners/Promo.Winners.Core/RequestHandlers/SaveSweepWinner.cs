using MediatR;
using Promo.Core;
using Promo.Core.Models;
using Promo.Winners.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Winners.Core.RequestHandlers
{
	public class SaveSweepWinner : IRequest<IEnumerable<Infrastructure.Models.SweepWinner>>
	{
		private List<SweepWinner> sweepWinners;
		private Guid sweepUid;

		public SaveSweepWinner(List<SweepWinner> sweepWinners, Guid sweepUid)
		{
			this.sweepWinners = sweepWinners;
			this.sweepUid = sweepUid;
		}

		public List<SweepWinner> SweepWinners
		{
			get { return sweepWinners; }
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class SaveSweepWinnerRequestHandler : IRequestHandler<SaveSweepWinner, IEnumerable<Infrastructure.Models.SweepWinner>>
	{
		private readonly ISweepWinnersRepository _repository;

		public SaveSweepWinnerRequestHandler(ISweepWinnersRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Infrastructure.Models.SweepWinner>> Handle(SaveSweepWinner request, CancellationToken cancellationToken)
		{
			var sweepWinners = new List<Infrastructure.Models.SweepWinner>();

			foreach(var winner in request.SweepWinners)
			{
				var sweepWinnerUid = Guid.NewGuid();
				await _repository.SaveSweepWinnerAsync(new Infrastructure.Models.SweepWinner
				{
					SweepWinnerUid = sweepWinnerUid,
					SweepEntryUid = winner.SweepEntryUid.GetValueOrDefault(),
					SweepUid = request.SweepUid,
					UserIdentifier = winner.UserIdentifier,
					UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), winner.UserIdentifierType),
					AssignedId = winner.SweepWinnerId
				});

				foreach(var backupWinner in winner.BackupWinners)
				{
					await _repository.SaveSweepBackupWinnerAsync(new Infrastructure.Models.SweepWinner
					{
						SweepWinnerUid = sweepWinnerUid,
						SweepEntryUid = backupWinner.SweepEntryUid.GetValueOrDefault(),
						SweepUid = request.SweepUid,
						UserIdentifier = backupWinner.UserIdentifier,
						UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), backupWinner.UserIdentifierType),
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

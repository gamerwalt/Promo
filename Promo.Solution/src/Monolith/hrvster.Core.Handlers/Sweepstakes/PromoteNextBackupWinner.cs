using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class PromoteNextBackupWinner : AsyncRequestHandler<PromoteNextBackupWinnerRequest>
	{
		private readonly ILogger<PromoteNextBackupWinner> _logger;
		private readonly ISweepWinnersRepository _repository;

		public PromoteNextBackupWinner(ILoggerFactory loggerFactory, ISweepWinnersRepository repository)
		{
			_logger = loggerFactory.CreateLogger<PromoteNextBackupWinner>();
			_repository = repository;
		}

		protected override async Task Handle(PromoteNextBackupWinnerRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Promoting next backup winner to be the new winner for the sweepstake {sweepName}.", request.Sweepstake.Name);

			var backupWinners = await GetBackupWinnersAsync(request);

			_logger.LogInformation("Retrieved backup winners for sweepstake {sweepName}", request.Sweepstake.Name);

			if (backupWinners == null || backupWinners.Count() == 0)
			{
				_logger.LogInformation("Looks like we don't have any backup winners for the sweepstake {sweepName}", request.Sweepstake.Name);

				return;
			}

			_logger.LogInformation("Getting our next backup winner for the sweepstake {sweepName}", request.Sweepstake.Name);
			
			var nextBackupWinner = GetNextBackupWinner(backupWinners);
			
			if(nextBackupWinner == null)
			{
				_logger.LogWarning("We retrieved our next backup winner for sweepstake {sweepName} but it's empty.",
					request.Sweepstake.Name);

				return;
			}

			_logger.LogInformation("Our next backup winner is {userIdentifier} for the sweepstake is {sweepName}",
				nextBackupWinner.UserIdentifier,
				request.Sweepstake.Name);

			//4. Once we have who to promote, save the previous user with this SweepWinnerUid into the new sweep_winners_unfulfilled table
			await _repository.CreateUnfulfilledWinnerAsync(request.Winner.SweepWinnerUid, request.Reason, cancellationToken);

			//5. update the sweep_winners table to the backup
			await _repository.PromoteBackupWinnerAsync(
				request.Winner.SweepWinnerUid, 
				nextBackupWinner.SweepEntry.SweepEntryUid, 
				nextBackupWinner.UserIdentifier,
				nextBackupWinner.UserIdentifierType.ToString(),
				cancellationToken);

			//6. Update the backup's selected_ind
			await _repository.SetBackupAsSelectedAsync(nextBackupWinner.SweepWinnerBackupId, cancellationToken);
		}

		private static SweepBackupWinner GetNextBackupWinner(IEnumerable<SweepBackupWinner> backupWinners)
		{
			var orderedBackupWinners = backupWinners.OrderBy(c => c.SweepWinnerBackupId).ToList();

			var selectedExists = orderedBackupWinners.Any(c => c.Selected);

			var nextBackupWinner = default(SweepBackupWinner);

			if (!selectedExists)
				nextBackupWinner = orderedBackupWinners.FirstOrDefault();
			else
				nextBackupWinner = orderedBackupWinners.SkipWhile(o => o.Selected).FirstOrDefault();

			if(nextBackupWinner == null && !selectedExists)
			{
				return orderedBackupWinners.FirstOrDefault();
			}

			if (nextBackupWinner == null) return null;

			return nextBackupWinner;
		}

		private async Task<IEnumerable<SweepBackupWinner>> GetBackupWinnersAsync(PromoteNextBackupWinnerRequest request)
		{
			_logger.LogInformation("Retrieving backup winners for sweepstake {sweepName}", request.Sweepstake.Name);

			return await _repository.GetSweepBackupWinnersAsync(request.Winner.SweepWinnerUid);
		}
	}
}

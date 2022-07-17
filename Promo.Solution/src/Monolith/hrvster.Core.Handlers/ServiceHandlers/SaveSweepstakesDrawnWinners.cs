using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class SaveSweepstakesDrawnWinners : AsyncRequestHandler<SaveSweepstakesDrawnWinnersRequest>
	{
		private readonly ILogger<SaveSweepstakesDrawnWinners> _logger;
		private ISweepWinnersRepository _repository;
		private IMediator _mediator;

		public SaveSweepstakesDrawnWinners(
			ILoggerFactory loggerFactory,
			ISweepWinnersRepository repository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SaveSweepstakesDrawnWinners>();
			_repository = repository;
			_mediator = mediator;
		}

		protected override async Task Handle(SaveSweepstakesDrawnWinnersRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Saving the winners for the sweepstake {sweepName} and campaign {campaignName}. Content: {content}",
				request.DrawProperties.SweepDrawPropertiesAndRule.Sweep.Name,
				request.DrawProperties.SweepDrawPropertiesAndRule.Campaign.CampaignName,
				request.ToJsonString());

			var sweep = request.DrawProperties.SweepDrawPropertiesAndRule.Sweep;
			var campaign = request.DrawProperties.SweepDrawPropertiesAndRule.Campaign;

			foreach (var winner in request.Winners)
			{
				var winnerUid = Guid.NewGuid();

				await _repository.SaveSweepWinnerAsync(new SweepWinner
				{
					SweepWinnerUid = winnerUid,
					SweepEntry = winner.SweepEntry,
					Sweep = sweep,
					UserIdentifier = winner.UserIdentifier,
					UserIdentifierType = winner.UserIdentifierType,
					AssignedId = winner.SweepWinnerId
				});

				foreach (var backupWinner in winner.BackupWinners)
				{
					await _repository.SaveSweepBackupWinnerAsync(new SweepWinner
					{
						SweepWinnerUid = winnerUid,
						SweepEntry = backupWinner.SweepEntry,
						Sweep = sweep,
						UserIdentifier = backupWinner.UserIdentifier,
						UserIdentifierType = backupWinner.UserIdentifierType,
						AssignedId = backupWinner.SweepWinnerId
					});
				}
			}
		}
	}
}

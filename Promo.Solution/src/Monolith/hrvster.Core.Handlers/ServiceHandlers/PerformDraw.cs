using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.Sweepstakes;
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

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class PerformDraw : AsyncRequestHandler<PerformDrawRequest>
	{
		private readonly ILogger<PerformDraw> _logger;
		private readonly IMediator _mediator;
		private readonly ISweepsRepository _sweepsRepository;

		public PerformDraw(ILoggerFactory loggerFactory, IMediator mediator, ISweepsRepository sweepsRepository)
		{
			_logger = loggerFactory.CreateLogger<PerformDraw>();
			_mediator = mediator;
			_sweepsRepository = sweepsRepository;
		}

		protected override async Task Handle(PerformDrawRequest request, CancellationToken cancellationToken)
		{
			if (await SatisfiesPreDrawConditions(request.DrawProperties.SweepEntries, request.DrawProperties.SweepDrawPropertiesAndRule))
			{
				var winners = await _mediator.Send(new DrawWinnersForSweepstakesRequest(request.DrawProperties));

				foreach(var winner in winners)
				{
					RemoveWinnerFromSweepstakesEntries(request.DrawProperties.SweepEntries, winner);
				}

				winners = await _mediator.Send(new DrawBackupWinnersForSweepstakesRequest(request.DrawProperties, winners));

				await _mediator.Send(new SaveSweepstakesDrawnWinnersRequest(winners, request.DrawProperties));
				
				await _mediator.Publish(new WinnersSaved(winners, request.DrawProperties));

				await _sweepsRepository.UpdateSweepAsDrawnAsync(request.DrawProperties.SweepDrawPropertiesAndRule.Sweep.SweepUid);
			}
		}

		private void RemoveWinnerFromSweepstakesEntries(List<SweepEntry> sweepEntries, SweepWinner winner)
		{
			sweepEntries.RemoveAll(c => c.UserIdentifier.Equals(winner.UserIdentifier) &&
				c.UserIdentifierType.ToString().Equals(winner.UserIdentifierType.ToString()));
		}

		private async Task<bool> SatisfiesPreDrawConditions(IEnumerable<SweepEntry> sweepEntries, SweepDrawPropertiesAndRule rule)
		{
			var distinctUsers = from e in sweepEntries
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

			var distinctUserCount = distinctUsers.ToList().Count;
			var winnerAndBackupCount = rule.DrawRule.WinnerCount + rule.DrawRule.WinnerBackupCount;

			if (distinctUserCount < winnerAndBackupCount)
			{
				_logger.LogInformation($"Rule: Total winners count {winnerAndBackupCount} is more than the distinct number of users {distinctUserCount} who entered the sweepstakes.");
				_logger.LogInformation($"Cannot continue. Please perform this draw manually or update the Winner count and back up winner count for Sweep: {rule.Sweep.Name} under Campaign: {rule.Campaign.CampaignName}");

				await _mediator.Send(new SweepsDistinctUsersLessThanWinnerCountMailRequest(rule.Account.EmailAddress, rule.Sweep.Name));

				await _mediator.Send(new UpsertSweepNotificationRequest(rule.Sweep, SweepNotificationType.NotEnoughUniqueEntrants));

				return false;
			}

			return true;
		}
	}
}

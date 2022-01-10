using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Promo.Clients.Internal;
using Promo.Core.Models;
using Promo.Sweeps.Core.Notifications;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
	public class SaveSweepDrawWinners : IRequest
	{
		private IEnumerable<SweepWinner> winners;
		private SweepDrawRule sweepDrawRule;

		public SaveSweepDrawWinners(IEnumerable<SweepWinner> winners, SweepDrawRule sweepDrawRule)
		{
			this.winners = winners;
			this.sweepDrawRule = sweepDrawRule;
		}

		public IEnumerable<SweepWinner> Winners
		{
			get { return winners; }
		}

		public SweepDrawRule SweepDrawRule
		{
			get { return sweepDrawRule; }
		}
	}

	public class SaveSweepDrawWinnersHandler : AsyncRequestHandler<SaveSweepDrawWinners>
	{
		private readonly ILogger<SaveSweepDrawWinnersHandler> _logger;
		private readonly IMediator _mediator;
		private readonly IWinnersClient _winnersClient;

		public SaveSweepDrawWinnersHandler(ILoggerFactory loggerFactory, IMediator mediator, IWinnersClient winnersClient)
		{
			_logger = loggerFactory.CreateLogger<SaveSweepDrawWinnersHandler>();
			_mediator = mediator;
			_winnersClient = winnersClient;
		}

		protected async override Task Handle(SaveSweepDrawWinners request, CancellationToken cancellationToken)
		{
			if(await _winnersClient.SaveSweepWinnersAsync(request.Winners, request.SweepDrawRule.SweepUid))
			{
				_logger.LogInformation("Sweep winners for Sweep: {sweepName} and Campaign {campaignName} successfully saved.", request.SweepDrawRule.SweepName, 
					request.SweepDrawRule.CampaignName);

				await _mediator.Publish(new SweepDrawHappened(request.SweepDrawRule));

				await _mediator.Publish(new WinnerSaved(request.Winners, request.SweepDrawRule));
				
				return;
			}

			_logger.LogWarning("Unable to save winners for Sweep: {sweepName} and Campaign: {campaignName}. See attached Winners: {winners}",
				request.SweepDrawRule.SweepName,
				request.SweepDrawRule.CampaignName,
				JsonConvert.SerializeObject(request.Winners));

			await _mediator.Publish(new SaveWinnersFailed(request.Winners, request.SweepDrawRule.CampaignName, request.SweepDrawRule.SweepName));
		}
	}
}

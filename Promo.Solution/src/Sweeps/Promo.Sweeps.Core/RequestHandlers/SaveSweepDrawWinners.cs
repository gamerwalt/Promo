using MediatR;
using Microsoft.Extensions.Logging;
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

		public SaveSweepDrawWinnersHandler(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SaveSweepDrawWinnersHandler>();
			_mediator = mediator;
		}

		protected async override Task Handle(SaveSweepDrawWinners request, CancellationToken cancellationToken)
		{
			foreach(var winner in request.Winners)
			{

			}

			await _mediator.Publish(new WinnerSaved(request.Winners, request.SweepDrawRule));
		}
	}
}

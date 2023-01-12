using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class MarkWinnerAsFulfilled : AsyncRequestHandler<MarkWinnerAsFulfilledRequest>
	{
		private readonly ISweepWinnersRepository _repository;

		public MarkWinnerAsFulfilled(ISweepWinnersRepository repository)
		{ 
			_repository = repository;
		}

		protected override async Task Handle(MarkWinnerAsFulfilledRequest request, CancellationToken cancellationToken)
		{
			await _repository.FulfilledAsync(request.SweepWinner.SweepWinnerUid, request.SweepstakePrize.PrizeId);
		}
	}
}

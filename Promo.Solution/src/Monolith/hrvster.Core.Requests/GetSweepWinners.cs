using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetSweepWinners : IRequest<IEnumerable<SweepWinner>>
	{
		private Guid sweepUid;

		public GetSweepWinners(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class GetSweepWinnersRequestHandler : IRequestHandler<GetSweepWinners, IEnumerable<SweepWinner>>
	{
		private readonly ISweepWinnersRepository _repository;

		public GetSweepWinnersRequestHandler(ISweepWinnersRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<SweepWinner>> Handle(GetSweepWinners request, CancellationToken cancellationToken)
		{
			return await _repository.GetSweepWinnersAsync(request.SweepUid);
		}
	}
}

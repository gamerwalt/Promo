using MediatR;
using Promo.Winners.Infrastructure;
using Promo.Winners.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Winners.Core.RequestHandlers
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

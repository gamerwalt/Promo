using MediatR;
using Promo.Prizes.Infrastructure;
using Promo.Prizes.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Prizes.Core.RequestHandlers
{
	public class GetSweepPrizes : IRequest<IEnumerable<SweepPrize>>
	{
		private Guid sweepUid;

		public GetSweepPrizes(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class GetSweepPrizesRequestHandler : IRequestHandler<GetSweepPrizes, IEnumerable<SweepPrize>>
	{
		private readonly ISweepPrizesRepository _repository;

		public GetSweepPrizesRequestHandler(ISweepPrizesRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<SweepPrize>> Handle(GetSweepPrizes request, CancellationToken cancellationToken)
		{
			return await _repository.GetSweepPrizesAsync(request.SweepUid);
		}
	}
}

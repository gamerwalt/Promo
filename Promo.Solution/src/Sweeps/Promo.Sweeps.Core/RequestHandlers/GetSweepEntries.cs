using MediatR;
using Promo.Sweeps.Infrastructure;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
	public class GetSweepEntries : IRequest<IEnumerable<SweepEntry>>
	{
		private Guid sweepUid;

		public GetSweepEntries(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class GetSweepEntriesHandler : IRequestHandler<GetSweepEntries, IEnumerable<SweepEntry>>
	{
		private readonly ISweepsEntriesRepository _repository;

		public GetSweepEntriesHandler(ISweepsEntriesRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<SweepEntry>> Handle(GetSweepEntries request, CancellationToken cancellationToken)
		{
			return await _repository.GetSweepEntriesAsync(request.SweepUid);
		}
	}
}

using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetSweepByUid : IRequest<Sweep>
	{
		private Guid sweepUid;

		public GetSweepByUid(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class GetSweepByUidHandler : IRequestHandler<GetSweepByUid, Sweep>
	{
		private readonly ISweepsRepository _repository;

		public GetSweepByUidHandler(ISweepsRepository repository)
		{
			_repository = repository;
		}

		public async Task<Sweep> Handle(GetSweepByUid request, CancellationToken cancellationToken)
		{
			return await _repository.GetAsync(request.SweepUid);
		}
	}
}

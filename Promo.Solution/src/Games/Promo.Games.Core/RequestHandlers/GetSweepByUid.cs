using MediatR;
using Promo.Games.Infrastructure;
using Promo.Games.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Games.Core.RequestHandlers
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

using MediatR;
using Promo.Games.Core.Requests;
using Promo.Games.Infrastructure;
using Promo.Games.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Games.Core.RequestHandlers
{
    public class UpdateSweep : IRequest<Sweep>
    {
        private Guid sweepUid;
        private Sweep sweep;
        private UpdateSweepRequest request;

        public UpdateSweep(Guid sweepUid, Sweep sweep, UpdateSweepRequest request)
        {
            this.sweepUid = sweepUid;
            this.sweep = sweep;
            this.request = request;
        }

        public Guid SweepUid
        {
            get { return sweepUid; }
        }

        public Sweep Sweep
        {
            get { return sweep; }
        }

        public UpdateSweepRequest Request
        {
            get { return request; }
        }
    }

    public class UpdateSweepRequestHandler : IRequestHandler<UpdateSweep, Sweep>
    {
        private readonly ISweepsRepository _repository;

        public UpdateSweepRequestHandler(ISweepsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sweep> Handle(UpdateSweep request, CancellationToken cancellationToken)
        {
            request.Sweep.Name = request.Request.Name;
            request.Sweep.StartDate = request.Request.StartDate;
            request.Sweep.EndDate = request.Request.EndDate;
            request.Sweep.DrawDate = request.Request.DrawDate;

            await _repository.UpdateAsync(request.SweepUid, request.Sweep);

            return await _repository.GetAsync(request.SweepUid);
        }
    }
}

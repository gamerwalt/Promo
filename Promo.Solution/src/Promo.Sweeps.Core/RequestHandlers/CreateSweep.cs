using MediatR;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
    public class CreateSweep : IRequest<Sweep>
    {
        private NewSweepRequest request;

        public CreateSweep(NewSweepRequest request)
        {
            this.request = request;
        }

        public NewSweepRequest Request
        {
            get { return request; }
        }
    }

    public class CreateSweepRequestHandler : IRequestHandler<CreateSweep, Sweep>
    {
        private readonly ISweepsRepository _repository;

        public CreateSweepRequestHandler(ISweepsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sweep> Handle(CreateSweep request, CancellationToken cancellationToken)
        {
            var sweepUid = Guid.NewGuid();
            await _repository.CreateAsync(new Sweep
            {
                SweepUid = sweepUid,
                CampaignUid = request.Request.CampaignUid,
                Name = request.Request.Name,
                StartDate = request.Request.StartDate,
                EndDate = request.Request.EndDate
            });

            return await _repository.GetAsync(sweepUid);
        }
    }
}

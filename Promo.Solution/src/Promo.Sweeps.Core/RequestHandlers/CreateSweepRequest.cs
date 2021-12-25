using MediatR;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Sweeps.Core.RequestHandlers
{
    public class CreateSweepRequest : IRequest<Sweep>
    {
        private NewSweepRequest request;

        public CreateSweepRequest(NewSweepRequest request)
        {
            this.request = request;
        }

        public NewSweepRequest Request
        {
            get { return request; }
        }
    }

    public class CreateSweepRequestHandler : IRequestHandler<CreateSweepRequest, Sweep>
    {
        private readonly ISweepsRepository _repository;

        public CreateSweepRequestHandler(ISweepsRepository repository)
        {
            _repository = repository;
        }

        public async Task<Sweep> Handle(CreateSweepRequest request, CancellationToken cancellationToken)
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

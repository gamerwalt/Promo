using MediatR;
using Promo.Games.Core.Requests;
using Promo.Games.Infrastructure;
using Promo.Games.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Games.Core.RequestHandlers
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
                Campaign = new Campaign
				{
					CampaignUid = request.Request.CampaignUid
				},
                Name = request.Request.Name,
                StartDate = request.Request.StartDate,
                EndDate = request.Request.EndDate,
				DrawDate = request.Request.DrawDate
            });

            return await _repository.GetAsync(sweepUid);
        }
    }
}

using MediatR;
using Promo.Games.Infrastructure;
using Promo.Games.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Games.Core.RequestHandlers
{
    public class GetSweepsForCampaignUid : IRequest<IEnumerable<Sweep>>
    {
        private Guid campaignUid;

        public GetSweepsForCampaignUid(Guid campaignUid)
        {
            this.campaignUid = campaignUid;
        }

        public Guid CampaignUid
        {
            get { return campaignUid; }
        }
    }

    public class GetSweepsForCampaignUidHandler : IRequestHandler<GetSweepsForCampaignUid, IEnumerable<Sweep>>
    {
        private readonly ISweepsRepository _repository;

        public GetSweepsForCampaignUidHandler(ISweepsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Sweep>> Handle(GetSweepsForCampaignUid request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllForCampaignAsync(request.CampaignUid);
        }
    }
}

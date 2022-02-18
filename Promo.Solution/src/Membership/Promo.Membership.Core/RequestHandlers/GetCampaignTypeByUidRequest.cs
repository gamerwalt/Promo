using MediatR;
using Promo.Membership.Infrastructure;
using Promo.Membership.Infrastructure.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
    public class GetCampaignTypeByUidRequest : IRequest<CampaignType>
    {
        private Guid campaignTypeUid;

        public GetCampaignTypeByUidRequest(Guid campaignTypeUid)
        {
            this.campaignTypeUid = campaignTypeUid;
        }

        public Guid CampaignTypeUid
        {
            get { return campaignTypeUid; }
        }
    }

    public class GetCampaignTypeByUidRequestHandler : IRequestHandler<GetCampaignTypeByUidRequest, CampaignType>
    {
        private readonly ICampaignTypesRepository _repository;
        public GetCampaignTypeByUidRequestHandler(ICampaignTypesRepository repository)
        {
            _repository = repository;
        }

        public async Task<CampaignType> Handle(GetCampaignTypeByUidRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetCampaignTypeByUidAsync(request.CampaignTypeUid);
        }
    }
}

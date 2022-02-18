using MediatR;
using Promo.Membership.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
    public class GetCampaignByUidRequest : IRequest<Infrastructure.Models.Campaign>
    {
        private Guid campaignUid;

        public GetCampaignByUidRequest(Guid campaignUid)
        {
            this.campaignUid = campaignUid;
        }

        public Guid CampaignUid
        {
            get { return campaignUid; }
        }
    }

    public class GetCampaignByUidRequestHandler : IRequestHandler<GetCampaignByUidRequest, Infrastructure.Models.Campaign>
    {
        private readonly ICampaignRepository _repository;

        public GetCampaignByUidRequestHandler(ICampaignRepository repository)
        {
            _repository = repository;
        }

        public async Task<Infrastructure.Models.Campaign> Handle(GetCampaignByUidRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetCampaignByUidAsync(request.CampaignUid);
        }
    }
}

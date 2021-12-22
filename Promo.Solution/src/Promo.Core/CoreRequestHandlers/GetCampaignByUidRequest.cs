using MediatR;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Models.Campaigns;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.CoreRequestHandlers
{
    public class GetCampaignByUidRequest : IRequest<Campaign>
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

    public class GetCampaignByUidRequestHandler : IRequestHandler<GetCampaignByUidRequest, Campaign>
    {
        private readonly ICampaignRepository _repository;

        public GetCampaignByUidRequestHandler(ICampaignRepository repository)
        {
            _repository = repository;
        }

        public async Task<Campaign> Handle(GetCampaignByUidRequest request, CancellationToken cancellationToken)
        {
            return await _repository.GetCampaignByUidAsync(request.CampaignUid);
        }
    }
}

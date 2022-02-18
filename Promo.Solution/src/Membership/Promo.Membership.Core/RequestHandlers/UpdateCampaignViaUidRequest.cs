using MediatR;
using Promo.Membership.Core.Notifications;
using Promo.Membership.Core.Requests;
using Promo.Membership.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
    public class UpdateCampaignViaUidRequest : IRequest<Infrastructure.Models.Campaign>
    {
        private Guid _campaignUid;
        private UpdateCampaignRequest _request;

        public UpdateCampaignViaUidRequest(Guid campaignUid, UpdateCampaignRequest request)
        {
            _campaignUid = campaignUid;
            _request = request;
        }

        public Guid CampaignUid
        {
            get { return _campaignUid; }
        }

        public UpdateCampaignRequest Request
        {
            get { return _request; }
        }
    }

    public class UpdateCampaignViaUidRequestHandler : IRequestHandler<UpdateCampaignViaUidRequest, Infrastructure.Models.Campaign>
    {
        private ICampaignRepository _repository;
        private IMediator _mediator;

        public UpdateCampaignViaUidRequestHandler(ICampaignRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Infrastructure.Models.Campaign> Handle(UpdateCampaignViaUidRequest request, CancellationToken cancellationToken)
        {
            await _repository.UpdateCampaignAsync(request.CampaignUid, new Infrastructure.Models.Campaign
            {
                CampaignName = request.Request.CampaignName,
                Description = request.Request.Description,
                StartDate = request.Request.StartDate,
                EndDate = request.Request.EndDate,
                ActiveInd = request.Request.ActiveInd,
                CampaignType = new Infrastructure.Models.CampaignType
                {
                    CampaignTypeUid = request.Request.CampaignTypeUid
                }
            });

            var campaign = await _repository.GetCampaignByUidAsync(request.CampaignUid);

            await _mediator.Publish(new CampaignUpdated(request.CampaignUid));

            return campaign;
        }
    }
}

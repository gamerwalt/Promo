using MediatR;
using Promo.Campaign.Core.Notifications;
using Promo.Campaign.Core.Requests;
using Promo.Campaign.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Campaign.Core.RequestHandlers
{
    public class UpdateCampaignViaUidRequest : IRequest
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

    public class UpdateCampaignViaUidRequestHandler : AsyncRequestHandler<UpdateCampaignViaUidRequest>
    {
        private ICampaignRepository _repository;
        private IMediator _mediator;

        public UpdateCampaignViaUidRequestHandler(ICampaignRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        protected async override Task Handle(UpdateCampaignViaUidRequest request, CancellationToken cancellationToken)
        {
            await _repository.UpdateCampaignAsync(request.CampaignUid, new Infrastructure.Models.Campaign
            {
                CampaignName = request.Request.CampaignName,
                Description = request.Request.Description,
                StartDate = request.Request.StartDate,
                EndDate = request.Request.EndDate,
                ActiveInd = request.Request.ActiveInd
            });

            await _mediator.Publish(new CampaignUpdated(request.CampaignUid));
        }
    }
}

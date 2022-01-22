using MediatR;
using Promo.Membership.Core.Notifications;
using Promo.Membership.Core.Requests;
using Promo.Membership.Infrastructure;
using Promo.Membership.Infrastructure.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
    public class CreateCampaignRequest : IRequest<Guid>
    {
        private NewCampaignRequest request;
        private ClaimsPrincipal user;

        public CreateCampaignRequest(NewCampaignRequest request, ClaimsPrincipal user)
        {
            this.request = request;
            this.user = user;
        }

        public NewCampaignRequest NewCampaign
        {
            get { return request; }
        }

        public Guid AccountUid
        {
            get { return Guid.Parse(user.Claims.FirstOrDefault(c => c.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase)).Value); }
        }
    }

    public class CreateCampaignRequestHandler : IRequestHandler<CreateCampaignRequest, Guid>
    {
        private readonly ICampaignRepository _repository;
        private readonly IMediator _mediator;
        public CreateCampaignRequestHandler(ICampaignRepository repository, IMediator mediator)
        {
            _repository = repository;
            _mediator = mediator;
        }

        public async Task<Guid> Handle(CreateCampaignRequest request, CancellationToken cancellationToken)
        {
            var campaignUid = Guid.NewGuid();

            await _repository.CreateCampaignAsync(new Infrastructure.Models.Campaign
            {
                CampaignUid = campaignUid,
                CampaignName = request.NewCampaign.Name,
                Description = request.NewCampaign.Description,
                StartDate = request.NewCampaign.StartDate,
                EndDate = request.NewCampaign.EndDate,
                CampaignType = new CampaignType
                {
                    CampaignTypeUid = request.NewCampaign.CampaignTypeUid
                },
                AccountUid = request.AccountUid
            });

            await _mediator.Publish(new CampaignCreated(request.NewCampaign, campaignUid));

            return campaignUid;
        }
    }
}

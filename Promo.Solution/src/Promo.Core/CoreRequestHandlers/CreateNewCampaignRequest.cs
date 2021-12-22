using MediatR;
using Promo.Core.ApiRequests;
using Promo.Core.CoreRequestNotifications;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Models.Campaigns;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Core.CoreRequestHandlers
{
    public class CreateCampaignRequest : IRequest<Guid>
    {
        private ApiRequests.CreateCampaignRequest request;
        private ClaimsPrincipal user;

        public CreateCampaignRequest(ApiRequests.CreateCampaignRequest request, ClaimsPrincipal user)
        {
            this.request = request;
            this.user = user;
        }

        public ApiRequests.CreateCampaignRequest NewCampaign
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
            var guid = Guid.NewGuid();

            await _repository.CreateCampaignAsync(new Campaign
            {
                CampaignUid = guid,
                CampaignName = request.NewCampaign.Name,
                Description = request.NewCampaign.Description,
                StartDate = request.NewCampaign.StartDate,
                EndDate = request.NewCampaign.EndDate,
                CampaignType = new CampaignType
                {
                    CampaignTypeUid = request.NewCampaign.CampaignTypeUid
                },
                Account = new Models.Account
                {
                    AccountUid = request.AccountUid
                }
            });

            await _mediator.Publish(new CampaignCreated(request));

            return guid;
        }
    }
}

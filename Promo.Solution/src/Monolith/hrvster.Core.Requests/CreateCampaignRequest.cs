using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
    public class CreateCampaignRequest : IRequest<Guid>
    {
        private NewCampaignRequest request;
        private ClaimsPrincipal user;
		private Guid accountUid;
		private string timezoneId;

		public CreateCampaignRequest(NewCampaignRequest request, ClaimsPrincipal user, string timezoneId)
        {
            this.request = request;
            this.user = user;
			this.timezoneId = timezoneId;
        }

		public CreateCampaignRequest(NewCampaignRequest newCampaignRequest, Guid userUid, string timezoneId)
		{
			this.request = newCampaignRequest;
			this.accountUid = userUid;
			this.timezoneId = timezoneId;
		}

		public NewCampaignRequest NewCampaign
        {
            get { return request; }
        }

		public string TimezoneId { get { return timezoneId; } }

        public Guid AccountUid
        {
            get 
			{
				return 
					user == null 
					? this.accountUid 
					: Guid.Parse(user.Claims.FirstOrDefault(c => c.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase)).Value); 
			}
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

            await _repository.CreateCampaignAsync(new Campaign
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
				Account = new Account
				{
					AccountUid = request.AccountUid
				}
            }, request.TimezoneId);

            await _mediator.Publish(new CampaignCreated(request.NewCampaign, request.AccountUid, campaignUid));

            return campaignUid;
        }
    }
}

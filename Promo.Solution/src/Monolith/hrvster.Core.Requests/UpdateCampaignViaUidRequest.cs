using hrvster.Core.Requests.ApiRequests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class UpdateCampaignViaUidRequest : IRequest<Campaign>
	{
		private Guid _campaignUid;
		private Guid _accountUid;
		private UpdateCampaignRequest _request;

		public UpdateCampaignViaUidRequest(Guid campaignUid, Guid accountUid, UpdateCampaignRequest request)
		{
			_campaignUid = campaignUid;
			_request = request;
			_accountUid = accountUid;
		}

		public Guid AccountUid
		{
			get { return _accountUid; }
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

	public class UpdateCampaignViaUidRequestHandler : IRequestHandler<UpdateCampaignViaUidRequest, Campaign>
	{
		private ICampaignRepository _repository;
		private IMediator _mediator;

		public UpdateCampaignViaUidRequestHandler(ICampaignRepository repository, IMediator mediator)
		{
			_repository = repository;
			_mediator = mediator;
		}

		public async Task<Campaign> Handle(UpdateCampaignViaUidRequest request, CancellationToken cancellationToken)
		{
			await _repository.UpdateCampaignAsync(request.CampaignUid, new Campaign
			{
				CampaignName = request.Request.CampaignName,
				Description = request.Request.Description,
				StartDate = request.Request.StartDate,
				EndDate = request.Request.EndDate,
				ActiveInd = request.Request.ActiveInd,
				CampaignType = new CampaignType
				{
					CampaignTypeUid = request.Request.CampaignTypeUid
				}
			});

			var campaign = await _repository.GetCampaignByUidAsync(request.CampaignUid, request.AccountUid);

			await _mediator.Publish(new CampaignUpdated(request.CampaignUid));

			return campaign;
		}
	}
}

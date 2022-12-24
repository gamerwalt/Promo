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
		private string _timezoneId;
		private UpdateCampaignRequest _request;

		public UpdateCampaignViaUidRequest(Guid campaignUid, Guid accountUid, UpdateCampaignRequest request, string timezoneId)
		{
			_campaignUid = campaignUid;
			_request = request;
			_accountUid = accountUid;
			_timezoneId = timezoneId;
		}

		public Guid AccountUid { get { return _accountUid; } }

		public Guid CampaignUid { get { return _campaignUid; } }

		public UpdateCampaignRequest Request { get { return _request; } }
		public string Timezone { get { return _timezoneId; } }
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
			}, request.Timezone);

			var campaign = await _repository.GetCampaignByUidAsync(request.CampaignUid, request.AccountUid);

			await _mediator.Publish(new CampaignUpdated(request.CampaignUid, request.AccountUid));

			return campaign;
		}
	}
}

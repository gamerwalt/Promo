using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetCampaignByUidRequest : IRequest<Campaign>
	{
		private Guid campaignUid;
		private Guid accountUid;

		public GetCampaignByUidRequest(Guid campaignUid, Guid accountUid)
		{
			this.campaignUid = campaignUid;
			this.accountUid = accountUid;
		}

		public Guid CampaignUid
		{
			get { return campaignUid; }
		}

		public Guid AccountUid
		{
			get { return accountUid; }
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
			return await _repository.GetCampaignByUidAsync(request.CampaignUid, request.AccountUid);
		}
	}
}

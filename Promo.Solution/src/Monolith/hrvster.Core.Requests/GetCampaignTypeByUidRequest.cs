using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetCampaignTypeByUidRequest : IRequest<CampaignType>
	{
		private Guid campaignTypeUid;

		public GetCampaignTypeByUidRequest(Guid campaignTypeUid)
		{
			this.campaignTypeUid = campaignTypeUid;
		}

		public Guid CampaignTypeUid
		{
			get { return campaignTypeUid; }
		}
	}

	public class GetCampaignTypeByUidRequestHandler : IRequestHandler<GetCampaignTypeByUidRequest, CampaignType>
	{
		private readonly ICampaignTypesRepository _repository;
		public GetCampaignTypeByUidRequestHandler(ICampaignTypesRepository repository)
		{
			_repository = repository;
		}

		public async Task<CampaignType> Handle(GetCampaignTypeByUidRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetCampaignTypeByUidAsync(request.CampaignTypeUid);
		}
	}
}

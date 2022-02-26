using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetCampaigns : IRequest<IEnumerable<Campaign>>
	{
		private List<Guid> campaignUids;

		public GetCampaigns(List<Guid> campaignUids)
		{
			this.campaignUids = campaignUids;
		}

		public List<Guid> CampaignUids
		{
			get { return campaignUids; }
		}
	}

	public class GetCampaignsHandler : IRequestHandler<GetCampaigns, IEnumerable<Campaign>>
	{
		private readonly ICampaignRepository _repository;

		public GetCampaignsHandler(ICampaignRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Campaign>> Handle(GetCampaigns request, CancellationToken cancellationToken)
		{
			var campaignList = new List<Campaign>();

			foreach (var campaignUid in request.CampaignUids)
			{
				campaignList.Add(await _repository.GetCampaignByUidAsync(campaignUid));
			}

			return campaignList;
		}
	}
}

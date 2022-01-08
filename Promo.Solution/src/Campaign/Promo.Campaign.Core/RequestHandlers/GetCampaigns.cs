using MediatR;
using Promo.Campaign.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Campaign.Core.RequestHandlers
{
	public class GetCampaigns : IRequest<IEnumerable<Infrastructure.Models.Campaign>>
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

	public class GetCampaignsHandler : IRequestHandler<GetCampaigns, IEnumerable<Infrastructure.Models.Campaign>>
	{
		private readonly ICampaignRepository _repository;

		public GetCampaignsHandler(ICampaignRepository repository)
		{
			_repository = repository;
		}

		public async Task<IEnumerable<Infrastructure.Models.Campaign>> Handle(GetCampaigns request, CancellationToken cancellationToken)
		{
			var campaignList = new List<Infrastructure.Models.Campaign>();

			foreach (var campaignUid in request.CampaignUids)
			{
				campaignList.Add(await _repository.GetCampaignByUidAsync(campaignUid));
			}

			return campaignList;
		}
	}
}

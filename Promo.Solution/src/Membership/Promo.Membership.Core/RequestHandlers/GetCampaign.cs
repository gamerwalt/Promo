using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Membership.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Membership.Core.RequestHandlers
{
	public class GetCampaign : IRequest<Infrastructure.Models.Campaign>
	{
		private Guid campaignUid;

		public GetCampaign(Guid campaignUid)
		{
			this.campaignUid = campaignUid;
		}

		public Guid CampaignUid
		{
			get { return campaignUid; }
		}
	}

	public class GetCampaignHandler : IRequestHandler<GetCampaign, Infrastructure.Models.Campaign>
	{
		private readonly ILogger<GetCampaignHandler> _logger;
		private readonly ICampaignRepository _campaignRepository;

		public GetCampaignHandler(ILoggerFactory loggerFactory, ICampaignRepository campaignRepository)
		{
			_logger = loggerFactory.CreateLogger<GetCampaignHandler>();
			_campaignRepository = campaignRepository;
		}

		public async Task<Infrastructure.Models.Campaign> Handle(GetCampaign request, CancellationToken cancellationToken)
		{
			return await _campaignRepository.GetCampaignByUidAsync(request.CampaignUid);
		}
	}
}

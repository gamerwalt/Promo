using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests
{
	public class GetCampaign : IRequest<Campaign>
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

	public class GetCampaignHandler : IRequestHandler<GetCampaign, Campaign>
	{
		private readonly ILogger<GetCampaignHandler> _logger;
		private readonly ICampaignRepository _campaignRepository;

		public GetCampaignHandler(ILoggerFactory loggerFactory, ICampaignRepository campaignRepository)
		{
			_logger = loggerFactory.CreateLogger<GetCampaignHandler>();
			_campaignRepository = campaignRepository;
		}

		public async Task<Campaign> Handle(GetCampaign request, CancellationToken cancellationToken)
		{
			return await _campaignRepository.GetCampaignByUidAsync(request.CampaignUid);
		}
	}
}

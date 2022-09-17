using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Account
{
	public class GetTotalResources : IRequestHandler<GetTotalResourcesRequest, TotalResources>
	{
		private readonly ILogger<GetTotalResources> _logger;
		private readonly ISweepsEntriesRepository _sweepsEntriesRepository;
		private readonly ICampaignRepository _campaignRepository;
		private readonly ISweepsRepository _sweepsRepository;

		public GetTotalResources(
			ILoggerFactory loggerFactory,
			ISweepsEntriesRepository sweepsEntriesRepository,
			ICampaignRepository campaignRepository,
			ISweepsRepository sweepsRepository)
		{
			_logger = loggerFactory.CreateLogger<GetTotalResources>();
			_sweepsEntriesRepository = sweepsEntriesRepository;
			_campaignRepository = campaignRepository;
			_sweepsRepository = sweepsRepository;
		}

		public async Task<TotalResources> Handle(GetTotalResourcesRequest request, CancellationToken cancellationToken)
		{
			var totalAccountSweepEntries = await _sweepsEntriesRepository.GetAccountSweepEntriesAsync(request.AccountUid);
			var totalCampaigns = await _campaignRepository.GetAccountCampaignsAsync(request.AccountUid);
			var totalSweeps = await _sweepsRepository.GetAllForAccountAsync(request.AccountUid);

			return new TotalResources
			{
				TotalSweepEntries = totalAccountSweepEntries.ToList().Count,
				TotalCampaigns = totalCampaigns.ToList().Count,
				TotalSweeps = totalSweeps.ToList().Count
			};
		}
	}
}

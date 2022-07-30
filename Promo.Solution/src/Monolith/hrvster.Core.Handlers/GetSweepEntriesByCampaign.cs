using hrvster.Core.Requests.WebRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class GetSweepEntriesByCampaign : IRequestHandler<GetSweepEntriesByCampaignRequest, IEnumerable<SweepEntryChart>>
	{
		private readonly ILogger<GetSweepEntriesByCampaign> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public GetSweepEntriesByCampaign(ILoggerFactory loggerFactory, ISweepsEntriesRepository repositoy)
		{
			_logger = loggerFactory.CreateLogger<GetSweepEntriesByCampaign>();
			_repository = repositoy;
		}

		public Task<IEnumerable<SweepEntryChart>> Handle(GetSweepEntriesByCampaignRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting tweets harvested for the account {accountUid} and campaign {campaignUid}",
				request.UserUid,
				request.CampaignUid);

			var result = _repository.GetSweepEntriesByCampaignAndUserAsync(request.UserUid, request.CampaignUid);

			return result;
		}
	}
}

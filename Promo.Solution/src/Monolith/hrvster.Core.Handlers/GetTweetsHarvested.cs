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
	public class GetTweetsHarvested : IRequestHandler<GetTweetsHarvestedRequest, IEnumerable<HarvestedTweet>>
	{
		private readonly ILogger<GetTweetsHarvested> _logger;
		private readonly ISweepsEntriesRepository _repository;

		public GetTweetsHarvested(ILoggerFactory loggerFactory, ISweepsEntriesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetTweetsHarvested>();
			_repository = repository;
		}

		public Task<IEnumerable<HarvestedTweet>> Handle(GetTweetsHarvestedRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Getting tweets harvested for the account {accountUid} and campaign {campaignUid}", 
				request.UserUid,
				request.CampaignUid);

			var result = _repository.GetHarvestedTweetsAsync(request.UserUid, request.CampaignUid);

			return result;
		}
	}
}

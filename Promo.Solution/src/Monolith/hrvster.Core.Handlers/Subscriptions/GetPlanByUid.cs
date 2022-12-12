using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Subscriptions
{
	public class GetPlanByUid : IRequestHandler<GetPlanByUidRequest, Plan>
	{
		private readonly ILogger<GetPlanByUid> _logger;
		private readonly IPlansRepository _repository;
		
		public GetPlanByUid(ILoggerFactory loggerFactory, IPlansRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetPlanByUid>();
			_repository = repository;
		}

		public Task<Plan> Handle(GetPlanByUidRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving plan with uid {planUid}", request.PlanUid);

			return _repository.GetPlanByUidAsync(request.PlanUid, cancellationToken);
		}
	}
}

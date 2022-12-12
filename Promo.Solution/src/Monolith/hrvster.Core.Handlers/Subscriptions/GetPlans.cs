using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Subscriptions
{
	public class GetPlans : IRequestHandler<GetPlansRequest, IEnumerable<Plan>>
	{
		private readonly ILogger<GetPlans> _logger;
		private readonly IPlansRepository _repository;

		public GetPlans(ILoggerFactory loggerFactory, IPlansRepository repository) 
		{
			_logger = loggerFactory.CreateLogger<GetPlans>();
			_repository= repository;
		}

		public Task<IEnumerable<Plan>> Handle(GetPlansRequest request, CancellationToken cancellationToken)
		{
			return _repository.GetPlansAsync();
		}
	}
}

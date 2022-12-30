using hrvster.Core.Requests;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models.Core;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class SubscribeUserToPlan : IRequestHandler<SubscribeUserToPlanRequest, ServiceOperation<AccountSubscription>>
	{
		private readonly ILogger<SubscribeUserToPlan> _logger;
		private readonly IAccountSubscriptionsRepository _repository;

		public SubscribeUserToPlan(ILoggerFactory loggerFactory, IAccountSubscriptionsRepository repository)
		{
			_logger = loggerFactory.CreateLogger<SubscribeUserToPlan>();
			_repository = repository;
		}

		public Task<ServiceOperation<AccountSubscription>> Handle(
			SubscribeUserToPlanRequest request, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}

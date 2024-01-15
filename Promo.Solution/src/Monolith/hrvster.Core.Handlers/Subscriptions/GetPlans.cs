using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Subscriptions
{
	public class GetPlans : IRequestHandler<GetPlansRequest, IEnumerable<Plan>>
	{
		private readonly ILogger<GetPlans> _logger;
		private readonly PaymentProviderOptions _paymentProviderOption;
		private readonly Func<PaymentClient, IPlanRepository> _planRepository;
		private readonly IPlanRepository _repository;

		public GetPlans(
			ILoggerFactory loggerFactory,
			PaymentProviderOptions paymentProviderOption,
			Func<PaymentClient, IPlanRepository> planRepository)
		{
			_logger = loggerFactory.CreateLogger<GetPlans>();
			_planRepository = planRepository;
			_paymentProviderOption = paymentProviderOption;

			var paymentProvider = Enum.Parse<PaymentClient>(_paymentProviderOption.PaymentProvider);

			_repository = _planRepository(paymentProvider);
		}

		public async Task<IEnumerable<Plan>> Handle(GetPlansRequest request, CancellationToken cancellationToken)
		{
			return await _repository.GetPlansAsync();
		}
	}
}

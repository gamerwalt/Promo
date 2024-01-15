using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Models;
using hrvster.Models.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Subscriptions
{
	public class UnsubscribeAccount : AsyncRequestHandler<UnsubscribeAccountRequest>
	{
		private readonly ILogger<UnsubscribeAccount> _logger;
		private readonly ISubscriptionService _subscriptionService;
		private readonly PaymentProviderOptions _paymentProviderOption;

		public UnsubscribeAccount(
			ILoggerFactory loggerFactory,
			ISubscriptionService subscriptionService,
			PaymentProviderOptions paymentProviderOptions)
		{
			_logger = loggerFactory.CreateLogger<UnsubscribeAccount>();
			_subscriptionService = subscriptionService;
			_paymentProviderOption = paymentProviderOptions;
		}

		protected override async Task Handle(UnsubscribeAccountRequest request, CancellationToken cancellationToken)
		{
			await _subscriptionService.UnsubscribeAsync(
				request.AccountUid,
				GetPaymentClient(),
				cancellationToken);
		}

		private static PaymentClient GetPaymentClient()
		{
			return PaymentClient.Flutterwave;
		}
	}
}

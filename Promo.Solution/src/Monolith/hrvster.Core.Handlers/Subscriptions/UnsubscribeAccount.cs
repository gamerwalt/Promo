using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Models;
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

		public UnsubscribeAccount(
			ILoggerFactory loggerFactory,
			ISubscriptionService subscriptionService)
		{
			_logger = loggerFactory.CreateLogger<UnsubscribeAccount>();
			_subscriptionService = subscriptionService;
		}

		protected override async Task Handle(UnsubscribeAccountRequest request, CancellationToken cancellationToken)
			=> await _subscriptionService.UnsubscribeAsync(
				request.AccountUid,
				PaymentClient.Flutterwave,
				cancellationToken);
	}
}

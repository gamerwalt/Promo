using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
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
		{
			//1. Call the flutterwave client to unsubscribe the user
			//2. Once that comes back, we unsubscribe the user, but leave the end date alone

			await _subscriptionService.UnsubscribeAsync(
				request.AccountUid, 
				PaymentClient.Flutterwave, 
				cancellationToken);
		}
	}
}

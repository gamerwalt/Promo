using hrvster.Core.Clients.Stripe.Models;
using hrvster.Core.Clients.Stripe.Processors;
using hrvster.Core.Requests.WebRequests.Stripe;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Stripe.Events
{
	public class ProcessStripeEvent : AsyncRequestHandler<ProcessStripeEventRequest>
	{
		private readonly ILogger<ProcessStripeEvent> _logger;
		private readonly IStripeEventProcessor _processor;

		public ProcessStripeEvent(ILoggerFactory loggerFactory, IStripeEventProcessor processor)
		{
			_logger = loggerFactory.CreateLogger<ProcessStripeEvent>();
			_processor = processor;
		}

		protected override async Task Handle(ProcessStripeEventRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Processing a stripe event with details {uid}", request.StripeEvent.Id);

			switch (request.StripeEvent.Type)
			{
				case StripeEventType.CustomerSubscriptionCreated:
				case StripeEventType.CustomerSubscriptionUpdated:
					await _processor.ProcessCustomerSubscriptionAsync(request.StripeEvent, cancellationToken);
					break;
			}
		}
	}
}

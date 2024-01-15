using hrvster.Core.Clients.Stripe.Models;
using hrvster.Core.Notifications.Stripe;
using hrvster.Infrastructure.Stripe;
using hrvster.Models.Stripe;
using MediatR;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Clients.Stripe.Processors
{
	public interface IStripeEventProcessor
	{
		Task ProcessCustomerSubscriptionAsync(Event stripeEvent, CancellationToken cancellationToken);
	}

	public class StripeEventProcessor : IStripeEventProcessor
	{
		private readonly ILogger<StripeEventProcessor> _logger;
		private readonly IStripeEventRepository _eventRepository;
		private readonly IMediator _mediator;

		private readonly CustomerService _customerService;
		private readonly PlanService _planService;
		
		public StripeEventProcessor(ILoggerFactory loggerFactory, IStripeEventRepository eventRepository, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<StripeEventProcessor>();
			_eventRepository = eventRepository;
			_mediator = mediator;

			_customerService = new CustomerService();
			_planService = new PlanService();
		}

		public async Task ProcessCustomerSubscriptionAsync(Event stripeEvent, CancellationToken cancellationToken)
		{
			var eventUid = Guid.NewGuid();

			var rawEvent = new StripeEvent
			{
				Uid = eventUid,
				RawEvent = stripeEvent.ToJson(),
				Status = hrvster.Models.StripeEventStatus.New
			};

			await _eventRepository.SaveAsync(rawEvent);

			_logger.LogInformation("Saved the stripe event with uid {eventUid}", eventUid);

			_logger.LogInformation("Saving other information about the event in the stripe_raw_events table for eventUid {eventUid}.", eventUid);
			
			await SaveStripeRawEventAsync(stripeEvent, eventUid);
		}

		private async Task SaveStripeRawEventAsync(Event stripeEvent, Guid eventUid)
		{
			var subscription = stripeEvent.Data.Object as Subscription;
			var customer = await _customerService.GetAsync(subscription.CustomerId);
			var planId = subscription.Items.Data.FirstOrDefault()?.Plan.Id;
			var plan = await _planService.GetAsync(planId);
			var productId = plan.ProductId;

			var stripeRawEvent = new StripeRawEvent
			{
				Uid = eventUid,
				ExternalId = stripeEvent.Id,
				ObjectType = stripeEvent.Object,
				DataObjectType = stripeEvent.Data.Object?.GetType().Name,
				Currency = subscription.Currency,
				CurrentPeriodStart = subscription.CurrentPeriodStart,
				CurrentPeriodEnd = subscription.CurrentPeriodEnd,
				StartDate = subscription.StartDate,
				ExternalCustomerId = subscription.CustomerId,
				ExternalPlanId = subscription.Items.Data.FirstOrDefault()?.Plan.Id,
				ExternalProductId = productId,
				Amount = plan.AmountDecimal,
				ExternalInvoiceId = subscription.LatestInvoiceId,
				ExternalStatus = subscription.Status,
				CustomerEmail = customer.Email
			};

			await _eventRepository.CreateRawEventAsync(stripeRawEvent);

			_logger.LogInformation("Finished saving the stripe raw event for event with uid {eventUid}", eventUid);

			_logger.LogInformation("Current event with uid {eventUid}'s status is {eventStatus}", eventUid, subscription.Status);

			if(subscription.Status == StripeSubscriptionStatus.Active)
			{
				_logger.LogInformation("Publishing the event UserSubscribedThroughStripe.");

				await _mediator.Publish(new UserSubscribedThroughStripe
				{
					ExternalEventId = stripeEvent.Id,
					StartDate = subscription.CurrentPeriodStart,
					EndDate = subscription.CurrentPeriodEnd,
					CustomerId = subscription.CustomerId,
					PlanId = planId,
					ProductId = productId,
					CustomerEmail = customer.Email
				});
			}
		}
	}
}

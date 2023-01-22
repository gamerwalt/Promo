using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Extensions;
using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Subscription
{
	public interface ISubscriptionService
	{
		SubscriptionStatus IsAccountSubscribed(Guid accountUid);
		Task ProcessFlutterwaveTransactionAsync(Guid flutterwaveEventUid, FlutterwaveCardTransaction cardTransaction);
		Task SaveFlutterwaveEventAsync(string flutterwaveEventString);
		Task<ServiceOperation<AccountSubscription>> SubscribeUserToAsync(Plan plan, Guid accountUid);
	}

	public class SubscriptionService : ISubscriptionService
	{
		private readonly ILogger<SubscriptionService> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly IFlutterwaveEventRepository _flutterwaveEventRepository;
		private readonly IMediator _mediator;

		public SubscriptionService(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			IFlutterwaveEventRepository flutterwaveEventRepository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionService>();
			_repository = repository;
			_flutterwaveEventRepository = flutterwaveEventRepository;
			_mediator = mediator;
		}

		public SubscriptionStatus IsAccountSubscribed(Guid accountUid)
		{
			var accountSubscription = _repository.GetAccountSubscription(accountUid);

			if (accountSubscription == null)
			{
				return SubscriptionStatus.UnSubscribed;
			}

			_logger.LogInformation("Account {accountUid} is subscribed with {subscription}", accountUid, accountSubscription.ToJsonString());

			return SubscriptionStatus.Subscribed;
		}

		public async Task ProcessFlutterwaveTransactionAsync(Guid flutterwaveEventUid, FlutterwaveCardTransaction cardTransaction)
		{
			_logger.LogInformation("FlutterwaveEventUid {eventUid}. Retrieving the account subscription for email {email}",
				flutterwaveEventUid,
				cardTransaction.Customer.Email);

			var subscriptionAccount = await _repository.GetAccountSubscriptionByEmailAsync(cardTransaction.Customer.Email);

			if (subscriptionAccount == null)
			{
				_logger.LogWarning("FlutterwaveEventUid {eventUid}. Subscription Warning: We received the event {event} from flutterwave, however, the account subscription with email {email} is not within in our system. Card Transaction",
					flutterwaveEventUid,
					cardTransaction.ToJsonString(),
					cardTransaction.Customer.Email);

				return;
			}

			if (cardTransaction.Status.Equals(
				FlutterwaveCardTransactionStatus.Successful.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				switch (subscriptionAccount.SubscriptionStatus)
				{
					case SubscriptionStatus.PendingPayment:
						await _repository.UpdateAccountSubscriptionStatusAsync(subscriptionAccount.SubscriptionUid, SubscriptionStatus.Subscribed);
						break;

					case SubscriptionStatus.Subscribed:
						//This is a new Card Transaction
						//if we are close to the end date, then this means this is a re-subscription
						//so we update the start dates and end dates
						var startDate = subscriptionAccount.EndDate.AddDays(1);
						var startDateUtc = subscriptionAccount.EndDateUtc.AddDays(1);
						var endDate = startDate.AddDays(30);
						var endDateUtc = startDateUtc.AddDays(30);

						await _repository.UpdateNewDatesAsync(subscriptionAccount.SubscriptionUid, startDate, startDateUtc, endDate, endDateUtc);

						break;
				}
			}
		}

		public async Task<ServiceOperation<AccountSubscription>> SubscribeUserToAsync(Plan plan, Guid accountUid)
		{
			var result = await _mediator.Send(new SubscribeUserToPlanRequest(accountUid, plan));

			_mediator.Publish(new UserSubscribed(accountUid, plan));

			return result;
		}

		public async Task SaveFlutterwaveEventAsync(string flutterwaveEventString)
		{
			var flutterwaveEvent = new FlutterwaveEvent()
			{
				Payload = flutterwaveEventString,
				FlutterwaveEventUid = Guid.NewGuid(),
				EventStatus = FlutterwaveEventStatus.New
			};

			await _flutterwaveEventRepository.CreateAsync(flutterwaveEvent);
		}
	}
}

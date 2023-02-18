using hrvster.ApplicationServices.Payments;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Extensions;
using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Subscription
{
	public interface ISubscriptionService
	{
		SubscriptionStatus IsAccountSubscribed(Guid accountUid);
		Task ProcessFlutterwaveTransactionAsync(Guid flutterwaveEventUid, FlutterwaveCardTransaction cardTransaction);
		Task SaveFlutterwaveEventAsync(string flutterwaveEventString);
		Task<ServiceOperation<AccountSubscription>> SubscribeUserToAsync(Plan plan, Guid accountUid);
		Task UnsubscribeAsync(Guid accountUid, PaymentClient paymentClient, CancellationToken cancellationToken);
	}

	public class SubscriptionService : ISubscriptionService
	{
		private readonly ILogger<SubscriptionService> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly IFlutterwaveEventRepository _flutterwaveEventRepository;
		private readonly IMediator _mediator;
		private readonly Func<PaymentClient, IPaymentAdaptor> _paymentResolver;
		private readonly IFlutterwaveAccountSubscriptionRepository _flutterwaveAccountSubscriptionRepository;
		private readonly IAccountsRepository _accountsRepository;

		public SubscriptionService(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			IFlutterwaveEventRepository flutterwaveEventRepository,
			IMediator mediator,
			Func<PaymentClient, IPaymentAdaptor> paymentResolver,
			IFlutterwaveAccountSubscriptionRepository flutterwaveAccountSubscriptionRepository,
			IAccountsRepository accountsRepository)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionService>();
			_repository = repository;
			_flutterwaveEventRepository = flutterwaveEventRepository;
			_mediator = mediator;
			_paymentResolver = paymentResolver;
			_flutterwaveAccountSubscriptionRepository = flutterwaveAccountSubscriptionRepository;
			_accountsRepository = accountsRepository;
		}

		public SubscriptionStatus IsAccountSubscribed(Guid accountUid)
		{
			var accountSubscription = _repository.GetAccountSubscription(accountUid);

			if (accountSubscription == null)
			{
				return SubscriptionStatus.UnSubscribed;
			}

			_logger.LogInformation("Account {accountUid} is subscribed with {subscription}", accountUid, accountSubscription.ToJsonString());

			return accountSubscription.SubscriptionStatus;
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

		public async Task UnsubscribeAsync(Guid accountUid, PaymentClient paymentClient, CancellationToken cancellationToken)
		{
			//1. Call the flutterwave client to unsubscribe the user
			//2. Once that comes back, we unsubscribe the user, but leave the end date alone

			var accountSubscription = await _repository.GetAccountSubscriptionAsync(accountUid);

			if(accountSubscription== null)
			{
				_logger.LogError("Unable to locate the account subscription for account with Uid {accountUid}", accountUid);
				_logger.LogError("Unable to unsubscribe the account with Uid {accountUid}", accountUid);

				return;
			}

			var account = await _accountsRepository.GetAsync(accountUid);

			if(account == null)
			{
				_logger.LogError("Unable to locate the account for account with Uid {accountUid}", accountUid);
				_logger.LogError("Unable to unsubscribe the account with Uid {accountUid}", accountUid);

				return;
			}

			var client = _paymentResolver(paymentClient);

			var subscription = string.Empty;

			switch(paymentClient)
			{
				case PaymentClient.Flutterwave:
					_logger.LogInformation("Retrieving user subscription from Flutterwave for account with EmailAddress {emailAddress}",
						account.EmailAddress);

					subscription = await client.GetUserSubscriptionAsync(account.EmailAddress, cancellationToken);

					if(string.IsNullOrWhiteSpace(subscription))
					{
						_logger.LogError("Unable to locate the subscription for account with Uid {accountUid} from Flutterwave", accountUid);
						_logger.LogError("Unable to unsubscribe the account with Uid {accountUid}", accountUid);

						return;
					}

					_logger.LogInformation("Retrieved subscription for account with uid {accountUid} from Flutterwave.", account.AccountUid);

					var flutterwaveSubscription = JsonConvert.DeserializeObject<FlutterwaveSubscriptionBase>(subscription);

					if(flutterwaveSubscription.Subscriptions.Count == 0)
					{
						_logger.LogError("Unable to locate the subscription for account with Uid {accountUid} from Flutterwave", accountUid);
						_logger.LogError("Unable to unsubscribe the account with Uid {accountUid}", accountUid);

						return;
					}

					_logger.LogInformation("Unsubscribing account from Flutterwave with account uid {accountUid}", account.AccountUid);

					await client.UnsubscribeUserAsync(flutterwaveSubscription.Subscriptions.FirstOrDefault().Id, cancellationToken);

					_logger.LogInformation("Finished unsubscribing account with uid {accountUid} from Flutterwave", account.AccountUid);

					await _repository.UpdateAccountSubscriptionStatusAsync(accountSubscription.SubscriptionUid, SubscriptionStatus.UnSubscribed);

					break;
			}

			
		}
	}
}

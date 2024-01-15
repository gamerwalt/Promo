using hrvster.Infrastructure;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace hrvster.Core.Notifications.Stripe.Handlers
{
	public class CreateUserSubscription : INotificationHandler<UserSubscribedThroughStripe>
	{
		private readonly ILogger<CreateUserSubscription> _logger;
		private readonly PaymentProviderOptions _paymentProviderOption;
		private readonly Func<PaymentClient, IPlanRepository> _planRepository;
		private readonly IAccountSubscriptionsRepository _accountSubscriptionRepository;
		private readonly IAccountsRepository _accountsRepository;

		public CreateUserSubscription(
			ILoggerFactory loggerFactory,
			PaymentProviderOptions paymentProviderOption,
			Func<PaymentClient, IPlanRepository> planRepository,
			IAccountSubscriptionsRepository accountSubscriptionRepository,
			IAccountsRepository accountsRepository)
		{
			_logger = loggerFactory.CreateLogger<CreateUserSubscription>();
			_planRepository = planRepository;
			_paymentProviderOption = paymentProviderOption;
			_accountSubscriptionRepository = accountSubscriptionRepository;
			_accountsRepository = accountsRepository;
		}

		public async Task Handle(UserSubscribedThroughStripe notification, CancellationToken cancellationToken)
		{
			var repository = _planRepository(Enum.Parse<PaymentClient>(_paymentProviderOption.PaymentProvider));

			var plans = await repository.GetPlansAsync();

			var plan = plans.FirstOrDefault(p => p.StripePlan.ExternalId == notification.PlanId);

			if (plan == null)
			{
				_logger.LogWarning("Unable to find a plan within our systems that match the stripe plan {stripePlanId} event coming in.", notification.PlanId);

				return;
			}

			var userAccount = await _accountsRepository.GetByEmailAddressAsync(notification.CustomerEmail);

			if (userAccount == null)
			{
				_logger.LogWarning("Unable to find the user within our systems that match the stripe user.");

				return;
			}

			await CreateAccountSubscriptionAsync(notification, plan, userAccount);
		}

		private async Task CreateAccountSubscriptionAsync(UserSubscribedThroughStripe notification, Plan? plan, Account userAccount)
		{
			var subscriptionUid = Guid.NewGuid();

			var accountSubscription = new AccountSubscription
			{
				Account = new Account
				{
					AccountUid = userAccount.AccountUid
				},
				Plan = plan,
				StartDate = notification.StartDate,
				StartDateUtc = notification.StartDate.ToUniversalTime(),
				EndDate = notification.EndDate,
				EndDateUtc = notification.EndDate.ToUniversalTime(),
				IsDiscounted = false,
				SubscriptionStatus = SubscriptionStatus.Subscribed,
				SubscriptionUid = subscriptionUid
			};

			await _accountSubscriptionRepository.CreateAsync(accountSubscription);
		}
	}
}

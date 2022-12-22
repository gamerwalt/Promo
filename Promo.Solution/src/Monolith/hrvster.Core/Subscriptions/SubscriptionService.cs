using hrvster.Core.Authentication;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace hrvster.Core.Subscriptions
{
	public interface ISubscriptionService
	{
		SubscriptionStatus IsAccountSubscribed(Guid accountUid);
	}

	public class SubscriptionService : ISubscriptionService 
	{
		private readonly ILogger<SubscriptionService> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly Subscription _subscription;
		private readonly HrvsterUser _hrvsterUser;

		public SubscriptionService(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			Subscription subscription,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionService>();
			_repository = repository;
			_subscription = subscription;
			_hrvsterUser = hrvsterUser;
		}

		public SubscriptionStatus IsAccountSubscribed(Guid accountUid)
		{
			var accountSubscription = _repository.GetAccountSubscription(accountUid);

			if (accountSubscription == null)
			{
				return SubscriptionStatus.UnSubscribed;
			}
			else if (accountSubscription.EndDate.ToUniversalTime() <= DateTime.UtcNow)
			{
				return SubscriptionStatus.SubscribedButEndDate;
			}

			_logger.LogInformation("Account {accountUid} is subscribed with {subscription}", _hrvsterUser.UserUid, accountSubscription.ToJsonString());

			_subscription.PlanName = accountSubscription.Plan.PlanName;
			_subscription.IsDiscounted = accountSubscription.IsDiscounted;
			_subscription.DiscountedBy = accountSubscription.DiscountedBy;
			_subscription.PromotionsPerMonth = accountSubscription.Plan.PromotionsPerMonth;
			_subscription.SweepstakesPerMonth = accountSubscription.Plan.SweepstakesPerMonth;
			_subscription.SweepstakesMaximumEntries = accountSubscription.Plan.SweepstakesMaximumEntries;
			_subscription.PricePerMonth = accountSubscription.Plan.PricePerMonth;
			_subscription.PricePerYear = accountSubscription.Plan.PricePerYear;

			return SubscriptionStatus.Subscribed;
		}
	}
}

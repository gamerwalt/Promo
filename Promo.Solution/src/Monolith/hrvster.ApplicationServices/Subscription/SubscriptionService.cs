using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace hrvster.ApplicationServices.Subscription
{
	public interface ISubscriptionService
	{
		SubscriptionStatus IsAccountSubscribed(Guid accountUid);
	}

	public class SubscriptionService : ISubscriptionService
	{
		private readonly ILogger<SubscriptionService> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly SubscriptionModel _subscriptionModel;
		private readonly HrvsterUser _hrvsterUser;

		public SubscriptionService(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			SubscriptionModel subscriptionModel,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionService>();
			_repository = repository;
			_subscriptionModel = subscriptionModel;
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

			_subscriptionModel.PlanName = accountSubscription.Plan.PlanName;
			_subscriptionModel.IsDiscounted = accountSubscription.IsDiscounted;
			_subscriptionModel.DiscountedBy = accountSubscription.DiscountedBy;
			_subscriptionModel.PromotionsPerMonth = accountSubscription.Plan.PromotionsPerMonth;
			_subscriptionModel.SweepstakesPerMonth = accountSubscription.Plan.SweepstakesPerMonth;
			_subscriptionModel.SweepstakesMaximumEntries = accountSubscription.Plan.SweepstakesMaximumEntries;
			_subscriptionModel.PricePerMonth = accountSubscription.Plan.PricePerMonth;
			_subscriptionModel.PricePerYear = accountSubscription.Plan.PricePerYear;

			return SubscriptionStatus.Subscribed;
		}
	}
}

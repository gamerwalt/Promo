using hrvster.Core.Requests;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Extensions;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Subscription
{
	public interface ISubscriptionService
	{
		SubscriptionStatus IsAccountSubscribed(Guid accountUid);
		Task<ServiceOperation<AccountSubscription>> SubscribeUserToAsync(Plan plan, Guid accountUid);
	}

	public class SubscriptionService : ISubscriptionService
	{
		private readonly ILogger<SubscriptionService> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly SubscriptionModel _subscriptionModel;
		private readonly IMediator _mediator;

		public SubscriptionService(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			SubscriptionModel subscriptionModel,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionService>();
			_repository = repository;
			_subscriptionModel = subscriptionModel;
			_mediator = mediator;
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

			_logger.LogInformation("Account {accountUid} is subscribed with {subscription}", accountUid, accountSubscription.ToJsonString());

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

		public async Task<ServiceOperation<AccountSubscription>> SubscribeUserToAsync(Plan plan, Guid accountUid)
		{
			return await _mediator.Send(new SubscribeUserToPlanRequest(accountUid, plan));
		}
	}
}

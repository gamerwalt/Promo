using hrvster.Core.Authentication;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace hrvster.Core.Subscriptions
{
	public class ShouldBeSubscribed : TypeFilterAttribute
	{
		public ShouldBeSubscribed() : base(typeof(ShouldBeSubscribedFilter)) { }
	}

	public class ShouldBeSubscribedFilter : IAuthorizationFilter
	{
		private readonly HrvsterUser _hrvsterUser;
		private readonly Subscription _subscription;
		private readonly ILogger<ShouldBeSubscribedFilter> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly ISessionManager _sessionManager;

		public ShouldBeSubscribedFilter(
			HrvsterUser hrvsterUser,
			Subscription subscription,
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			ISessionManager sessionManager)
		{
			_hrvsterUser = hrvsterUser;
			_subscription = subscription;
			_logger = loggerFactory.CreateLogger<ShouldBeSubscribedFilter>();
			_repository = repository;
			_sessionManager = sessionManager;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			_logger.LogInformation("Attempting to check if user {userUid} is subscribed", _hrvsterUser.UserUid);

			if (_hrvsterUser.UserUid == Guid.Empty)
			{
				context.Result = new ForbidResult();

				var displayNameValue = context.HttpContext.Request.Headers[SessionConstants.UserDisplayName].ToList();
				_hrvsterUser.DisplayName = displayNameValue.FirstOrDefault();
			}
			else
			{
				IsAccountSubscribed(context);
			}
		}

		private void IsAccountSubscribed(AuthorizationFilterContext context)
		{
			var result = _repository.GetAccountSubscription(_hrvsterUser.UserUid);

			if (result == null)
			{
				context.Result = new RedirectToActionResult("Index", "Subscriptions", new { subscriptionStatus = SubscriptionStatus.UnSubscribed.ToString() });
			}
			else if(result.EndDate.ToUniversalTime() <= DateTime.UtcNow)
			{
				context.Result = new RedirectToActionResult(
					"Index", 
					"Subscriptions", 
					new { subscriptionStatus = SubscriptionStatus.SubscribedButEndDate.ToString() });
			}
			else
			{
				_logger.LogInformation("Account {accountUid} is subscribed with {subscription}", _hrvsterUser.UserUid, result);

				_subscription.PlanName = result.Plan.PlanName;
				_subscription.IsDiscounted = result.IsDiscounted;
				_subscription.DiscountedBy = result.DiscountedBy;
				_subscription.PromotionsPerMonth = result.Plan.PromotionsPerMonth;
				_subscription.SweepstakesPerMonth = result.Plan.SweepstakesPerMonth;
				_subscription.SweepstakesMaximumEntries = result.Plan.SweepstakesMaximumEntries;
				_subscription.PricePerMonth = result.Plan.PricePerMonth;
				_subscription.PricePerYear = result.Plan.PricePerYear;
			}
		}
	}
}

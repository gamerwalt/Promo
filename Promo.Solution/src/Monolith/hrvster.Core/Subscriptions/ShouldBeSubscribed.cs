using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
using hrvster.Models;
using hrvster.Models.Subscriptions;
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
		private HrvsterUser _hrvsterUser;

		private readonly ILogger<ShouldBeSubscribedFilter> _logger;
		private readonly ISubscriptionService _subscriptionManager;
		private readonly IAccountSubscriptionService _accountSubscriptionService;
		private readonly ISessionManager _sessionManager;

		public ShouldBeSubscribedFilter(
			ILoggerFactory loggerFactory,
			ISubscriptionService subscriptionManager,
			IAccountSubscriptionService accountSubscriptionService,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<ShouldBeSubscribedFilter>();
			_sessionManager = sessionManager;
			_subscriptionManager = subscriptionManager;
			_accountSubscriptionService = accountSubscriptionService;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

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
			var subscriptionStatus = _subscriptionManager.IsAccountSubscribed(_hrvsterUser.UserUid);
			var accountSubscription = _accountSubscriptionService.GetAccountSubscription(_hrvsterUser.UserUid);

			if (accountSubscription == null)
			{
				context.Result = new RedirectToActionResult(
					"Index",
					"Subscriptions",
					new { subscriptionStatus = SubscriptionStatus.UnSubscribed.ToString() });

				return;
			}

			_logger.LogInformation("User is currently {subscriptionStatus}.", subscriptionStatus);

			switch (subscriptionStatus)
			{
				case SubscriptionStatus.UnSubscribed:
				case SubscriptionStatus.PendingPayment:
				case SubscriptionStatus.EndOfSubscription:
					_logger.LogWarning("Redirecting user to unsubscribed page.");
						
					context.Result = new RedirectToActionResult(
						"Index",
						"Subscriptions",
						new { subscriptionStatus = SubscriptionStatus.UnSubscribed.ToString() });
				
					break;
				case SubscriptionStatus.Subscribed:
					if(IsSubscriptionEndDate(accountSubscription))
					{
						_logger.LogWarning("Redirecting user to unsubscribed page.");

						context.Result = new RedirectToActionResult(
							"Index",
							"Subscriptions",
							new { SubscriptionStatus = SubscriptionStatus.EndOfSubscription.ToString() } );
					}

					break;
			}
		}

		private bool IsSubscriptionEndDate(AccountSubscription accountSubscription)
		{
			if(accountSubscription != null)
			{
				return accountSubscription.EndDateUtc < DateTime.UtcNow;
			}

			return false;
		}
	}
}

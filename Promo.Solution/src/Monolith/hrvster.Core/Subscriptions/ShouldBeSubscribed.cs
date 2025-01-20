using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
using hrvster.Models;
using hrvster.Models.Options;
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
		private readonly PaymentPendingOptions _paymentPendingSettings;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public ShouldBeSubscribedFilter(
			ILoggerFactory loggerFactory,
			ISubscriptionService subscriptionManager,
			IAccountSubscriptionService accountSubscriptionService,
			ISessionManager sessionManager,
			PaymentPendingOptions paymentPendingSettings,
			HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<ShouldBeSubscribedFilter>();
			_sessionManager = sessionManager;
			_subscriptionManager = subscriptionManager;
			_accountSubscriptionService = accountSubscriptionService;
			_paymentPendingSettings = paymentPendingSettings;
			_hrvsterSettings = hrvsterSettings;
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
					_logger.LogWarning("User is pending payment. We will check if the time is up.");
					if(accountSubscription.IsTrial)
					{
						var currentDaysInTrial = (DateTime.UtcNow - accountSubscription.StartDateUtc).TotalDays;
						if(currentDaysInTrial > _hrvsterSettings.TrialDays)
						{
							context.Result = new RedirectToActionResult(
								"Index",
								"Subscriptions",
								new { SubscriptionStatus = SubscriptionStatus.EndOfSubscription.ToString() });
						}
						break;
					}
					else
					{
						var currentHoursInPendingPayment = (DateTime.UtcNow - accountSubscription.StartDateUtc).TotalHours;
						if(currentHoursInPendingPayment > _paymentPendingSettings.NumberOfHoursBeforeRollback)
						{
							context.Result = new RedirectToActionResult(
								"Index",
								"Subscriptions",
								new { SubscriptionStatus = SubscriptionStatus.EndOfSubscription.ToString() });
						}
					}
					break;

				case SubscriptionStatus.EndOfSubscription:
					EndOfSubscription(context);
					break;

				case SubscriptionStatus.Subscribed:
					IsStillSubscribed(context, accountSubscription);
					break;
			}
		}

		private void EndOfSubscription(AuthorizationFilterContext context)
		{
			_logger.LogWarning("Redirecting user to unsubscribed page.");

			context.Result = new RedirectToActionResult(
				"Index",
				"Subscriptions",
				new { subscriptionStatus = SubscriptionStatus.UnSubscribed.ToString() });
		}

		private void IsStillSubscribed(AuthorizationFilterContext context, AccountSubscription accountSubscription)
		{
			if (IsSubscriptionEndDate(accountSubscription))
			{
				_logger.LogWarning("Redirecting user to unsubscribed page.");

				context.Result = new RedirectToActionResult(
					"Index",
					"Subscriptions",
					new { SubscriptionStatus = SubscriptionStatus.EndOfSubscription.ToString() });
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

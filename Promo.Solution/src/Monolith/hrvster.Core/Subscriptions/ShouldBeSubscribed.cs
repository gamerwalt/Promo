using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
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
		private readonly ILogger<ShouldBeSubscribedFilter> _logger;
		private readonly ISubscriptionService _subscriptionManager;

		public ShouldBeSubscribedFilter(
			HrvsterUser hrvsterUser,
			ILoggerFactory loggerFactory,
			ISubscriptionService subscriptionManager)
		{
			_hrvsterUser = hrvsterUser;
			_logger = loggerFactory.CreateLogger<ShouldBeSubscribedFilter>();
			_subscriptionManager = subscriptionManager;
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
			var subscriptionStatus = _subscriptionManager.IsAccountSubscribed(_hrvsterUser.UserUid);

			switch (subscriptionStatus)
			{
				case SubscriptionStatus.UnSubscribed:
					context.Result = new RedirectToActionResult(
						"Index",
						"Subscriptions",
						new { subscriptionStatus = SubscriptionStatus.UnSubscribed.ToString() });
					break;
				case SubscriptionStatus.SubscribedButEndDate:
					context.Result = new RedirectToActionResult(
						"Index",
						"Subscriptions",
						new { subscriptionStatus = SubscriptionStatus.SubscribedButEndDate.ToString() });
					break;
			}
		}
	}
}

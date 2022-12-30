using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SubscriptionsController : Controller
	{
		private readonly ILogger<SubscriptionsController> _logger;
		private readonly IMediator _mediator;
		private readonly ISubscriptionService _subscriptionService;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public SubscriptionsController(
			ILoggerFactory loggerFactory, 
			IMediator mediator,
			ISubscriptionService subscriptionService,
			IToastNotification notificationService,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionsController>();
			_mediator = mediator;
			_subscriptionService = subscriptionService;
			_sessionManager = sessionManager;
		}

		public async Task<ActionResult> Index(string subscriptionStatus)
		{
			return RedirectToAction(subscriptionStatus);
		}

		[Route("/Subscriptions/Unsubscribed")]
		public async Task<ActionResult> UnSubscribed()
		{
			var plans = await _mediator.Send(new GetPlansRequest());

			return View(plans);
		}

		[HttpPost]
		[Route("/SelectPlan/{planUid:Guid}")]
		public async Task<ActionResult> SelectPlanForUser(Guid planUid)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var plan = await _mediator.Send(new GetPlanByUidRequest(planUid));

			if (_hrvsterUser.UserUid == Guid.Empty)
			{
				return RedirectToAction("Dashboard");
			}

			if (plan == null) return RedirectToAction("Dashboard");

			var operation = await _subscriptionService.SubscribeUserToAsync(plan, _hrvsterUser.UserUid);

			return View(plan);
		}

		[HttpGet]
		[Route("/subscriptions/flutterwave/completed")]
		public async Task<ActionResult> Completed(string status, string txRef)
		{
			_logger.LogInformation("Information received on the get route Flutterwave. Status {status} and Transaction Reference {txRef}",
				status,
				txRef);

			return new OkObjectResult(new { message = "Get OK" });
		}

		[HttpPost]
		[Route("/subscriptions/flutterwave/completed")]
		public async Task<ActionResult> CompletedPost(string status, string txRef)
		{
			_logger.LogInformation("Information received on the get route Flutterwave. Status {status} and Transaction Reference {txRef}",
				status,
				txRef);

			return new OkObjectResult(new { message = "Post OK" });
		}
	}
}

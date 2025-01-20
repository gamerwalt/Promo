using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Subscription;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;

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
		private readonly IAccountsService _accountsService;
		private readonly PaymentProviderOptions _paymentProviderOption;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		private HrvsterUser _hrvsterUser;

		public SubscriptionsController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			ISubscriptionService subscriptionService,
			IToastNotification notificationService,
			ISessionManager sessionManager,
			IAccountsService accountsService,
			PaymentProviderOptions paymentProviderOptions,
			HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionsController>();
			_mediator = mediator;
			_subscriptionService = subscriptionService;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
			_accountsService = accountsService;
			_paymentProviderOption = paymentProviderOptions;
			_hrvsterSettings = hrvsterSettings;
		}

		public async Task<IActionResult> Index(string subscriptionStatus)
		{
			return RedirectToAction(subscriptionStatus);
		}

		[Route("/Subscriptions/Unsubscribed")]
		public async Task<IActionResult> UnSubscribed()
		{
			var plans = await _mediator.Send(new GetPlansRequest());

			var paymentClient = Enum.Parse<PaymentClient>(_paymentProviderOption.PaymentProvider);

			if (paymentClient == PaymentClient.Flutterwave)
			{
				return View(plans);
			}
			else if (paymentClient == PaymentClient.Stripe)
			{
				return View();
			}

			return View();
		}

		[Route("/Subscriptions/EndOfSubscription")]
		public async Task<IActionResult> EndOfSubscription()
		{
			_notificationService.AddInfoToastMessage("You are either in trial mode or you have reached the end of your subscription. You will need to subscribe again to continue.");

			return RedirectToAction("UnSubscribed");
		}

		[HttpPost]
		[Route("/SelectPlan/Trial/{planUid:Guid}")]
		public async Task<IActionResult> SelectTrialForUser(Guid planUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var plan = await _mediator.Send(new GetPlanByUidRequest(planUid));

			if(_hrvsterUser.UserUid == Guid.Empty)
			{
				return RedirectToAction("Dashboard");
			}

			if (plan == null) return RedirectToAction("Dashboard");

			var operation = await _subscriptionService.SubscribeUserToAsync(plan, _hrvsterUser.UserUid, true);

			if (operation.HasError)
			{
				_logger.LogWarning("Error trying to subscribe the user with Uid {userUid} to the plan {planName}. Message received. ",
					_hrvsterUser.UserUid,
					plan.PlanName);

				_notificationService.AddErrorToastMessage(operation.Message);

				return RedirectToAction("Index", "Dashboard");
			}

			_notificationService.AddSuccessToastMessage($"You have been subscribed to the trial. You will be able to use the plan for {_hrvsterSettings.TrialDays} days.");

			return RedirectToAction("Index", "Dashboard");
		}

		[HttpPost]
		[Route("/SelectPlan/{planUid:Guid}")]
		public async Task<IActionResult> SelectPlanForUser(Guid planUid)
		{
			_logger.LogInformation("Posting information for select plan {planUid}", planUid);

			_hrvsterUser = _sessionManager.GetUserInformation();

			_logger.LogInformation("Retrieved user information from session for {email}", _hrvsterUser.EmailAddress);

			var plan = await _mediator.Send(new GetPlanByUidRequest(planUid));

			_logger.LogInformation("Retrieved plan information from database {plan}", plan.ToJsonString());

			if (_hrvsterUser.UserUid == Guid.Empty)
			{
				return RedirectToAction("Dashboard");
			}

			if (plan == null) return RedirectToAction("Dashboard");

			_logger.LogInformation("Subscribing user {email} to the plan {planName}",
				_hrvsterUser.EmailAddress,
				plan.PlanName);

			var operation = await _subscriptionService.SubscribeUserToAsync(plan, _hrvsterUser.UserUid);

			_logger.LogInformation("Finished subscribing user {email} to plan {plan}. Operation: {operation}",
				_hrvsterUser.EmailAddress,
				plan.ToJsonString(),
				operation.ToJsonString());

			if (operation.HasError)
			{
				_logger.LogWarning("Error trying to subscribe the user with Uid {userUid} to the plan {planName}. Message received. ",
					_hrvsterUser.UserUid,
					plan.PlanName);

				_notificationService.AddErrorToastMessage(operation.Message);

				return RedirectToAction("Index", "Dashboard");
			}

			_logger.LogInformation("Redirecting user {email} to the link {link} to process payment.",
				_hrvsterUser.EmailAddress,
				plan.FlutterwavePlan.ExternalMonthlyPaymentLink);

			var account = await _accountsService.GetAccountByUidAsync(_hrvsterUser.UserUid);

			return Redirect($"{plan.FlutterwavePlan.ExternalMonthlyPaymentLink}?email={_hrvsterUser.EmailAddress}&firstname={account.FirstName}&lastname={account.LastName}");
		}

		[HttpGet]
		[Route("/subscriptions/flutterwave/completed/plan/{planName}")]
		public async Task<IActionResult> Completed([FromRoute] string planName, [FromQuery] string status, [FromQuery] string txRef)
		{
			_logger.LogInformation("A redirect happened and now with the plan name {planName}", planName);

			if (_sessionManager.ContainsKey(SessionConstants.UserUid) &&
				_sessionManager.ContainsKey(SessionConstants.UserToken))
			{
				var userUid = _sessionManager.ContainsKey(SessionConstants.UserUid);
				var emailAddress = _sessionManager.ContainsKey(SessionConstants.UserEmail);

				_logger.LogInformation("The user with uid {userUid} and email {emailAddress} is still logged in.",
					userUid,
					emailAddress);
			}

			_notificationService.AddSuccessToastMessage("Looks like your payment went through properly. We have subscribed you but we will be confirming if payment was successfully made in 24 hours.");

			return RedirectToAction("index", "Dashboard");
		}

		[HttpPost]
		[Route("/subscriptions/flutterwave/completed")]
		public async Task<IActionResult> CompletedPost(string status, string txRef)
		{
			_logger.LogInformation("Information received on the get route Flutterwave. Status {status} and Transaction Reference {txRef}",
				status,
				txRef);

			return new OkObjectResult(new { message = "Post OK" });
		}
	}
}

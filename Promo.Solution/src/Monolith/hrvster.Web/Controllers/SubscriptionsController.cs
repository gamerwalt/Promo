using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SubscriptionsController : Controller
	{
		private readonly ILogger<SubscriptionsController> _logger;
		private readonly IMediator _mediator;

		public SubscriptionsController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SubscriptionsController>();
			_mediator = mediator;
		}

		public async Task<ActionResult> Index(string subscriptionStatus)
		{
			return RedirectToAction(subscriptionStatus);
		}

		public async Task<ActionResult> UnSubscribed()
		{
			var plans = await _mediator.Send(new GetPlansRequest());

			return View(plans);
		}

		[HttpGet]
		[Route("/SelectPlan/{planUid:Guid}")]
		public async Task<ActionResult> SelectPlan(Guid planUid)
		{
			var plan = await _mediator.Send(new GetPlanByUidRequest(planUid));

			if (plan == null) return RedirectToAction("Dashboard");

			return View(plan);
		}
	}
}

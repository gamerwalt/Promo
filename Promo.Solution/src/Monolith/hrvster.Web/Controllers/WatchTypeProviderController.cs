using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Models;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class WatchTypeProviderController : Controller
	{
		private readonly ILogger<WatchTypeProviderController> _logger;
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly INotyfService _notificationService;

		public WatchTypeProviderController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			HrvsterUser hrvsterUser,
			INotyfService notificationService)
		{
			_logger = loggerFactory.CreateLogger<WatchTypeProviderController>();
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<IActionResult> Select(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var watchTypeProviders = await _mediator.Send(new GetWatchTypeProvidersRequest());

			var model = new SelectWatchTypeProvider()
			{
				Providers = watchTypeProviders
					.Where(a => a.ActiveInd)
					.Select(a => new SelectListItem()
					{
						Text = a.WatchTypeProviderName,
						Value = a.WatchTypeProviderCode
					}),
				Sweep = sweep,
				Campaign = sweep.Campaign,
				ReturnAction = actionName,
				ReturnController = controllerName
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Select(Guid sweepUid, SelectWatchTypeProvider form)
		{
			if(ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

					return View(form);
				}

				if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
					return RedirectToAction("Index", "Dashboard");

				switch (form.ProviderCode)
				{
					case Constants.TwitterProvider:

						//if profile exists, we redirect to supply the details
						//var profile = await _mediator.Send(new GetUserProfileRequest(_hrvsterUser.UserUid, ProfileType.Twitter));

						//if(profile != null)
						//{
						//	return RedirectToAction("StartCreate", "SweepRules", new
						//	{
						//		sweepUid = sweepUid,
						//		provider = Constants.TwitterProvider
						//	});
						//}

						HttpContext.Session.SetString(SessionConstants.SweepUid, sweepUid.ToString());
						HttpContext.Session.SetString(SessionConstants.ReturnAction, form.ReturnAction);
						HttpContext.Session.SetString(SessionConstants.ReturnController, form.ReturnController);

						return RedirectToAction("Login", "TwitterProvider");
					case Constants.FaceBookProvider:
						break;
					case Constants.InstagramProvider:
						break;
				}
			}

			return View(form);
		}
	}
}

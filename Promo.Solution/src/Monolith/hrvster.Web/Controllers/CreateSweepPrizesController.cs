using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class CreateSweepPrizesController : Controller
	{
		private readonly ILogger<CreateSweepPrizesController> _logger;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly HrvsterUser _hrvsterUser;

		public CreateSweepPrizesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IToastNotification notificationService,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepPrizesController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		[Route("/CreateSweepPrizes/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			//do we already have all the prizes we need?
			//if we do, go to view for updates
			var prizes = await _mediator.Send(new GetSweepPrizes(sweepUid));
			var drawRules = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));

			if(prizes.Count() == drawRules.WinnerCount)
			{
				_notificationService.AddWarningToastMessage("We have all the prizes we need. You can update them at this point or remove them.");

				return RedirectToAction("View", new { sweepUid = sweepUid });
			}

			var model = new CreateSweepPrize
			{
				SweepName = sweep.Name,
				SweepUid = sweep.SweepUid,
				SweepId = sweep.SweepId,
				Position = prizes.Count() + 1,
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateSweepPrizes/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid, CreateSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			return View(form);
		}
	}
}

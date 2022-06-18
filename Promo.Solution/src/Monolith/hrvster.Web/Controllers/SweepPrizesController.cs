using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepPrizesController : Controller
	{
		private readonly ILogger<SweepPrizesController> _logger;
		private readonly IMediator _mediator;

		public SweepPrizesController(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepPrizesController>();
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid sweepUid, Guid campaignUid, string returnAction, string returnController)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}
			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if (drawRule == null)
			{
				_logger.LogError("Could not find the draw rules for the sweepstake with uid: {sweepUid}", sweepUid);

				ModelState.AddModelError("CustomError", "Could not find the draw rules for the sweepstake.");

				return RedirectToAction(returnAction, returnController, new { sweepUid = sweepUid });
			}

			var model = await _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));
			model.SweepId = sweep.SweepId;
			model.SweepName = sweep.Name;
			model.ReturnController = returnController;
			model.ReturnAction = returnAction;
			model.CampaignUid = campaignUid;
			model.WinnerCount = drawRule.WinnerCount;

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create(Guid sweepUid, Guid campaignUid, string returnAction, string returnController)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if(drawRule == null)
			{
				_logger.LogError("Could not find the draw rules for the sweepstake with uid: {sweepUid}", sweepUid);

				ModelState.AddModelError("CustomError", "Could not find the draw rules for the sweepstake.");

				return RedirectToAction(returnAction, returnController, new { sweepUid = sweepUid });
			}

			var currentPrizes = await _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));

			var model = new CreateSweepPrize
			{
				SweepId = sweep.SweepId,
				SweepName = sweep.Name,
				SweepUid = sweepUid,
				ReturnAction = returnAction,
				ReturnController = returnController,
				Position = GetNextPosition(currentPrizes)
			};

			if (currentPrizes.Prizes.Count == drawRule.WinnerCount)
			{
				model.DisableAll = true;
				ModelState.AddModelError("CustomError", "We have all the prizes we need for this sweepstake.");
				return RedirectToAction(returnAction, returnController, new { campaignUid = campaignUid });
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid sweepUid, string returnAction, string returnController, CreateSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if (drawRule == null)
			{
				_logger.LogError("Could not find the draw rules for the sweepstake with uid: {sweepUid}", sweepUid);

				ModelState.AddModelError("CustomError", "Could not find the draw rules for the sweepstake.");

				return RedirectToAction(returnAction, returnController, new { sweepUid = sweepUid });
			}

			var currentPrizes = await _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));
			if (currentPrizes.Prizes.Count == drawRule.WinnerCount)
			{
				ModelState.AddModelError("CustomError", "We already have the total number of prizes we require.");

				return RedirectToAction(returnAction, returnController, new { sweepUid = sweepUid });
			}
			
			if(ModelState.IsValid)
			{
				await _mediator.Send(new CreateASweepPrizeRequest(sweepUid, form.Position, form.PrizeName, form.PrizeDescription, form.MonetaryValue));

				return RedirectToAction(
					"View", 
					new { 
						sweepUid = sweepUid, 
						returnAction = returnAction, 
						returnController = returnController 
					});
			}

			return View(form);
		}

		private int GetNextPosition(SweepPrizes currentPrizes)
		{
			if (currentPrizes.Prizes.Count == 0) return 1;

			return currentPrizes.Prizes.Count + 1;
		}
	}
}

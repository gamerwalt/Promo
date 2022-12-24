using AutoMapper;
using FluentValidation;
using hrvster.Core;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepPrizesController : Controller
	{
		private readonly ILogger<SweepPrizesController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly IValidator<CreateSweepPrize> _createSweepPrizeValidator;
		private readonly IValidator<EditSweepPrize> _editSweepPrizeValidator;
		private readonly hrvster.Models.HrvsterUser _hrvsterUser;
		private readonly PrizePositionFinder _prizePositionFinder;

		public SweepPrizesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IMapper mapper,
			IToastNotification notificationService,
			IValidator<CreateSweepPrize> createSweepPrizeValidator,
			IValidator<EditSweepPrize> editSweepPrizeValidator,
			hrvster.Models.HrvsterUser hrvsterUser,
			PrizePositionFinder prizePositionFinder)
		{
			_logger = loggerFactory.CreateLogger<SweepPrizesController>();
			_mediator = mediator;
			_mapper = mapper;
			_notificationService = notificationService;
			_createSweepPrizeValidator = createSweepPrizeValidator;
			_editSweepPrizeValidator = editSweepPrizeValidator;
			_hrvsterUser = hrvsterUser;
			_prizePositionFinder = prizePositionFinder;
		}

		[HttpGet]
		public async Task<IActionResult> View(Guid sweepUid, Guid campaignUid, string returnAction, string returnController)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, returnAction, returnController);
			}
			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(returnAction ?? "Index", returnController ?? "Sweeps");

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if (drawRule == null)
			{
				return LogAndRedirectBack(
					$"Could not find the draw rules for the sweepstake {sweep.Name}. We need the draw rules before we can access the prizes.",
					sweepUid, campaignUid, returnAction, returnController);
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
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, returnAction, returnController);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(returnAction ?? "Index", returnController ?? "Sweeps");

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if (drawRule == null)
			{
				return LogAndRedirectBack(
					$"Could not find the draw rules for the sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, returnAction, returnController);
			}

			var currentPrizes = await _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));

			var model = new CreateSweepPrize
			{
				SweepId = sweep.SweepId,
				SweepName = sweep.Name,
				SweepUid = sweepUid,
				CampaignUid = campaignUid,
				ReturnAction = returnAction ?? "View",
				ReturnController = returnController ?? "Sweeps",
				Position = GetNextPosition(currentPrizes, drawRule.WinnerCount)
			};

			if (currentPrizes.Prizes.Count == drawRule.WinnerCount)
			{
				model.DisableAll = true;
				_notificationService.AddWarningToastMessage("We have all the prizes we need for this sweepstake.");

				return RedirectToAction(
					returnAction ?? "View",
					returnController ?? "Sweeps",
					new
					{
						campaignUid = campaignUid,
						sweepUid = sweepUid
					});
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Guid sweepUid, Guid campaignUid, string returnAction, string returnController, CreateSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, returnAction, returnController);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(returnAction ?? "Index", returnController ?? "Sweeps");

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if (drawRule == null)
			{
				return LogAndRedirectBack(
					$"Could not find the draw rules for the sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, returnAction, returnController);
			}

			var currentPrizes = await _mediator.Send(new GetSweepPrizesBySweepUidRequest(sweepUid));
			if (currentPrizes.Prizes.Count == drawRule.WinnerCount)
			{
				ModelState.AddModelError("CustomError", "We already have the total number of prizes we require.");

				return RedirectToAction(returnAction, returnController, new { campaignUid = campaignUid, sweepUid = sweepUid });
			}

			var validationResult = await _createSweepPrizeValidator.ValidateAsync(form);

			if (validationResult.IsValid)
			{
				await _mediator.Send(new CreateASweepPrizeRequest(sweepUid, form.Position, form.PrizeName, form.PrizeDescription, form.MonetaryValue));

				return RedirectToAction(
					returnAction ?? "View", returnController ?? "Sweeps",
					new
					{
						sweepUid = sweepUid,
						campaignUid = campaignUid,
						returnAction = returnAction ?? "View",
						returnController = returnController ?? "Sweeps"
					});
			}

			_notificationService.AddErrorToastMessage(
				$"There were issues saving the Sweep Prize. ${System.Environment.NewLine}Errors returned: {string.Join(", ", validationResult.Errors)}");

			return View(form);
		}

		[HttpGet]
		public async Task<IActionResult> DeletePrize(Guid prizeUid, Guid sweepUid, Guid campaignUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				return LogAndRedirectBack($"Could not find the prize with the uid: {prizeUid}.",
					sweepUid, campaignUid, actionName, controllerName);
			}

			var model = new DeleteSweepPrize
			{
				Sweep = sweep,
				SweepId = sweep.SweepId,
				Prize = _mapper.Map<Prize>(prize),
				PrizeName = prize.PrizeName,
				PrizeUid = prize.PrizeUid,
				MonetaryValue = prize.PrizeValue,
				PrizeDescription = prize.PrizeDescription,
				SweepName = sweep.Name,
				SweepUid = sweep.SweepUid,
				CampaignUid = campaignUid,
				ReturnAction = actionName,
				ReturnController = controllerName,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeletePrize(
			Guid prizeUid,
			Guid sweepUid,
			Guid campaignUid,
			string actionName,
			string controllerName,
			DeleteSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				return LogAndRedirectBack($"Could not find the prize with the uid: {prizeUid}.",
					sweepUid, campaignUid, actionName, controllerName);
			}

			await _mediator.Send(new DeleteSweepPrizeRequest(prizeUid));

			_notificationService.AddSuccessToastMessage("Prize deleted successfully.");

			return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps",
				new
				{
					sweepUid = sweepUid,
					campaignUid = campaignUid,
					actionName = actionName ?? "View",
					controllerName = controllerName ?? "Sweeps"
				});
		}

		[HttpGet]
		public async Task<IActionResult> EditPrize(
			Guid prizeUid,
			Guid sweepUid,
			Guid campaignUid,
			string actionName,
			string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				return LogAndRedirectBack($"Could not find the prize with the uid: {prizeUid}.",
					sweepUid, campaignUid, actionName, controllerName);
			}

			var model = new EditSweepPrize
			{
				Sweep = sweep,
				SweepId = sweep.SweepId,
				Prize = _mapper.Map<Prize>(prize),
				PrizeName = prize.PrizeName,
				PrizeUid = prize.PrizeUid,
				MonetaryValue = prize.PrizeValue,
				PrizeDescription = prize.PrizeDescription,
				SweepName = sweep.Name,
				SweepUid = sweep.SweepUid,
				CampaignUid = campaignUid,
				ReturnAction = actionName,
				ReturnController = controllerName
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> EditPrize(
			Guid prizeUid,
			Guid sweepUid,
			Guid campaignUid,
			string actionName,
			string controllerName,
			EditSweepPrize form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				return LogAndRedirectBack(
					$"Could not find sweepstake with uid: {sweepUid}",
					sweepUid, campaignUid, actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var prize = await _mediator.Send(new GetSweepPrizeByPrizeUid(prizeUid));
			if (prize == null)
			{
				return LogAndRedirectBack($"Could not find the prize with the uid: {prizeUid}.",
					sweepUid, campaignUid, actionName, controllerName);
			}

			var validationResult = _editSweepPrizeValidator.Validate(form);

			if (validationResult.IsValid)
			{
				await _mediator.Send(new UpdateSweepPrize(sweepUid, prizeUid, form.PrizeName, form.PrizeDescription, form.MonetaryValue));

				_notificationService.AddSuccessToastMessage("Prize updated successfully");

				return RedirectToAction(
					actionName ?? "View",
					controllerName ?? "Sweeps",
					new
					{
						sweepUid = sweepUid,
						campaignUid = campaignUid,
						returnAction = actionName ?? "View",
						returnController = controllerName ?? "Sweeps"
					});
			}

			_notificationService.AddErrorToastMessage(
				$"There were issues saving the Sweep Prize. ${System.Environment.NewLine}Errors returned: {string.Join(", ", validationResult.Errors)}");

			return View(form);
		}


		private IActionResult LogAndRedirectBack(string message, Guid sweepUid, Guid campaignUid, string returnAction, string returnController)
		{
			_logger.LogError(message);
			_notificationService.AddWarningToastMessage(message);

			return RedirectToAction(returnAction ?? "View", returnController ?? "Sweeps", new { sweepUid = sweepUid, campaignUid = campaignUid });
		}

		private int GetNextPosition(SweepPrize currentPrizes, int winnerCount)
		{
			if (currentPrizes.Prizes.Count == 0) return 1;

			return _prizePositionFinder.GetNextPrizePosition(winnerCount, currentPrizes.Prizes);
		}
	}
}

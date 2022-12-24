using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Core.Validators.Sweepstakes;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.Options;
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
	public class SweepEntryRuleController : Controller
	{
		private readonly ILogger<SweepEntryRuleController> _logger;
		private readonly IMediator _mediator;
		private readonly SweepstakeConfiguration _sweepConfig;
		private readonly IToastNotification _notificationService;
		private readonly ISweepRuleUpdateValidator _sweepRuleValidator;
		private readonly hrvster.Models.HrvsterUser _hrvsterUser;

		public SweepEntryRuleController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			SweepstakeConfiguration sweepConfig,
			IToastNotification notificationService,
			ISweepRuleUpdateValidator sweepRuleValidator,
			hrvster.Models.HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<SweepEntryRuleController>();
			_mediator = mediator;
			_sweepConfig = sweepConfig;
			_notificationService = notificationService;
			_sweepRuleValidator = sweepRuleValidator;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		public async Task<ActionResult> Create(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var model = new Models.CreateSweepEntryRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				EntryPerUser = _sweepConfig.EntryPerUser,
				ReturnAction = actionName ?? "View",
				ReturnController = controllerName ?? "Sweeps"
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(Guid sweepUid, Models.CreateSweepEntryRule model, string actionName, string controllerName)
		{
			if (ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
					return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

				await _mediator.Send(new CreateSweepEntryRuleRequest
				{
					SweepUid = sweepUid,
					SweepId = model.SweepstakeId,
					EntryPerUser = model.EntryPerUser
				});

				_notificationService.AddSuccessToastMessage("Sweep entry rule created successfully.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> Update(Guid sweepUid, Guid entryRuleUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var sweepEntryRule = await _mediator.Send(new GetSweepEntryRuleByUid(entryRuleUid));
			if (sweepEntryRule == null)
			{
				_logger.LogError("Could not find the sweep entry rule with uid: {sweepEntryRuleUid}", entryRuleUid);

				_notificationService.AddWarningToastMessage($"Could not find the sweep entry rule with uid: {entryRuleUid}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			var canUpdate = await _sweepRuleValidator.CanUpdateAsync(sweep, sweepEntryRule);

			if (canUpdate.HasError)
			{
				_logger.LogWarning("Cannot update this sweep entry rule. Reason {message}", canUpdate.Message);

				_notificationService.AddWarningToastMessage($"Cannot update this sweep entry rule. Reason {canUpdate.Message}");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			var model = new UpdateSweepEntryRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid,
				EntryPerUser = sweepEntryRule.EntryPerUser,
				SweepEntryRuleUid = sweepEntryRule.EntryRuleUid,
				ReturnAction = actionName ?? "View",
				ReturnController = controllerName ?? "Sweeps"
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Update(Guid sweepUid, Guid entryRuleUid, UpdateSweepEntryRule form, string actionName, string controllerName)
		{
			if (ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
					return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

				var sweepEntryRule = await _mediator.Send(new GetSweepEntryRuleByUid(entryRuleUid));
				if (sweepEntryRule == null)
				{
					_logger.LogError("Could not find the sweep entry rule with uid: {sweepEntryRuleUid}", entryRuleUid);

					_notificationService.AddWarningToastMessage($"Could not find Entry rule with uid: {entryRuleUid}.");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				var canUpdate = await _sweepRuleValidator.CanUpdateAsync(sweep, sweepEntryRule);

				if (canUpdate.HasError)
				{
					_logger.LogWarning("Cannot update this sweep entry rule. Reason {message}", canUpdate.Message);

					_notificationService.AddWarningToastMessage($"Cannot update this sweep entry rule. Reason {canUpdate.Message}");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				var request = new UpdateSweepEntryRuleRequest
				(
					sweep.SweepUid,
					sweep.SweepId,
					sweepEntryRule.EntryRuleUid,
					form.EntryPerUser
				);

				await _mediator.Send(request);

				_notificationService.AddSuccessToastMessage("Sweep entry rule updated successfully.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("Update", "SweepEntryRule", new
			{
				sweepUid = sweepUid,
				entryRuleUid = entryRuleUid
			});
		}
	}
}

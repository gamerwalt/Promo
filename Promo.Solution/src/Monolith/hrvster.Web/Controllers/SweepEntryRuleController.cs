using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Core.Validators.Sweepstakes;
using hrvster.Models.Options;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
		private readonly INotyfService _notificationService;
		private readonly ISweepRuleUpdateValidator _sweepRuleValidator;

		public SweepEntryRuleController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			SweepstakeConfiguration sweepConfig,
			INotyfService notificationService,
			ISweepRuleUpdateValidator sweepRuleValidator)
		{
			_logger = loggerFactory.CreateLogger<SweepEntryRuleController>();
			_mediator = mediator;
			_sweepConfig = sweepConfig;
			_notificationService = notificationService;
			_sweepRuleValidator = sweepRuleValidator;
		}

		[HttpGet]
		public async Task<ActionResult> Create(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "SweepRules", new { sweepUid = sweepUid });
			}

			var model = new Models.CreateSweepEntryRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				EntryPerUser = _sweepConfig.EntryPerUser,
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(Guid sweepUid, Models.CreateSweepEntryRule model)
		{
			if(ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

					return View(model);
				}

				var request = new CreateSweepEntryRuleRequest
				{
					SweepUid = sweepUid,
					SweepId = model.SweepstakeId,
					EntryPerUser = model.EntryPerUser
				};

				await _mediator.Send(request);

				if (string.IsNullOrWhiteSpace(model.ReturnAction))
					return RedirectToAction("Create", new { sweepUid = sweepUid });

				return RedirectToAction(model.ReturnAction, model.ReturnController, new { sweepUid = sweepUid });
			}

			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> Update(Guid sweepUid, Guid entryRuleUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "SweepRules", new { sweepUid = sweepUid });
			}
			var sweepEntryRule = await _mediator.Send(new GetSweepEntryRuleByUid(entryRuleUid));
			if(sweepEntryRule == null)
			{
				_logger.LogError("Could not find the sweep entry rule with uid: {sweepEntryRuleUid}", entryRuleUid);

				_notificationService.Warning($"Could not find the sweep entry rule with uid: {entryRuleUid}.");

				return RedirectToAction("View", "SweepRules", new { sweepUid = sweepUid });
			}

			var canUpdate = await _sweepRuleValidator.CanUpdateAsync(sweep, sweepEntryRule);

			if(canUpdate.HasError)
			{
				_logger.LogWarning("Cannot update this sweep entry rule. Reason {message}", canUpdate.Message);

				_notificationService.Warning($"Cannot update this sweep entry rule. Reason {canUpdate.Message}");

				return RedirectToAction("View", "SweepRules", new { sweepUid = sweepUid });
			}

			var model = new UpdateSweepEntryRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid,
				EntryPerUser = sweepEntryRule.EntryPerUser,
				SweepEntryRuleUid = sweepEntryRule.EntryRuleUid,
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Update(Guid sweepUid, Guid entryRuleUid, UpdateSweepEntryRule form)
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
				var sweepEntryRule = await _mediator.Send(new GetSweepEntryRuleByUid(entryRuleUid));
				if (sweepEntryRule == null)
				{
					_logger.LogError("Could not find the sweep entry rule with uid: {sweepEntryRuleUid}", entryRuleUid);

					_notificationService.Warning($"Could not find Entry rule with uid: {entryRuleUid}.");

					return View(form);
				}

				var canUpdate = await _sweepRuleValidator.CanUpdateAsync(sweep, sweepEntryRule);

				if (canUpdate.HasError)
				{
					_logger.LogWarning("Cannot update this sweep entry rule. Reason {message}", canUpdate.Message);

					_notificationService.Warning($"Cannot update this sweep entry rule. Reason {canUpdate.Message}");

					return RedirectToAction("View", "Sweep", new {sweepUid = sweepUid});
				}

				var request = new UpdateSweepEntryRuleRequest
				(
					sweep.SweepUid,
					sweep.SweepId,
					sweepEntryRule.EntryRuleUid,
					form.EntryPerUser
				);

				await _mediator.Send(request);

				if (string.IsNullOrWhiteSpace(form.ReturnAction))
					return RedirectToAction("Update", new { sweepUid = sweepUid, entryRuleUid = entryRuleUid });

				return RedirectToAction(
					form.ReturnAction, 
					form.ReturnController, 
					new 
						{ sweepUid = sweepUid });
			}

			return View(form);
		}
	}
}

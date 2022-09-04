using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions.ModelExtensions;
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
	public class SweepDrawRuleController : Controller
	{
		private readonly ILogger<SweepDrawRuleController> _logger;
		private readonly IMediator _mediator;
		private readonly SweepstakeConfiguration _sweepConfig;
		private readonly INotyfService _notificationService;

		public SweepDrawRuleController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			SweepstakeConfiguration sweepConfig,
			INotyfService notificationService)
		{
			_logger = loggerFactory.CreateLogger<SweepDrawRuleController>();
			_mediator = mediator;
			_sweepConfig = sweepConfig;
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<ActionResult> Create(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			var model = new CreateSweepDrawRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				WinnerCount = _sweepConfig.WinnerCount,
				WinnerBackupCount = _sweepConfig.WinnerBackupCount,
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString()
			};

			return View(model);
		}

		[HttpGet]
		public async Task<ActionResult> Update(Guid sweepUid, Guid drawRuleUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			var sweepDrawRule = await _mediator.Send(new GetSweepDrawRuleRequest(drawRuleUid));
			if(sweepDrawRule == null)
			{
				_logger.LogError("Could not find the draw rule with uid: {sweepDrawRuleUid}", drawRuleUid);

				_notificationService.Warning($"Could not find the draw rule with uid: {drawRuleUid}");

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			var model = new UpdateSweepDrawRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				SweepDrawRuleUid = sweepDrawRule.DrawRuleUid,
				WinnerCount = sweepDrawRule.WinnerCount,
				WinnerBackupCount = sweepDrawRule.WinnerBackupCount,
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString()
			};

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Create(Guid sweepUid, CreateSweepDrawRule model, string actionName, string controllerName)
		{
			if (ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				var request = new CreateSweepDrawRuleRequest
				{
					SweepUid = sweepUid,
					SweepId = model.SweepstakeId,
					WinnerCount = model.WinnerCount,
					WinnerBackupCount = model.WinnerBackupCount
				};

				await _mediator.Send(request);

				_notificationService.Success("Draw rule created successfully.", 10);

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}

			_notificationService.Error(ModelState.BuildMessageFromErrors());

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Update(Guid sweepUid, Guid drawRuleUid, UpdateSweepDrawRule form, string actionName, string controllerName)
		{
			if(ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				var sweepDrawRule = await _mediator.Send(new GetSweepDrawRuleRequest(drawRuleUid));
				if (sweepDrawRule == null)
				{
					_logger.LogError("Could not find the draw rule with uid: {sweepDrawRuleUid}", drawRuleUid);

					_notificationService.Warning($"Could not find the draw rule with uid: {drawRuleUid}");

					return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
				}

				var request = new UpdateSweepDrawRuleRequest(sweep.SweepId, sweepUid, drawRuleUid, form.WinnerCount, form.WinnerBackupCount);

				await _mediator.Send(request);

				_notificationService.Success("Draw rule updated successfully.", 10);

				return RedirectToAction(actionName ?? "View", controllerName ?? "Sweeps", new { sweepUid = sweepUid });
			}
			
			_notificationService.Error(ModelState.BuildMessageFromErrors());

			return View(form);
		}
	}
}

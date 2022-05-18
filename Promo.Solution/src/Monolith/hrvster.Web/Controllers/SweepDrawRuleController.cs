using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Core.Settings;
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
		private readonly SweepConfiguration _sweepConfig;

		public SweepDrawRuleController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			SweepConfiguration sweepConfig)
		{
			_logger = loggerFactory.CreateLogger<SweepDrawRuleController>();
			_mediator = mediator;
			_sweepConfig = sweepConfig;
		}

		[HttpGet]
		public async Task<ActionResult> Create(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
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

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var sweepDrawRule = await _mediator.Send(new GetSweepDrawRuleRequest(drawRuleUid));
			if(sweepDrawRule == null)
			{
				_logger.LogError("Could not find the draw rule with uid: {sweepDrawRuleUid}", drawRuleUid);

				return new NotFoundObjectResult($"Could not find the draw rule with uid: {drawRuleUid}");
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
		public async Task<ActionResult> Create(Guid sweepUid, CreateSweepDrawRule model)
		{
			if (ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
				}

				var request = new CreateSweepDrawRuleRequest
				{
					SweepUid = sweepUid,
					SweepId = model.SweepstakeId,
					WinnerCount = model.WinnerCount,
					WinnerBackupCount = model.WinnerBackupCount
				};

				await _mediator.Send(request);

				if (string.IsNullOrWhiteSpace(model.ReturnAction))
					return RedirectToAction("Create", new { sweepUid = sweepUid });

				return RedirectToAction(model.ReturnAction, model.ReturnController, new { sweepUid = sweepUid });
			}

			return View(model);
		}

		[HttpPost]
		public async Task<ActionResult> Update(Guid sweepUid, Guid drawRuleUid, UpdateSweepDrawRule form)
		{
			if(ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
				}

				var sweepDrawRule = await _mediator.Send(new GetSweepDrawRuleRequest(drawRuleUid));
				if (sweepDrawRule == null)
				{
					_logger.LogError("Could not find the draw rule with uid: {sweepDrawRuleUid}", drawRuleUid);

					return new NotFoundObjectResult($"Could not find the draw rule with uid: {drawRuleUid}");
				}

				var request = new UpdateSweepDrawRuleRequest(sweep.SweepId, sweepUid, drawRuleUid, form.WinnerCount, form.WinnerBackupCount);

				await _mediator.Send(request);

				if (string.IsNullOrWhiteSpace(form.ReturnAction))
					return RedirectToAction("Update", new { sweepUid = sweepUid, drawRuleUid = drawRuleUid });

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

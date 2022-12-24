using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Extensions.ModelExtensions;
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
	public class CreateDrawRulesController : Controller
	{
		private readonly ILogger<CreateDrawRulesController> _logger;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly hrvster.Models.HrvsterUser _hrvsterUser;

		public CreateDrawRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IToastNotification notificationService,
			hrvster.Models.HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<CreateDrawRulesController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		[Route("/CreateDrawRules/Create/Sweep/{sweepUid:Guid}")]
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

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));

			if(drawRule != null)
			{
				_logger.LogInformation("Draw rules already exists for sweep {sweepstakeName}.", sweep.Name);

				return RedirectToAction("View", new { sweepUid = sweepUid });
			}

			var model = new CreateSweepDrawRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateDrawRules/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid, CreateSweepDrawRule form)
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

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));

			if (drawRule != null)
			{
				_logger.LogInformation("Draw rules already exists for sweep {sweepstakeName}.", sweep.Name);

				return RedirectToAction("View", new { sweepUid = sweepUid });
			}

			if(ModelState.IsValid)
			{
				await _mediator.Send(new CreateSweepDrawRuleRequest
				{
					SweepId = sweep.SweepId,
					SweepUid = sweep.SweepUid,
					WinnerCount = form.WinnerCount,
					WinnerBackupCount = form.WinnerBackupCount,
					AccountUid = _hrvsterUser.UserUid
				});

				_notificationService.AddSuccessToastMessage("Draw rules added successfully. Now we create the prizes for the winners.");

				return RedirectToAction("Create", "CreateSweepPrizes", new { sweepUid = sweepUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from creating the draw rule sweepstake {sweepName}. ClassName: {className}. Method Name: {action}. Message: {message}",
				sweep.Name,
				nameof(CreateDrawRulesController),
				nameof(Create),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			form.SweepstakeName = sweep.Name;
			form.SweepstakeId = sweep.SweepId;
			form.SweepstakeUid = sweep.SweepUid;

			return View(form);
		}

		[HttpGet]
		[Route("/CreateDrawRules/View/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid)
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

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if(drawRule == null)
			{
				return RedirectToAction("Create", new { sweepUid = sweepUid });
			}

			var model = new UpdateSweepDrawRule
			{
				SweepDrawRuleUid = drawRule.DrawRuleUid,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeId = sweep.SweepId,
				WinnerCount = drawRule.WinnerCount,
				WinnerBackupCount = drawRule.WinnerBackupCount
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateDrawRules/View/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid, UpdateSweepDrawRule form)
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

			if(sweep.SweepUid != sweepUid)
				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });

			var drawRule = await _mediator.Send(new GetDrawRuleForSweepsRequest(sweepUid));
			if (drawRule == null)
			{
				return RedirectToAction("Create", new { sweepUid = sweepUid });
			}

			if (drawRule.DrawRuleUid != form.SweepDrawRuleUid)
				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });

			if(ModelState.IsValid)
			{
				if(IsDirty(form, drawRule))
				{
					await _mediator.Send(new UpdateSweepDrawRuleRequest(
					sweep.SweepId,
					sweep.SweepUid,
					form.SweepDrawRuleUid,
					form.WinnerCount,
					form.WinnerBackupCount));

					_notificationService.AddSuccessToastMessage("Draw rule updated successfully.");
				}

				return RedirectToAction("Create", "CreateSweepPrizes", new { sweepUid = sweepUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from updating the Draw Rule for sweepstake {sweepName}. ClassName: {className}. Method Name: {action}. Message: {message}",
				sweep.Name,
				nameof(CreateDrawRulesController),
				nameof(View),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			form.SweepDrawRuleUid = drawRule.DrawRuleUid;
			form.SweepstakeUid = sweepUid;
			form.SweepstakeName = sweep.Name;
			form.SweepstakeId = sweep.SweepId;

			return View(form);
		}

		private bool IsDirty(UpdateSweepDrawRule form, DrawRule drawRule)
		{
			return form.WinnerCount != drawRule.WinnerCount ||
				form.WinnerBackupCount != drawRule.WinnerBackupCount;
		}
	}
}

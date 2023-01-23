using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Core.Requests.WebRequests.Sweeps.Creation;
using hrvster.Core.Requests.WebRequests.Sweeps.Gets;
using hrvster.Core.Requests.WebRequests.Sweeps.Updates;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;
using CreateSweepEntryRule = hrvster.Web.Models.CreateSweepEntryRule;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class CreateEntryRulesController : Controller
	{
		private readonly ILogger<CreateEntryRulesController> _logger;
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public CreateEntryRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IToastNotification notificationService,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<CreateEntryRulesController>();
			_mediator = mediator;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
		}

		[HttpGet]
		[Route("/CreateEntryRules/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			//Do we have any entry already or winner count and backup
			var sweepEntryRule = await _mediator.Send(new GetSweepEntryRuleForSweepRequest(sweepUid));

			if (sweepEntryRule != null)
			{
				_logger.LogInformation("Entry rule and draw rule already exists for sweep {sweepName}. Details: {details}",
					sweep.Name,
					sweepEntryRule.ToJsonString());

				return RedirectToAction("View", new { sweepUid = sweepUid });
			}

			var model = CreateSweepEntryRuleModel(sweep);

			return View(model);
		}

		private CreateSweepEntryRule CreateSweepEntryRuleModel(Sweep sweep)
		{
			return new CreateSweepEntryRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				EntryPerUser = 0,
				SweepstakeName = sweep.Name
			};
		}

		[HttpPost]
		[Route("/CreateEntryRules/Create/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Create(Guid sweepUid, CreateSweepEntryRule form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			//Do we have any entry already or winner count and backup
			var sweepEntryRule = await _mediator.Send(new GetSweepEntryRuleForSweepRequest(sweepUid));

			if (sweepEntryRule != null)
			{
				_logger.LogInformation("Entry rule and draw rule already exists for sweep {sweepName}. Details: {details}",
					sweep.Name,
					sweepEntryRule.ToJsonString());

				return RedirectToAction("View", new { sweepUid = sweepUid });
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new CreateEntryRuleRequest(sweep, form.EntryPerUser));

				_notificationService.AddSuccessToastMessage("Entry rule created successfully.");

				return RedirectToAction("Create", "CreateDrawRules", new { sweepUid = sweepUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from creating the Entry rule sweepstake {sweepName}. ClassName: {className}. Method Name: {action}. Message: {message}",
				sweep.Name,
				nameof(CreateEntryRulesController),
				nameof(Create),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			form.SweepstakeName = sweep.Name;
			form.SweepstakeId = sweep.SweepId;
			form.SweepstakeUid = sweep.SweepUid;

			return View(form);
		}

		[HttpGet]
		[Route("/CreateEntryRules/View/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			var entryRule = await _mediator.Send(new GetSweepEntryRuleForSweepRequest(sweepUid));

			if (entryRule == null)
				return RedirectToAction("Create", new { sweepUid = sweepUid });

			_logger.LogInformation("Entry rules for sweep {sweepName}, Details: {details}", sweep.Name, entryRule.ToJsonString());

			var model = new UpdateSweepEntryRule
			{
				EntryPerUser = entryRule.EntryPerUser,
				SweepstakeUid = sweepUid,
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepEntryRuleUid = entryRule.EntryRuleUid,
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateEntryRules/View/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> View(Guid sweepUid, UpdateSweepEntryRule form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogWarning("Could not find the sweepstake with uid {sweepsUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("View", "Sweeps", new { sweepsUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			if (ModelState.IsValid)
			{
				var entryRule = await _mediator.Send(new GetSweepEntryRuleForSweepRequest(sweepUid));

				if (IsDirty(form, entryRule))
				{
					_notificationService.AddSuccessToastMessage("Entry rule updated successfully.");
					
					await _mediator.Send(new UpdateEntryRuleRequest(sweep, form.SweepEntryRuleUid, form.EntryPerUser));
				}

				return RedirectToAction("Create", "CreateDrawRules", new { sweepUid = sweepUid });
			}

			var messages = ModelState.BuildMessageFromErrors();

			_logger.LogInformation("Error from updating the Entry Rule for sweepstake {sweepName}. ClassName: {className}. Method Name: {action}. Message: {message}",
				sweep.Name,
				nameof(CreateEntryRulesController),
				nameof(View),
				messages);

			_notificationService.AddErrorToastMessage(messages);

			form.SweepstakeName = sweep.Name;
			form.SweepstakeId = sweep.SweepId;
			form.SweepstakeUid = sweep.SweepUid;

			return View(form);
		}

		private bool IsDirty(UpdateSweepEntryRule form, SweepEntryRule entryRule)
		{
			return form.EntryPerUser != entryRule.EntryPerUser;
		}
	}
}

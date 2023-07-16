using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Core.Subscriptions;
using hrvster.Models;
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
	[ShouldBeSubscribed]
	public class SweepRulesController : Controller
	{
		private readonly ILogger<SweepRulesController> _logger;
		private readonly IMediator _mediator;
		private readonly Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> _publisherResolver;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		private HrvsterUser _hrvsterUser;

		public SweepRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> publisherResolver,
			IMapper mapper,
			IToastNotification notificationService,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<SweepRulesController>();
			_mediator = mediator;
			_publisherResolver = publisherResolver;
			_mapper = mapper;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweepRule = await _mediator.Send(new GetSweepstakeAndRulesRequests(sweepUid));

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index", "Dashboard");
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("Index", "Sweeps");

			return View(sweepRule);
		}

		[HttpGet]
		public async Task<IActionResult> Activate(
			Guid sweepUid,
			Guid ruleUid,
			string actionName,
			string controllerName)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.AddWarningToastMessage($"Could not find rule with uid: {ruleUid} ");

				return RedirectToAction(actionName, controllerName);
			}

			var entriesExists = await _mediator.Send(new EntriesExistsForSweepsRequest(sweepUid));

			if (entriesExists)
			{
				_logger.LogWarning("Sweep entries exists for sweepstakes with uid: {sweepUid} and name {sweepName} and rule for {watchValue}.",
					sweep.SweepUid,
					sweep.Name,
					rule.WatchValue);

				_notificationService.AddErrorToastMessage($"Sweep entries exists for sweepstakes {sweep.Name} and rule for {rule.WatchValue}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			var model = new ActivateSweepRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid,
				ReturnAction = actionName,
				ReturnController = controllerName,
				Rule = _mapper.Map<hrvster.Models.WebModels.Sweeps.Rule>(rule)
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Activate(
			Guid sweepUid, Guid ruleUid, string actionName, string controllerName, ActivateSweepRule form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.AddWarningToastMessage($"Could not find rule with uid: {ruleUid} ");

				return RedirectToAction(actionName, controllerName);
			}

			var entriesExists = await _mediator.Send(new EntriesExistsForSweepsRequest(sweepUid));

			if (entriesExists)
			{
				_logger.LogWarning("Sweep entries exists for sweepstakes with uid: {sweepUid} and name {sweepName} and rule for {watchValue}.",
					sweep.SweepUid,
					sweep.Name,
					rule.WatchValue);

				_notificationService.AddErrorToastMessage($"Sweep entries exists for sweepstakes {sweep.Name} and rule for {rule.WatchValue}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new ActivateSweepRuleRequest(sweepUid, ruleUid));

				_notificationService.AddSuccessToastMessage($"Rule {rule.WatchValue} for sweepstake {sweep.Name} deactivated successfully.");
			}

			return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
		}

		[HttpGet]
		public async Task<IActionResult> Deactivate(
			Guid sweepUid,
			Guid ruleUid,
			string actionName,
			string controllerName)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.AddWarningToastMessage($"Could not find rule with uid: {ruleUid}");

				return RedirectToAction(actionName, controllerName);
			}

			var entriesExists = await _mediator.Send(new EntriesExistsForSweepsRequest(sweepUid));

			if (entriesExists)
			{
				_logger.LogWarning("Sweep entries exists for sweepstakes with uid: {sweepUid} and name {sweepName} and rule for {watchValue}.",
					sweep.SweepUid,
					sweep.Name,
					rule.WatchValue);

				_notificationService.AddErrorToastMessage($"Sweep entries exists for sweepstakes {sweep.Name} and rule for {rule.WatchValue}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			var model = new DeactivateSweepRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid,
				ReturnAction = actionName,
				ReturnController = controllerName,
				Rule = _mapper.Map<hrvster.Models.WebModels.Sweeps.Rule>(rule)
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Deactivate(Guid sweepUid, Guid ruleUid, string actionName, string controllerName, DeactivateSweepRule form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.AddWarningToastMessage($"Could not find rule with uid: {ruleUid} ");

				return RedirectToAction(actionName, controllerName);
			}

			var entriesExists = await _mediator.Send(new EntriesExistsForSweepsRequest(sweepUid));

			if (entriesExists)
			{
				_logger.LogWarning("Sweep entries exists for sweepstakes with uid: {sweepUid} and name {sweepName} and rule for {watchValue}.",
					sweep.SweepUid,
					sweep.Name,
					rule.WatchValue);

				_notificationService.AddErrorToastMessage($"Sweep entries exists for sweepstakes {sweep.Name} and rule for {rule.WatchValue}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new DeactivateSweepRuleRequest(sweepUid, ruleUid));

				_notificationService.AddSuccessToastMessage($"Rule {rule.WatchValue} for sweepstake {sweep.Name} deactivated successfully.");
			}

			return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
		}

		[HttpGet]
		public async Task<IActionResult> DeleteRule(Guid sweepUid, Guid ruleUid, string actionName, string controllerName)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.AddWarningToastMessage($"Could not find rule with uid: {ruleUid} ");

				return RedirectToAction(actionName, controllerName);
			}

			var entriesExists = await _mediator.Send(new EntriesExistsForSweepsAndRuleRequest(ruleUid, sweepUid));

			if (entriesExists)
			{
				_logger.LogWarning("Sweep entries exists for sweepstakes with uid: {sweepUid} and name {sweepName} and rule for {watchValue}.",
					sweep.SweepUid,
					sweep.Name,
					rule.WatchValue);

				_notificationService.AddErrorToastMessage($"Sweep entries exists for sweepstakes {sweep.Name} and rule for {rule.WatchValue}.");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			var model = new DeleteSweepRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeName = sweep.Name,
				SweepstakeUid = sweep.SweepUid,
				ReturnAction = actionName,
				ReturnController = controllerName,
				Rule = _mapper.Map<hrvster.Models.WebModels.Sweeps.Rule>(rule)
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteRule(Guid sweepUid, Guid ruleUid, string returnAction, string returnController, DeleteSweepRule form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return View(form);
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(returnAction ?? "Index", returnController ?? "Sweeps");

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.AddWarningToastMessage($"Could not find rule with uid: {ruleUid} ");

				return View(form);
			}

			var entriesExists = await _mediator.Send(new EntriesExistsForSweepsAndRuleRequest(ruleUid, sweepUid));

			if (entriesExists)
			{
				_logger.LogWarning("Sweep entries exists for sweepstakes with uid: {sweepUid} and name {sweepName} and rule for {watchValue}.",
					sweep.SweepUid,
					sweep.Name,
					rule.WatchValue);

				_notificationService.AddErrorToastMessage($"Sweep entries exists for sweepstakes {sweep.Name} and rule for {rule.WatchValue}.");

				return RedirectToAction(returnAction ?? "View", returnController ?? "SweepRules", new { sweepUid = sweepUid });
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new DeleteSweepTweetRuleRequest(sweepUid, ruleUid, _hrvsterUser.UserUid));

				_notificationService.AddSuccessToastMessage("Rule deleted successfully. The post should have been deleted as well.");

				return RedirectToAction(returnAction, returnController, new
				{
					sweepUid = sweepUid
				});
			}

			return View(form);
		}
	}
}

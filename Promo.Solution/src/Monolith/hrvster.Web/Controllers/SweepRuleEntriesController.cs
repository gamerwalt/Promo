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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class SweepRuleEntriesController : Controller
	{
		private readonly ILogger<SweepRuleEntriesController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly ISessionManager _sessionManager;

		private hrvster.Models.HrvsterUser _hrvsterUser;

		public SweepRuleEntriesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IMapper mapper,
			IToastNotification notificationService,
			ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<SweepRuleEntriesController>();
			_mediator = mediator;
			_mapper = mapper;
			_notificationService = notificationService;
			_sessionManager = sessionManager;
		}

		public async Task<IActionResult> View(Guid sweepUid, Guid ruleUid, string actionName, string controllerName)
		{
			_hrvsterUser = _sessionManager.SetUserInformation();

			var sweepEntries = await _mediator.Send(new GetSweepRuleEntriesRequest(sweepUid, ruleUid));
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			var rule = await _mediator.Send(new GetSweepRuleByUidRequest(ruleUid));

			if (sweep == null)
			{
				_logger.LogWarning("There is no sweeps with the uid {sweepUid}.", sweepUid);
				_notificationService.AddWarningToastMessage($"There is no sweeps with the uid {sweepUid}");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			if (rule == null)
			{
				_logger.LogWarning("There is no sweeps with the uid {ruleUid}.", ruleUid);
				_notificationService.AddWarningToastMessage($"There is no rule with the uid {ruleUid}");

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			var model = new ViewSweepEntries
			{
				SweepEntries = _mapper.Map<IEnumerable<SweepEntry>>(sweepEntries),
				Sweepstake = sweep,
				Rule = rule
			};

			return View(model);
		}
	}
}

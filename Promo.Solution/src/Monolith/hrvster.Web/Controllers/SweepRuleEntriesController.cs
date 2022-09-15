using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models.Options;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
		private readonly INotyfService _notificationService;
		private readonly NotificationServiceOptions _notificationConfiguration;
		private readonly HrvsterUser _hrvsterUser;

		public SweepRuleEntriesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IMapper mapper,
			INotyfService notificationService,
			NotificationServiceOptions notificationServiceOptions,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<SweepRuleEntriesController>();
			_mediator = mediator;
			_mapper = mapper;
			_notificationConfiguration = notificationServiceOptions;
			_notificationService = notificationService;
			_hrvsterUser = hrvsterUser;
		}

		public async Task<IActionResult> View(Guid sweepUid, Guid ruleUid, string actionName, string controllerName)
		{
			var sweepEntries = await _mediator.Send(new GetSweepRuleEntriesRequest(sweepUid, ruleUid));
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			var rule = await _mediator.Send(new GetSweepRuleByUidRequest(ruleUid));

			if(sweep == null)
			{
				_logger.LogWarning("There is no sweeps with the uid {sweepUid}.", sweepUid);
				_notificationService.Warning($"There is no sweeps with the uid {sweepUid}", _notificationConfiguration.DismissInSeconds);

				return RedirectToAction(actionName ?? "View", controllerName ?? "SweepRules", new { sweepUid = sweepUid });
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction(actionName ?? "Index", controllerName ?? "Sweeps");

			if (rule == null)
			{
				_logger.LogWarning("There is no sweeps with the uid {ruleUid}.", ruleUid);
				_notificationService.Warning($"There is no rule with the uid {ruleUid}", _notificationConfiguration.DismissInSeconds);

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

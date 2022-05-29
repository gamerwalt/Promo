using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepRulesController : Controller
	{
		private readonly ILogger<SweepRulesController> _logger;
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;

		public SweepRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator, 
			HrvsterUser hrvsterUser, 
			IMapper mapper)
		{
			_logger = loggerFactory.CreateLogger<SweepRulesController>();
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweepRule = await _mediator.Send(new GetSweepRules(sweepUid));

			return View(sweepRule);
		}

		[HttpGet]
		public async Task<IActionResult> Create(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}
			var watchTypes = await _mediator.Send(new GetWatchTypeRequest());

			var model = new CreateSweepRule
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList(),
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString(),
				Campaign = sweep.Campaign
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Guid sweepUid, CreateSweepRule form)
		{
			if(ModelState.IsValid)
			{
				var request = new CreateSweepRuleRequest(sweepUid, form.SweepstakeId, form.WatchTypeId, form.WatchValue, form.TweetId);

				await _mediator.Send(request);
			}

			return View(form);
		}
	}
}

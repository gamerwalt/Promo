using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Models;
using hrvster.Models.WebModels.Sweeps;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Parameters;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SweepRulesController : Controller
	{
		private readonly ILogger<SweepRulesController> _logger;
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;
		private readonly IWatchTypeProvider _watchTypeProvider;

		public SweepRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator, 
			HrvsterUser hrvsterUser, 
			IMapper mapper,
			IWatchTypeProvider watchTypeProvider)
		{
			_logger = loggerFactory.CreateLogger<SweepRulesController>();
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
			_watchTypeProvider = watchTypeProvider;
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweepRule = await _mediator.Send(new GetSweepRules(sweepUid));

			return View(sweepRule);
		}

		//[HttpGet]
		//public async Task<IActionResult> Create(Guid sweepUid, string actionName, string controllerName)
		//{
		//	var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
		//	if (sweep == null)
		//	{
		//		_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

		//		return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
		//	}
		//	var watchTypes = await _mediator.Send(new GetWatchTypeRequest());

		//	var model = new CreateSweepRule
		//	{
		//		SweepstakeId = sweep.SweepId,
		//		SweepstakeUid = sweep.SweepUid,
		//		SweepstakeName = sweep.Name,
		//		WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList(),
		//		ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
		//		ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString(),
		//		Campaign = sweep.Campaign
		//	};

		//	return View(model);
		//}

		[HttpGet]
		public async Task<IActionResult> SelectProvider(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var watchTypeProviders = await _mediator.Send(new GetWatchTypeProvidersRequest());

			var model = new SelectWatchTypeProvider()
			{
				Providers = watchTypeProviders
					.Where(a => a.ActiveInd)
					.Select(a => new SelectListItem()
					{
						Text = a.WatchTypeProviderName,
						Value = a.WatchTypeProviderCode
					}),
				Sweep = sweep,
				Campaign = sweep.Campaign,
				ReturnAction = actionName,
				ReturnController = controllerName
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SelectProvider(Guid sweepUid, SelectWatchTypeProvider form)
		{
			if(ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
				}

				HttpContext.Session.SetString(SessionConstants.SweepUid, sweepUid.ToString());
				HttpContext.Session.SetString(SessionConstants.ReturnController, form.ReturnController);
				HttpContext.Session.SetString(SessionConstants.ReturnAction, form.ReturnAction);

				var authorizationUrl = await _watchTypeProvider.GetAuthorizationUrl(form.ProviderCode, Request.Scheme, Request.Host.Value, $"/SweepRules/Validate{form.ProviderCode}Auth");

				return new RedirectResult(authorizationUrl);
			}

			return View(form);
		}

		public async Task<IActionResult> ValidateTwitterAuth()
		{
			if(Request.Query["denied"].Count > 0)
			{
				return RedirectToAction("FailedAuthValidation");
			}

			var userDetails = await _watchTypeProvider.IsValidAuthorization(Constants.TwitterProvider, Request.QueryString.Value);

			//update the profile

			var sweepUid = HttpContext.Session.GetString(SessionConstants.SweepUid);
			var returnController = HttpContext.Session.GetString(SessionConstants.ReturnController);
			var returnAction = HttpContext.Session.GetString(SessionConstants.ReturnAction);

			HttpContext.Session.Remove(SessionConstants.SweepUid);
			HttpContext.Session.Remove(SessionConstants.ReturnController);
			HttpContext.Session.Remove(SessionConstants.ReturnAction);

			return RedirectToAction("CreateTweet", 
				new 
				{ 
					sweepUid = sweepUid, 
					actionName = returnAction, 
					controllerName = returnController 
				});
		}

		[HttpGet]
		public async Task<IActionResult> CreateTweet(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				return new NotFoundObjectResult($"Could not find sweepstake with uid: {sweepUid} ");
			}

			var model = new CreateSweepRuleTweet
			{
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString()
			};

			return View(model);
		}
	}
}

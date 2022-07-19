using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Models;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[AutoValidateAntiforgeryToken]
	public class SweepRulesController : Controller
	{
		private string[] AllowedExtensions = {".jpeg", ".jpg", ".png", ".gif"};
		private readonly ILogger<SweepRulesController> _logger;
		private readonly IMediator _mediator;
		private readonly IWatchTypeProvider _watchTypeProvider;
		private readonly Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> _publisherResolver;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMapper _mapper;
		private readonly INotyfService _notificationService;

		public SweepRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IWatchTypeProvider watchTypeProvider,
			Func<ExternalClientType, Core.Clients.Publishers.IExternalClient> publisherResolver,
			HrvsterUser hrvsterUser,
			IMapper mapper,
			INotyfService notificationService)
		{
			_logger = loggerFactory.CreateLogger<SweepRulesController>();
			_mediator = mediator;
			_watchTypeProvider = watchTypeProvider;
			_publisherResolver = publisherResolver;
			_hrvsterUser = hrvsterUser;
			_mapper = mapper;
			_notificationService = notificationService;
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			var sweepRule = await _mediator.Send(new GetSweepRules(sweepUid));

			return View(sweepRule);
		}

		[HttpGet]
		public async Task<IActionResult> DeleteRule(Guid sweepUid, Guid ruleUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if(rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.Warning($"Could not find rule with uid: {ruleUid} ");

				return RedirectToAction(actionName, controllerName);
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
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return View(form);
			}

			var rule = await _mediator.Send(new GetRule(ruleUid));
			if (rule == null)
			{
				_logger.LogError("Could not find the rule with uid: {ruleUid}", ruleUid);

				_notificationService.Warning($"Could not find rule with uid: {ruleUid} ");

				return View(form);
			}

			if (ModelState.IsValid)
			{
				await _mediator.Send(new DeleteSweepTweetRuleRequest(sweepUid, ruleUid, _hrvsterUser.UserUid));

				return RedirectToAction(returnAction, returnController, new
				{
					sweepUid = sweepUid
				});
			}

			return View(form);
		}

		[HttpGet]
		public async Task<IActionResult> SelectProvider(Guid sweepUid, string actionName, string controllerName)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
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
		public async Task<IActionResult> SelectProvider(Guid sweepUid, SelectWatchTypeProvider form)
		{
			if (ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

					return View(form);
				}

				//do we have a profile with access tokens already for this provider?
				//Todo: We should determine if we need to get the access token again
				//var profile = await _mediator.Send(new GetUserProfile(_hrvsterUser.UserUid, ProfileType.Twitter));
				//if (profile != null && !string.IsNullOrWhiteSpace(profile.AccessToken))
				//{
				//	switch (form.ProviderCode)
				//	{
				//		case Constants.TwitterProvider:
				//			return RedirectToAction("CreateTweet",
				//				new
				//				{
				//					sweepUid = sweepUid,
				//					actionName = form.ReturnAction,
				//					controllerName = form.ReturnController
				//				});
				//	}
				//}

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
			if (Request.Query["denied"].Count > 0)
			{
				return RedirectToAction("FailedAuthValidation");
			}

			var userDetails = await _watchTypeProvider.IsValidAuthorization(Constants.TwitterProvider, Request.QueryString.Value);

			_logger.LogInformation("Profile validated for user with uid: {userUid} via {providerType}",
				_hrvsterUser.UserUid,
				ProfileType.Twitter.ToString());

			//update the profile
			await _mediator.Publish(new ProfileValidated(userDetails, _hrvsterUser.UserUid, ProfileType.Twitter));

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

				_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction(actionName, controllerName);
			}

			var watchTypes = await _mediator.Send(new GetWatchTypeRequest());

			var model = new CreateSweepRuleTweet
			{
				WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList(),
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				ReturnAction = actionName ?? HttpContext.Request.RouteValues["action"].ToString(),
				ReturnController = controllerName ?? HttpContext.Request.RouteValues["controller"].ToString(),
				ScheduledDate = sweep.StartDate.AddHours(-1)
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTweet(Guid sweepUid, string returnAction, string returnController, CreateSweepRuleTweet form)
		{
			if (ModelState.IsValid)
			{
				var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
				if (sweep == null)
				{
					_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

					_notificationService.Warning($"Could not find sweepstake with uid: {sweepUid}.");

					return View(form);
				}
				
				var extension = string.Empty;
				if (form.Media1 != null)
				{
					extension = Path.GetExtension(form.Media1.FileName);
					if (!AllowedExtensions.Contains(extension))
					{
						_notificationService.Warning($"Media 1's extension is not allowed.");

						return View(form);
					}
				}
				
				if (form.Media2 != null)
				{
					extension = Path.GetExtension(form.Media2.FileName);
					if (!AllowedExtensions.Contains(extension))
					{
						_notificationService.Warning($"Media 2's extension is not allowed.");

						return View(form);
					}
				}
				
				if(form.Media3 != null)
				{
					extension = Path.GetExtension(form.Media3.FileName);
					if (!AllowedExtensions.Contains(extension))
					{
						_notificationService.Warning($"Media 3's extension is not allowed.");

						return View(form);
					}
				}
				
				if(form.Media4 != null)
				{
					extension = Path.GetExtension(form.Media3.FileName);
					if (!AllowedExtensions.Contains(extension))
					{
						_notificationService.Warning($"Media 4's extension is not allowed.");

						return View(form);
					}
				}

				//upload files
				var media = new List<IFormFile>();
				var mediaLocation = default(IEnumerable<string>);

				if (form.Media1 != null || form.Media1?.Length > 0)
					media.Add(form.Media1);
				if (form.Media2 != null || form.Media2?.Length > 0)
					media.Add(form.Media2);
				if (form.Media3 != null || form.Media3?.Length > 0)
					media.Add(form.Media3);
				if (form.Media4 != null || form.Media4?.Length > 0)
					media.Add(form.Media4);

				if(media.Count > 0)
					mediaLocation = await _mediator.Send(new UploadFileToFileServerRequest(media, _hrvsterUser.UserUid, ProfileType.Twitter));

				var profile = await _mediator.Send(new GetUserProfile(_hrvsterUser.UserUid, ProfileType.Twitter));

				var published = await _mediator.Send(
					new PublishSweepRuleTweetRequest(
						sweep, 
						profile,
						form.WatchTypeId,
						form.WatchValue, 
						form.TweetText, 
						form.PostNow, 
						form.ScheduledDate,
						form.MustBeFollowing,
						mediaLocation));
				
				if(string.IsNullOrWhiteSpace(returnAction))
				{
					return View(form);
				}

				return RedirectToAction(returnAction, returnController, new
				{
					sweepUid = sweepUid
				});
			}

			var watchTypes = await _mediator.Send(new GetWatchTypeRequest());

			form.WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList();

			return View(form);
		}

		//[HttpGet]
		//public async Task<IActionResult> Delete()
		//{

		//}
	}
}

using AutoMapper;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Core;
using hrvster.Core.Requests.WebRequests.Files;
using hrvster.Core.Requests.WebRequests.Publishing;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class CreateTweetController : Controller
	{
		private readonly ILogger<CreateTweetController> _logger;
		private readonly IMediator _mediator;
		private readonly IMapper _mapper;
		private readonly IToastNotification _notificationService;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IWatchTypeProvider _watchTypeProvider;
		private readonly SweepstakeConfiguration _sweepstakeConfiguration;

		public CreateTweetController(
			ILoggerFactory loggerFactory,
			IMediator mediator,
			IMapper mapper,
			IToastNotification notificationService,
			HrvsterUser hrvsterUser,
			IWatchTypeProvider watchTypeProvider,
			SweepstakeConfiguration sweepstakeConfiguration)
		{
			_logger = loggerFactory.CreateLogger<CreateTweetController>();
			_mediator = mediator;
			_mapper = mapper;
			_notificationService = notificationService;
			_hrvsterUser = hrvsterUser;
			_watchTypeProvider = watchTypeProvider;
			_sweepstakeConfiguration = sweepstakeConfiguration;
		}

		[HttpGet]
		[Route("/CreateTweet/SelectProvider/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> SelectProvider(Guid sweepUid)
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
				ReturnAction = "Create",
				ReturnController = "CreateTweet"
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateTweet/SelectProvider/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> SelectProvider(
			Guid sweepUid,
			SelectWatchTypeProvider form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index", "Dashboard");
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });

			if(ModelState.IsValid)
			{
				return RedirectToAction("CreateProfile", new { sweepUid = sweepUid, providerCode = form.ProviderCode });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("SelectProvider", new { sweepUid = sweepUid });
		}

		[HttpGet]
		[Route("/CreateTweet/Profile/Sweep/{sweepUid:Guid}/Provider/{providerCode}")]
		public async Task<IActionResult> CreateProfile(Guid sweepUid, string providerCode)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index", "Dashboard");
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });

			if (string.IsNullOrWhiteSpace(providerCode))
			{
				_notificationService.AddWarningToastMessage("You need to select a provider.");
				return RedirectToAction("SelectProvider", new { sweepUid = sweepUid });
			}

			var watchTypeProviders = await _mediator.Send(new GetWatchTypeProvidersRequest());

			if(watchTypeProviders.Any(c => c.WatchTypeProviderCode == providerCode && c.ActiveInd))
			{
				//for now we know twitter is the only enabled provider
				var profile = await _mediator.Send(new GetUserProfile(_hrvsterUser.UserUid, ProfileType.Twitter));
				if(profile == null)
				{
					//redirect to get the tokens from the provider code
					HttpContext.Session.SetString(SessionConstants.SweepUid, sweepUid.ToString());

					var authorizationUrl = await _watchTypeProvider.GetAuthorizationUrl(providerCode, Request.Scheme, Request.Host.Value, $"/CreateTweet/Validate{providerCode}Auth");

					return new RedirectResult(authorizationUrl);
				}

				return RedirectToAction(nameof(Watch), new { sweepUid = sweepUid });
			}

			_notificationService.AddWarningToastMessage("You have selected an invalid provider.");

			return RedirectToAction("SelectProvider", new { sweepUid = sweepUid });
		}

		[HttpGet]
		public IActionResult FailedAuthValidation()
		{
			//log some information here
			return RedirectToActionPermanent("Index", "Sweeps");
		}

		[HttpGet]
		public async Task<IActionResult> ValidateTwitterAuth()
		{
			if (Request.Query["denied"].Count > 0)
			{
				//log some information here
				return RedirectToAction("FailedAuthValidation");
			}

			var userDetails = await _watchTypeProvider.IsValidAuthorization(Constants.TwitterProvider, Request.QueryString.Value);
			_logger.LogInformation("Profile validated for user with uid: {userUid} via {providerType}",
				_hrvsterUser.UserUid,
				ProfileType.Twitter.ToString());
			
			await _mediator.Publish(new ProfileValidated(userDetails, _hrvsterUser.UserUid, ProfileType.Twitter));

			var sweepUid = HttpContext.Session.GetString(SessionConstants.SweepUid);
			HttpContext.Session.Remove(SessionConstants.SweepUid);

			return RedirectToAction(nameof(Watch), new {sweepUid = sweepUid});
		}

		[HttpGet]
		[Route("/CreateTweet/Watch/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Watch(Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index", "Dashboard");
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });

			var profile = await _mediator.Send(new GetUserProfile(_hrvsterUser.UserUid, ProfileType.Twitter));
			if(profile == null || string.IsNullOrWhiteSpace(profile.AccessToken))
			{
				_notificationService.AddWarningToastMessage("We detected an anomaly. We redirected you to your sweeps page.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			var watchTypes = await _mediator.Send(new GetWatchTypeRequest());

			var model = new CreateSweepRuleTweet
			{
				WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList(),
				SweepstakeId = sweep.SweepId,
				SweepstakeUid = sweep.SweepUid,
				SweepstakeName = sweep.Name,
				SweepstakeStartDate = sweep.StartDate,
				SweepstakeEndDate = sweep.EndDate,
				SweepstakeDrawDate = sweep.DrawDate,
				ScheduledDate = sweep.StartDate.AddMinutes(-_sweepstakeConfiguration.TweetScheduleDefaultMinutes)
			};

			return View(model);
		}

		[HttpPost]
		[Route("/CreateTweet/Watch/Sweep/{sweepUid:Guid}")]
		public async Task<IActionResult> Watch(Guid sweepUid, CreateSweepRuleTweet form)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
			{
				_logger.LogError("Could not find sweepstake with uid: {sweepUid}", sweepUid);

				_notificationService.AddWarningToastMessage($"Could not find sweepstake with uid: {sweepUid}.");

				return RedirectToAction("Index", "Dashboard");
			}

			if (sweep.Account.AccountUid != _hrvsterUser.UserUid)
				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });

			var profile = await _mediator.Send(new GetUserProfile(_hrvsterUser.UserUid, ProfileType.Twitter));
			if (profile == null || string.IsNullOrWhiteSpace(profile.AccessToken))
			{
				_notificationService.AddWarningToastMessage("We detected an anomaly. We redirected you to your sweeps page.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			if(ModelState.IsValid)
			{
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

				if (media.Count > 0)
					mediaLocation = await _mediator.Send(new UploadFileToFileServerRequest(media, _hrvsterUser.UserUid, ProfileType.Twitter, _hrvsterUser.Token));

				var published = await _mediator.Send(
					new PublishSweepRuleTweetRequest(
						sweep,
						profile,
						form.WatchTypeId,
						form.WatchValue,
						form.TweetText,
						form.PostNow,
						form.ScheduledDate,
						true,
						mediaLocation));
				var word = form.PostNow ? "created" : "scheduled";

				_notificationService.AddSuccessToastMessage($"Tweet {word} successfully.");

				return RedirectToAction("View", "Sweeps", new { sweepUid = sweepUid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			var watchTypes = await _mediator.Send(new GetWatchTypeRequest());
			form.WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList();
			form.SweepstakeUid = sweep.SweepUid;
			form.ScheduledDate = sweep.StartDate.AddMinutes(-_sweepstakeConfiguration.TweetScheduleDefaultMinutes);

			return View(form);
		}
	}
}

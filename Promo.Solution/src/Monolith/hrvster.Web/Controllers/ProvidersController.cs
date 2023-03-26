using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	[ShouldBeSubscribed]
	public class ProvidersController : Controller
	{
		private readonly ILogger<ProvidersController> _logger;
		private readonly ISessionManager _sessionManager;
		private readonly IToastNotification _notificationService;
		private readonly IWatchTypeProvider _watchTypeProvider;
		private readonly IProvidersService _providersService;
		private readonly IAccountsService _accountsService;
		private readonly IGameService _gameService;

		private HrvsterUser _hrvsterUser;

		public ProvidersController(
			ILoggerFactory loggerFactory,
			ISessionManager sessionManager,
			IToastNotification notificationService,
			IWatchTypeProvider watchTypeProvider,
			IProvidersService providersService,
			IAccountsService accountsService,
			IGameService gameService)
		{
			_logger = loggerFactory.CreateLogger<ProvidersController>();
			_sessionManager = sessionManager;
			_notificationService = notificationService;
			_watchTypeProvider = watchTypeProvider;
			_providersService = providersService;
			_accountsService = accountsService;
			_gameService = gameService;
		}

		[HttpGet]
		[Route("/providers/select")]
		public async Task<IActionResult> SelectProvider(Guid uid, string actionName, string controllerName, string gameType)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var watchTypeProviders = await _providersService.GetWathcTypeProvidersAsync();

			var model = new SelectWatchTypeProvider()
			{
				Uid = uid,
				Providers = watchTypeProviders
					.Where(a => a.ActiveInd)
					.Select(a => new SelectListItem()
					{
						Text = a.WatchTypeProviderName,
						Value = a.WatchTypeProviderCode
					}),
				ReturnAction = actionName,
				ReturnController = controllerName,
				GameType = gameType
			};

			return View(model);
		}

		[HttpPost]
		[Route("/providers/select")]
		public async Task<IActionResult> SelectProvider(Guid uid, string actionName, string controllerName, string gameType, SelectWatchTypeProvider form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var watchTypeProviders = await _providersService.GetWathcTypeProvidersAsync();

			if(ModelState.IsValid)
			{
				var profile = await _accountsService.GetProfileAsync(_hrvsterUser.UserUid, ProfileType.Twitter);

				if (profile != null && !string.IsNullOrWhiteSpace(profile.AccessToken))
				{
					switch (form.ProviderCode)
					{
						case Constants.TwitterProvider:
							return RedirectToAction("CreateTweet",
								new
								{
									Uid = uid,
									actionName = actionName,
									controllerName = controllerName,
									gameType = gameType
								});
					}
				}

				HttpContext.Session.SetString(SessionConstants.Uid, uid.ToString());
				HttpContext.Session.SetString(SessionConstants.ReturnController, controllerName);
				HttpContext.Session.SetString(SessionConstants.ReturnAction, actionName);
				HttpContext.Session.SetString(SessionConstants.GameType, gameType);

				var authorizationUrl = await _watchTypeProvider.GetAuthorizationUrl(form.ProviderCode, Request.Scheme, Request.Host.Value, $"/SweepRules/Validate{form.ProviderCode}Auth");

				return new RedirectResult(authorizationUrl);
			}

			var model = new SelectWatchTypeProvider()
			{
				Uid = uid,
				Providers = watchTypeProviders
					.Where(a => a.ActiveInd)
					.Select(a => new SelectListItem()
					{
						Text = a.WatchTypeProviderName,
						Value = a.WatchTypeProviderCode
					}),
				ReturnAction = actionName,
				ReturnController = controllerName,
				GameType = gameType
			};

			return View(model);
		}

		public IActionResult FailedAuthValidation()
		{
			return RedirectToActionPermanent("Index", "MatchGames");
		}

		public async Task<IActionResult> ValidateTwitterAuth()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			if (Request.Query["denied"].Count > 0)
			{
				return RedirectToAction("FailedAuthValidation");
			}

			var userDetails = await _watchTypeProvider.IsValidAuthorization(Constants.TwitterProvider, Request.QueryString.Value);

			_logger.LogInformation("Profile validated for user with uid: {userUid} via {providerType}",
				_hrvsterUser.UserUid,
				ProfileType.Twitter.ToString());

			//update the profile
			await _accountsService.RaiseProfileValidatedEventAsync(userDetails, _hrvsterUser.UserUid, ProfileType.Twitter);

			var Uid = HttpContext.Session.GetString(SessionConstants.Uid);
			var returnController = HttpContext.Session.GetString(SessionConstants.ReturnController);
			var returnAction = HttpContext.Session.GetString(SessionConstants.ReturnAction);
			var gameType = HttpContext.Session.GetString(SessionConstants.GameType);

			HttpContext.Session.Remove(SessionConstants.Uid);
			HttpContext.Session.Remove(SessionConstants.ReturnController);
			HttpContext.Session.Remove(SessionConstants.ReturnAction);
			HttpContext.Session.Remove(SessionConstants.GameType);

			return RedirectToAction("CreateTweet",
				new
				{
					Uid = Uid,
					actionName = returnAction ?? "View",
					controllerName = returnController ?? "MatchGames", //Use game type here to properly do the controllerName,
					gameType = gameType
				});
		}

		[HttpGet]
		[Route("/providers/create/tweet")]
		public async Task<IActionResult> CreateTweet(Guid Uid, string actionName, string controllerName, string gameType)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var watchTypes = await _providersService.GetWatchTypesAsync();

			var gameDetail = await _gameService.GetGameDetailsAsync(gameType, Uid);

			if (gameDetail == null)
			{
				_logger.LogWarning("Unable to get game details for the uid {uid} and the game type {gameType}", Uid, gameType);

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			var model = GetCreateRuleTweet(Uid, actionName, controllerName, gameType, watchTypes, gameDetail);

			return View(model);
		}

		[HttpPost]
		[Route("/providers/create/tweet")]
		public async Task<IActionResult> CreateTweet(Guid Uid, string actionName, string controllerName, string gameType, CreateRuleTweet form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var watchTypes = await _providersService.GetWatchTypesAsync();

			var gameDetail = await _gameService.GetGameDetailsAsync(gameType, Uid);

			if (gameDetail == null)
			{
				_logger.LogWarning("Unable to get game details for the uid {uid} and the game type {gameType}", Uid, gameType);

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			if(ModelState.IsValid)
			{
				_notificationService.AddSuccessToastMessage("Publishing and saving the tweet rule for this game.");

				await _gameService.CreateRuleTweetAsync(
					_hrvsterUser,
					gameType, 
					Uid, 
					form.GameId, 
					form.WatchTypeId, 
					form.WatchValue, 
					form.TweetText, 
					form.PostNow, 
					form.ScheduledDate, 
					form.Media1, 
					form.Media2, 
					form.Media3, 
					form.Media4,
					form.MustBeFollowing);

				_notificationService.AddSuccessToastMessage("Tweet published successfully.");

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			var model = GetCreateRuleTweet(Uid, actionName, controllerName, gameType, watchTypes, gameDetail);

			return View(model);
		}

		private static CreateRuleTweet GetCreateRuleTweet(Guid Uid, string actionName, string controllerName, string gameType, IEnumerable<hrvster.Models.WebModels.WatchType> watchTypes, GameDetail gameDetail)
		{
			return new CreateRuleTweet
			{
				WatchTypes = watchTypes.Select(a => new SelectListItem() { Text = a.WatchTypeName, Value = a.WatchTypeId.ToString() }).ToList(),
				GameUid = Uid,
				ActionName = actionName,
				ControllerName = controllerName,
				ScheduledDate = gameDetail.StartDate.AddMinutes(-4),
				GameType = gameType,
				GameName = gameDetail?.Name,
				StartDate = gameDetail.StartDate,
				EndDate = gameDetail.EndDate,
				GameId = gameDetail.Id
			};
		}
	}
}

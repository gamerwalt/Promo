using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.ApplicationServices.Harvesters.Facebook;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Core.Subscriptions;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
		private readonly IFacebookPageSerivce _facebookPageService;
		private readonly FacebookConfigOptions _facebookOptions;
		private readonly HarvesterSettingsOptions _hrvsterOptions;
		private readonly IRebrandlyService _rebrandlyService;
		private readonly IHyperlinkService _hyperLinkService;
		private readonly ITwitterHttpService _twitterHttpService;

		private HrvsterUser _hrvsterUser;

		public ProvidersController(
			ILoggerFactory loggerFactory,
			ISessionManager sessionManager,
			IToastNotification notificationService,
			IWatchTypeProvider watchTypeProvider,
			IProvidersService providersService,
			IAccountsService accountsService,
			IGameService gameService,
			IFacebookPageSerivce facebookPageSerivce,
			FacebookConfigOptions facebookOptions,
			HarvesterSettingsOptions hrvsterOptions,
			IRebrandlyService rebrandlyService,
			IHyperlinkService hyperLinkService,
			ITwitterHttpService twitterHttpService)
		{
			_logger = loggerFactory.CreateLogger<ProvidersController>();
			_sessionManager = sessionManager;
			_notificationService = notificationService;
			_watchTypeProvider = watchTypeProvider;
			_providersService = providersService;
			_accountsService = accountsService;
			_gameService = gameService;
			_facebookPageService = facebookPageSerivce;
			_facebookOptions = facebookOptions;
			_hrvsterOptions = hrvsterOptions;
			_rebrandlyService = rebrandlyService;
			_hyperLinkService = hyperLinkService;
			_twitterHttpService = twitterHttpService;
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

			if (ModelState.IsValid)
			{
				HttpContext.Session.SetString(SessionConstants.Uid, uid.ToString());
				HttpContext.Session.SetString(SessionConstants.ReturnController, controllerName);
				HttpContext.Session.SetString(SessionConstants.ReturnAction, actionName);
				HttpContext.Session.SetString(SessionConstants.GameType, gameType);

				var profile = await _accountsService.GetProfileAsync(_hrvsterUser.UserUid, ProfileType.Twitter);
				if (profile == null || DateTime.Now >= profile.UpdatedDate.AddHours(2))
				{
					var url = _twitterHttpService.GetAuthorizationUrl(Request.Scheme, Request.Host.Value, $"/Providers/Validate{form.ProviderCode}Auth");

					return new RedirectResult(url);
				}

				_sessionManager.SetKeyValue(SessionConstants.ExternalClientType, ExternalClientType.Twitter.ToString());

				return RedirectToAction("CreatePost",
				new
				{
					Uid = uid,
					actionName = actionName ?? "View",
					controllerName = controllerName ?? "MatchGames", //Use game type here to properly do the controllerName,
					gameType = gameType
				});
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
			var uid = HttpContext.Session.GetString(SessionConstants.Uid);
			var controllerName = HttpContext.Session.GetString(SessionConstants.ReturnController);
			var actionName = HttpContext.Session.GetString(SessionConstants.ReturnAction);
			var gameType = HttpContext.Session.GetString(SessionConstants.GameType);

			return RedirectToActionPermanent(actionName, controllerName, GetReturnObject(gameType, uid));
		}

		public async Task<IActionResult> ValidateTwitterAuth()
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var Uid = HttpContext.Session.GetString(SessionConstants.Uid);
			var returnController = HttpContext.Session.GetString(SessionConstants.ReturnController);
			var returnAction = HttpContext.Session.GetString(SessionConstants.ReturnAction);
			var gameType = HttpContext.Session.GetString(SessionConstants.GameType);
			var code = Request.Query["code"].ToString();

			if (Request.Query["error"].Count > 0)
			{
				return RedirectToActionPermanent(returnAction, returnController, GetReturnObject(gameType, Uid));
			}

			var profile = await _accountsService.GetProfileAsync(_hrvsterUser.UserUid, ProfileType.Twitter);
			if (profile == null || DateTime.Now >= profile.UpdatedDate.AddHours(2))
			{
				var accessTokenObject = await _twitterHttpService.GetAccessTokenAsyncForOfflineAccess(
				code, Request.Scheme, Request.Host.Value, $"/Providers/ValidateTWITTERAuth");

				var userDetails = await _twitterHttpService.GetAuthenticatedUserAsync(accessTokenObject);

				_logger.LogInformation("Profile validated for user with uid: {userUid} via {providerType}",
					_hrvsterUser.UserUid,
					ProfileType.Twitter.ToString());

				//update the profile
				await _accountsService.RaiseProfileValidatedEventAsync(userDetails, _hrvsterUser.UserUid, ProfileType.Twitter);
			}

			HttpContext.Session.Remove(SessionConstants.Uid);
			HttpContext.Session.Remove(SessionConstants.ReturnController);
			HttpContext.Session.Remove(SessionConstants.ReturnAction);
			HttpContext.Session.Remove(SessionConstants.GameType);

			_sessionManager.SetKeyValue(SessionConstants.ExternalClientType, ExternalClientType.Twitter.ToString());

			if (string.IsNullOrWhiteSpace(returnController) ||
				string.IsNullOrWhiteSpace(gameType) ||
				string.IsNullOrWhiteSpace(returnAction) ||
				string.IsNullOrWhiteSpace(Uid))
			{
				return RedirectToAction("Index", "Dashboard");
			}

			return RedirectToAction("CreatePost",
				new
				{
					Uid = Uid,
					actionName = returnAction ?? "View",
					controllerName = returnController ?? "MatchGames", //Use game type here to properly do the controllerName,
					gameType = gameType
				});
		}

		public async Task<IActionResult> ValidateFBAuth(string code, string state, string error, string error_description)
		{
			//_hrvsterUser = _sessionManager.GetUserInformation();

			//var Uid = HttpContext.Session.GetString(SessionConstants.Uid);
			//var returnController = HttpContext.Session.GetString(SessionConstants.ReturnController);
			//var returnAction = HttpContext.Session.GetString(SessionConstants.ReturnAction);
			//var gameType = HttpContext.Session.GetString(SessionConstants.GameType);

			//if (!string.IsNullOrWhiteSpace(error))
			//{
			//	_logger.LogError("Facebook authentication failed for user with uid {userUid} with description {errorDescription}",
			//		_hrvsterUser.UserUid,
			//		error_description);

			//	return RedirectToActionPermanent(returnAction, returnController, GetReturnObject(gameType, Uid));
			//}

			//var redirectUri = await _watchTypeProvider.GetCallbackUrl(Request.Scheme, Request.Host.Value, $"/Providers/Validate{Constants.FaceBookProvider}Auth");

			//_facebookClient.AppId = _facebookOptions.AppId;
			//_facebookClient.AppSecret = _facebookOptions.AppSecret;
			//dynamic tokenResult = await _facebookClient.GetTaskAsync("/oauth/access_token",
			//	new
			//	{
			//		client_id = _facebookOptions.AppId,
			//		client_secret = _facebookOptions.AppSecret,
			//		code,
			//		redirect_uri = redirectUri
			//	});

			//var accessToken = tokenResult.access_token;

			//_facebookClient.AccessToken = accessToken;

			//var appSecretProof = GenerateAppSecretProof(_facebookClient.AppSecret, accessToken);

			//dynamic user = await _facebookClient.GetTaskAsync("me", new { fields = "id,name,email,location,link,picture.type(large)", appsecret_proof = appSecretProof });

			//var userDict = (IDictionary<string, object>)user;

			//var userDetails = new UserProviderDetails()
			//{
			//	AccessToken = accessToken.ToString(),
			//	Location = user.location != null ? user.location.name : null,
			//	Name = user.name,
			//	ProfileImage = user.picture.data.url,
			//	ScreenName = user.name,
			//	ExternalProfileId = userDict["id"].ToString(),
			//	Description = user.name,
			//	AccessTokenSecret = string.Empty,
			//	BearerToken = string.Empty,
			//	ConsumerKey = string.Empty,
			//	ConsumerSecret = string.Empty
			//};

			//_logger.LogInformation("Profile validated for user with uid: {userUid} via {providerType}",
			//	_hrvsterUser.UserUid,
			//	ProfileType.Twitter.ToString());

			////update the profile
			//await _accountsService.RaiseProfileValidatedEventAsync(userDetails, _hrvsterUser.UserUid, ProfileType.Facebook);

			//HttpContext.Session.Remove(SessionConstants.Uid);
			//HttpContext.Session.Remove(SessionConstants.ReturnController);
			//HttpContext.Session.Remove(SessionConstants.ReturnAction);
			//HttpContext.Session.Remove(SessionConstants.GameType);

			//_sessionManager.SetKeyValue(SessionConstants.ExternalClientType, ExternalClientType.Facebook.ToString());

			//if (string.IsNullOrWhiteSpace(returnController) ||
			//	string.IsNullOrWhiteSpace(gameType) ||
			//	string.IsNullOrWhiteSpace(returnAction) ||
			//	string.IsNullOrWhiteSpace(Uid))
			//{
			//	return RedirectToAction("Index", "Dashboard");
			//}

			//dynamic pages = await _facebookClient.GetTaskAsync("/me/accounts");
			//var facebookPages = new List<FacebookPage>();

			//foreach (var page in pages.data)
			//{
			//	facebookPages.Add(new FacebookPage
			//	{
			//		Id = page.id,
			//		Name = page.name,
			//		PageAccessToken = page.access_token,
			//	});
			//}

			//if(facebookPages.Count <= 0)
			//{
			//	_notificationService.AddErrorToastMessage("You have to select a page for us to post to.");

			//	return RedirectToAction("SelectProvider", new
			//	{
			//		uid = Uid,
			//		actionName = returnAction,
			//		controllerName = returnController,
			//		gameType = gameType
			//	});
			//}

			//if (facebookPages.Count > 0)
			//{
			//	//we need to save this
			//	await _facebookPageService.CreateAsync(_hrvsterUser.UserUid, facebookPages);
			//}

			////if the facebookPages.Count > 1, then we need the user to select which page first
			////else, we just go ahead and createPost. Save the details in the session first
			//if(facebookPages.Count > 1)
			//	return RedirectToAction("SelectPage",
			//		new
			//		{
			//			Uid = Uid,
			//			actionName = returnAction,
			//			controllerName = returnController,
			//			gameType = gameType
			//		});

			//_sessionManager.SetKeyValue(SessionConstants.PageId, facebookPages.FirstOrDefault()?.Id);

			//return RedirectToAction("CreatePost",
			//	new
			//	{
			//		Uid = Uid,
			//		actionName = returnAction,
			//		controllerName = returnController,
			//		gameType = gameType
			//	});

			return new OkObjectResult(new { message = "Authenticate via Facebook." });
		}

		[HttpGet]
		[Route("/providers/pages/select")]
		public async Task<IActionResult> SelectPage(
			Guid Uid,
			string actionName,
			string controllerName,
			string gameType)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var gameDetail = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), Uid);
			var pages = await _facebookPageService.GetPagesAsync(_hrvsterUser.UserUid);

			var model = new SelectPageToPost
			{
				GameName = gameDetail.Name,
				ActionName = actionName,
				ControllerName = controllerName,
				GameUid = Uid,
				Pages = pages.Select(a => new SelectListItem()
				{
					Text = a.Name,
					Value = a.Id
				}).ToList(),
				GameType = gameType
			};

			return View(model);
		}

		[HttpPost]
		[Route("/providers/pages/select")]
		public async Task<IActionResult> SelectPage(
			Guid Uid,
			string actionName,
			string controllerName,
			string gameType,
			SelectPageToPost form)
		{
			_hrvsterUser = _sessionManager.GetUserInformation();

			var gameDetail = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), Uid);

			if (ModelState.IsValid)
			{
				_sessionManager.SetKeyValue(SessionConstants.PageId, form.PageId);

				return RedirectToAction("CreatePost", new
				{
					Uid = Uid,
					actionName = actionName,
					controllerName = controllerName,
					gameType = gameType
				});
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("SelectPage", new
			{
				Uid = Uid,
				actionName = actionName,
				controllerName = controllerName,
				gameType = gameType
			});
		}

		[HttpGet]
		[Route("/providers/create/post")]
		public async Task<IActionResult> CreatePost(Guid Uid, string actionName, string controllerName, string gameType)
		{
			if (string.IsNullOrWhiteSpace(gameType) ||
				string.IsNullOrWhiteSpace(actionName) ||
				string.IsNullOrWhiteSpace(controllerName))
			{
				return RedirectToAction("index", "Dashboard");
			}

			_hrvsterUser = _sessionManager.GetUserInformation();

			var watchTypes = await _providersService.GetWatchTypesAsync();

			var gameDetail = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), Uid);

			if (gameDetail == null)
			{
				_logger.LogWarning("Unable to get game details for the uid {uid} and the game type {gameType}", Uid, gameType);

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			var model = GetCreateRulePost(Uid, actionName, controllerName, gameType, watchTypes, gameDetail);

			model.PageId = _sessionManager.GetValue(SessionConstants.PageId);

			var externalClientType = _sessionManager.GetValue(SessionConstants.ExternalClientType);

			if (string.IsNullOrWhiteSpace(externalClientType))
			{
				_logger.LogWarning("Unable to determine the external client type required.");

				_notificationService.AddWarningToastMessage("Unable to determine the external client type requried.");

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			return View(model);
		}

		[HttpPost]
		[Route("/providers/create/post")]
		public async Task<IActionResult> CreatePost(Guid Uid, string actionName, string controllerName, string gameType, CreateRulePost form)
		{

			if (string.IsNullOrWhiteSpace(gameType) ||
				string.IsNullOrWhiteSpace(actionName) ||
				string.IsNullOrWhiteSpace(controllerName))
			{
				return RedirectToAction("index", "Dashboard");
			}

			_hrvsterUser = _sessionManager.GetUserInformation();

			var watchTypes = await _providersService.GetWatchTypesAsync();

			var gameDetail = await _gameService.GetGameDetailsAsync(Enum.Parse<GameTypesEnum>(gameType), Uid);

			if (gameDetail == null)
			{
				_logger.LogWarning("Unable to get game details for the uid {uid} and the game type {gameType}", Uid, gameType);

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			if (ModelState.IsValid)
			{
				var ruleUid = Guid.NewGuid();

				var externalClientType = _sessionManager.GetValue(SessionConstants.ExternalClientType);

				if (string.IsNullOrWhiteSpace(externalClientType))
				{
					_logger.LogWarning("Unable to determine the external client type required.");

					_notificationService.AddWarningToastMessage("Unable to determine the external client type requried.");

					return RedirectToAction(actionName, controllerName, new { uid = Uid });
				}

				var companyGameLink = await GetAccountCompanyLink(_hrvsterUser.UserUid, gameType, Uid);

				var link = await _rebrandlyService.GenerateShortLinkAsync($"{companyGameLink}?externalClient={externalClientType}&ruleUid={ruleUid}");
				var urlLink = $"https://{link}";

				form.Post = $"{form.Post}{Environment.NewLine}{urlLink}";

				await _gameService.CreateRulePostAsync(
					ruleUid,
					urlLink,
					externalClientType,
					_hrvsterUser,
					gameType,
					Uid,
					form.GameId,
					form.WatchTypeId,
					form.WatchValue,
					form.Post,
					form.PostNow,
					form.ScheduledDate,
					form.Media1,
					form.Media2,
					form.Media3,
					form.Media4,
					form.PageId ?? _sessionManager.GetValue(SessionConstants.PageId),
					true);

				_notificationService.AddSuccessToastMessage("Post published successfully.");

				return RedirectToAction(actionName, controllerName, new { uid = Uid });
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			var model = GetCreateRulePost(Uid, actionName, controllerName, gameType, watchTypes, gameDetail);

			return View(model);
		}

		private async Task<string> GetAccountCompanyLink(Guid userUid, string gameType, Guid gameUid)
		{
			var companyLink = await _hyperLinkService.GetCompanyHyperlinkAsync(userUid);
			var gameLink = string.Empty;

			switch (gameType)
			{
				case GameTypes.Sweepstake:
					gameLink = $"{companyLink}{GameUrlTypes.SweepGame}";
					break;
				case GameTypes.MatchGame:
					gameLink = $"{companyLink}{GameUrlTypes.MatchGame}";
					break;
				default:
					gameLink = $"{companyLink}{GameUrlTypes.SweepGame}";
					break;
			}

			return gameLink.Replace(GameUrlTypeReplaceInformation.ReplaceableGameUid, $"{gameUid}");
		}

		private static CreateRulePost GetCreateRulePost(Guid Uid, string actionName, string controllerName, string gameType, IEnumerable<hrvster.Models.WebModels.WatchType> watchTypes, GameDetail gameDetail)
		{
			return new CreateRulePost
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

		private static string GenerateAppSecretProof(string appSecret, string accessToken)
		{
			using var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(appSecret));
			byte[] hash = hmac.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}

		private static object GetReturnObject(string gameType, string uid)
		{
			if (gameType == GameTypes.Sweepstake.ToString())
			{
				return new { SweepUid = uid };
			}
			else if (gameType == GameTypes.MatchGame.ToString())
			{
				return new { Uid = uid };
			}

			return new { Uid = uid };
		}
	}
}

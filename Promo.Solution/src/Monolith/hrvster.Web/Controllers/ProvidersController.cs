using hrvster.ApplicationServices;
using hrvster.ApplicationServices.Accounts;
using hrvster.Core.Authentication;
using hrvster.Core.Clients;
using hrvster.Models;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;

namespace hrvster.Web.Controllers
{
	public class ProvidersController : Controller
	{
		private readonly ILogger<ProvidersController> _logger;
		private readonly ISessionManager _sessionManager;
		private readonly IToastNotification _notificationService;
		private readonly IWatchTypeProvider _watchTypeProvider;
		private readonly IProvidersService _providersService;
		private readonly IAccountsService _accountsService;

		private HrvsterUser _hrvsterUser;

		public ProvidersController(
			ILoggerFactory loggerFactory,
			ISessionManager sessionManager,
			IToastNotification notificationService,
			IWatchTypeProvider watchTypeProvider,
			IProvidersService providersService,
			IAccountsService accountsService)
		{
			_logger = loggerFactory.CreateLogger<ProvidersController>();
			_sessionManager = sessionManager;
			_notificationService = notificationService;
			_watchTypeProvider = watchTypeProvider;
			_providersService = providersService;
			_accountsService = accountsService;
		}

		[HttpGet]
		[Route("/providers/select")]
		public async Task<IActionResult> SelectProvider(Guid uid, string actionName, string controllerName)
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
				ReturnController = controllerName
			};

			return View(model);
		}

		[HttpPost]
		[Route("/providers/select")]
		public async Task<IActionResult> SelectProvider(Guid uid, string actionName, string controllerName, SelectWatchTypeProvider form)
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
									sweepUid = uid,
									actionName = actionName,
									controllerName = controllerName
								});
					}
				}

				HttpContext.Session.SetString(SessionConstants.Uid, uid.ToString());
				HttpContext.Session.SetString(SessionConstants.ReturnController, controllerName);
				HttpContext.Session.SetString(SessionConstants.ReturnAction, actionName);

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
				ReturnController = controllerName
			};

			return View(model);
		}
	}
}

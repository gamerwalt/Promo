using Firebase.Auth;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NodaTime.TimeZones;
using NToastNotify;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class SessionController : Controller
	{
		private readonly ILogger<SessionController> _logger;
		private readonly IToastNotification _notificationService;
		private readonly IFirebaseAuthProvider _firebaseAuthProvider;
		private readonly ISessionManager _sessionManager;
		private readonly IMediator _mediator;
		private readonly HrvsterUser _hrvsterUser;

		public SessionController(
			ILoggerFactory loggerFactory,
			IToastNotification notificationService,
			IFirebaseAuthProvider firebaseAuthProvider,
			ISessionManager sessionManager,
			IMediator mediator,
			HrvsterUser hrvsterUser)
		{
			_logger = loggerFactory.CreateLogger<SessionController>();
			_notificationService = notificationService;
			_firebaseAuthProvider = firebaseAuthProvider;
			_sessionManager = sessionManager;
			_mediator = mediator;
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		public async Task<IActionResult> Denied(string returnUrl)
		{
			return RedirectToAction("SignIn", new { returnUrl = returnUrl });
		}

		[HttpGet]
		public ActionResult SignIn(string returnUrl)
		{
			var token = HttpContext.Session.GetString("_UserToken");

			return token == null ? View() :
				string.IsNullOrWhiteSpace(returnUrl) ?
					RedirectToAction("Index", "Dashboard") :
					Redirect(returnUrl);
		}

		// POST: SessionController/Create
		[HttpPost]
		public async Task<ActionResult> SignIn(SigninForm form, string returnUrl)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var link = await _firebaseAuthProvider.SignInWithEmailAndPasswordAsync(form.Email.Trim(), form.Password.Trim());

					if (!string.IsNullOrWhiteSpace(link.FirebaseToken))// && link.User.IsEmailVerified)
					{
						if (!link.User.IsEmailVerified)
						{
							return RedirectToAction("Index", "VerifyEmail");
						}

						var account = await _mediator.Send(new GetAccountAndProfileRequest(Guid.Parse(link.User.LocalId)));

						if(account == null)
						{
							_logger.LogError("Error: Unable to load account for {userUid} after signing in.", link.User.LocalId);

							_notificationService.AddErrorToastMessage("An error occurred. We are currently looking into this.");

							return RedirectToAction("SignIn");
						}

						_sessionManager.SetKeyValue(SessionConstants.UserToken, link.FirebaseToken);
						_sessionManager.SetKeyValue(SessionConstants.UserUid, link.User.LocalId);
						_sessionManager.SetKeyValue(SessionConstants.UserEmail, link.User.Email);
						_sessionManager.SetKeyValue(SessionConstants.UserDisplayName, link.User.DisplayName);
						_sessionManager.SetKeyValue(SessionConstants.Locale, account.Locale);

						var source = TzdbDateTimeZoneSource.Default;

						var timezone = TzdbDateTimeZoneSource.Default.ZoneLocations
							.Where(x => x.CountryName == account.Locale)
							.Select(tz => source.WindowsMapping.MapZones.FirstOrDefault(x => x.TzdbIds.Contains(source.CanonicalIdMap.First(y => y.Value == tz.ZoneId).Key)))
							.Where(x => x != null)
							.Select(x => x.WindowsId)
							.Distinct().FirstOrDefault();

						_sessionManager.SetKeyValue(SessionConstants.WindowsLocale, timezone);

						var isDaylight = TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now);
						_sessionManager.SetKeyValue(SessionConstants.IsDaylight, isDaylight.ToString());

						_hrvsterUser.UserUid = Guid.Parse(link.User.LocalId);
						_hrvsterUser.DisplayName = link.User.DisplayName;
						_hrvsterUser.Token = link.FirebaseToken;
						_hrvsterUser.EmailAddress = link.User.Email;
						_hrvsterUser.Locale = account.Locale;
						_hrvsterUser.WindowsTimeZone = timezone;
						_hrvsterUser.IsDaylight = isDaylight;

						await _mediator.Send(new UpdateLastLoginRequest(link.User.LocalId));

						if (string.IsNullOrWhiteSpace(returnUrl))
							return RedirectToActionPermanent(actionName: "Index", controllerName: "Dashboard");
						else
							return Redirect(returnUrl);
					}
				}
				catch (FirebaseAuthException ex)
				{
					var firebaseError = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);

					_logger.LogWarning("A Firebase error has occurred during logon. Error: {firebaseError}", firebaseError.ToJsonString());

					_notificationService.AddErrorToastMessage("Invalid email or password. Please try again.");

					return RedirectToAction("SignIn");
				}
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return RedirectToAction("SignIn");
		}

		[HttpPost]
		public async Task<ActionResult> SignOut()
		{
			var accountUid = _sessionManager.GetValue(SessionConstants.UserUid);

			if(!string.IsNullOrWhiteSpace(accountUid))
			{
				await _mediator.Publish(new AccountLoggedOut(accountUid));
			}

			_sessionManager.RemoveKeyValue(SessionConstants.UserToken);
			_sessionManager.RemoveKeyValue(SessionConstants.UserUid);
			_sessionManager.RemoveKeyValue(SessionConstants.UserEmail);

			return RedirectToAction("SignIn");
		}
	}
}

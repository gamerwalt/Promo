using Firebase.Auth;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.Extensions;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NToastNotify;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class SessionController : Controller
	{
		private readonly ILogger<SessionController> _logger;
		private readonly IToastNotification _notificationService;
		private readonly IFirebaseAuthProvider _firebaseAuthProvider;

		public SessionController(
			ILoggerFactory loggerFactory,
			IToastNotification notificationService,
			IFirebaseAuthProvider firebaseAuthProvider)
		{
			_logger = loggerFactory.CreateLogger<SessionController>();
			_notificationService = notificationService;
			_firebaseAuthProvider = firebaseAuthProvider;
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

						HttpContext.Session.SetString(SessionConstants.UserToken, link.FirebaseToken);
						HttpContext.Session.SetString(SessionConstants.UserUid, link.User.LocalId);
						HttpContext.Session.SetString(SessionConstants.UserEmail, link.User.Email);
						HttpContext.Session.SetString(SessionConstants.UserDisplayName, link.User.DisplayName);

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

		public async Task<ActionResult> SignOut()
		{
			HttpContext.Session.Remove("_UserToken");

			return RedirectToAction("SignIn");
		}
	}
}

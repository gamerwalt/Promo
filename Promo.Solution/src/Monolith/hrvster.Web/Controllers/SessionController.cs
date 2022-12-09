using Firebase.Auth;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models.Extensions;
using hrvster.Web.Models;
using MediatR;
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
		private readonly ISessionManager _sessionManager;
		private readonly IMediator _mediator;

		public SessionController(
			ILoggerFactory loggerFactory,
			IToastNotification notificationService,
			IFirebaseAuthProvider firebaseAuthProvider,
			ISessionManager sessionManager,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SessionController>();
			_notificationService = notificationService;
			_firebaseAuthProvider = firebaseAuthProvider;
			_sessionManager = sessionManager;
			_mediator = mediator;
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

						_sessionManager.SetKeyValue(SessionConstants.UserToken, link.FirebaseToken);
						_sessionManager.SetKeyValue(SessionConstants.UserUid, link.User.LocalId);
						_sessionManager.SetKeyValue(SessionConstants.UserEmail, link.User.Email);
						_sessionManager.SetKeyValue(SessionConstants.UserDisplayName, link.User.DisplayName);

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
			_sessionManager.RemoveKeyValue(SessionConstants.UserToken);

			return RedirectToAction("SignIn");
		}
	}
}

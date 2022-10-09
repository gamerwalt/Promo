using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Helpers;
using Firebase.Auth;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests;
using hrvster.Extensions.ModelExtensions;
using hrvster.Infrastructure.Authentication.ConfigurationOptions;
using hrvster.Models.Extensions;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class SessionController : Controller
	{
		private readonly ILogger<SessionController> _logger;
		private readonly IMediator _mediator;
		private readonly GoogleServicesOptions _googleServicesOptions;
		private readonly INotyfService _notificationService;
		private readonly FirebaseAuthProvider _firebaseAuthProvider;

		public SessionController(
			ILoggerFactory loggerFactory, 
			IMediator mediator, 
			IOptions<GoogleServicesOptions> googleServicesOptions,
			INotyfService notificationService,
			FirebaseAuthProvider firebaseAuthProvider)
		{
			_logger = loggerFactory.CreateLogger<SessionController>();
			_mediator = mediator;
			_googleServicesOptions = googleServicesOptions.Value;
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
			if(ModelState.IsValid)
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
							return RedirectToAction(actionName: "Index", controllerName: "Dashboard");
						else
							return Redirect(returnUrl);
					}
				}
				catch (FirebaseAuthException ex)
				{
					var firebaseError = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);

					_logger.LogWarning("A Firebase error has occurred during logon. Error: {firebaseError}", firebaseError.ToJsonString());

					_notificationService.Error("Invalid email or password. Please try again.", 10);

					return RedirectToAction("SignIn");
				}
			}

			_notificationService.Error(ModelState.BuildMessageFromErrors(), 10); 

			return RedirectToAction("SignIn");
		}

		public async Task<ActionResult> SignOut()
		{
			HttpContext.Session.Remove("_UserToken");

			return RedirectToAction("SignIn");
		}
	}
}

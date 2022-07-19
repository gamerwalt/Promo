using AspNetCoreHero.ToastNotification.Abstractions;
using Firebase.Auth;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Promo.Core.Authentication.ConfigurationOptions;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class SessionController : Controller
	{
		private readonly ILogger<SessionController> _logger;
		private readonly IMediator _mediator;
		private readonly GoogleServicesOptions _googleServicesOptions;
		private readonly INotyfService _notificationService;

		FirebaseAuthProvider authProvider;

		public SessionController(
			ILoggerFactory loggerFactory, 
			IMediator mediator, 
			IOptions<GoogleServicesOptions> googleServicesOptions,
			INotyfService notificationService)
		{
			_logger = loggerFactory.CreateLogger<SessionController>();
			_mediator = mediator;
			_googleServicesOptions = googleServicesOptions.Value;
			_notificationService = notificationService;

			authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(_googleServicesOptions.WebApiKey));
		}

		[HttpGet]
		public ActionResult SignIn()
		{
			var token = HttpContext.Session.GetString("_UserToken");

			return token == null ? View() : RedirectToAction("Index", "Dashboard");
		}

		// POST: SessionController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SignIn(SigninForm form)
		{
			try
			{
				var link = await authProvider.SignInWithEmailAndPasswordAsync(form.Email, form.Password);

				if (!string.IsNullOrWhiteSpace(link.FirebaseToken))// && link.User.IsEmailVerified)
				{
					HttpContext.Session.SetString(SessionConstants.UserToken, link.FirebaseToken);
					HttpContext.Session.SetString(SessionConstants.UserUid, link.User.LocalId);
					HttpContext.Session.SetString(SessionConstants.UserEmail, link.User.Email);
					HttpContext.Session.SetString(SessionConstants.UserDisplayName, link.User.DisplayName);

					return RedirectToAction(actionName: "Index", controllerName: "Dashboard");
				}
			}
			catch (FirebaseAuthException ex)
			{
				var firebaseError = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);

				_notificationService.Warning("Invalid email or password.");

				return View(form);
			}

			return RedirectToAction(nameof(SignIn));
		}

		public async Task<ActionResult> SignOut()
		{
			HttpContext.Session.Remove("_UserToken");

			return RedirectToAction("SignIn");
		}
	}
}

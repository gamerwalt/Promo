using hrvster.Exceptions;
using hrvster.Web.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System.Diagnostics;

namespace hrvster.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IToastNotification _toastNotification;

		public HomeController(ILogger<HomeController> logger, IToastNotification toastNotification)
		{
			_logger = logger;
			_toastNotification = toastNotification;
		}

		public IActionResult Index()
		{
			return RedirectToAction("Index", "Dashboard");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			//Todo: Get more error details here

			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult HandleError()
		{
			var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

			if (exceptionFeature != null)
			{
				if (exceptionFeature.Error is AccountIsNotSubscribedException customException)
				{
					_toastNotification.AddErrorToastMessage(customException.Message);
				}
				else
				{
					_logger.LogError("An exception occurred. Exception is of type {exceptionType}.", exceptionFeature.Error.GetType().Name);

					_toastNotification.AddErrorToastMessage("An unexpected error occurred.");
				}

				return Redirect(HttpContext.Request.Headers["Referer"].ToString());
			}

			return RedirectToAction("Index");
		}
	}
}

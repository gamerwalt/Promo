using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Requests.WebRequests;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class VerifyEmailController : Controller
	{
		private readonly ILogger<VerifyEmailController> _logger;
		private readonly IMediator _mediator;
		private readonly INotyfService _notificationService;

		public VerifyEmailController(ILoggerFactory loggerFactory, IMediator mediator, INotyfService notificationService)
		{
			_logger = loggerFactory.CreateLogger<VerifyEmailController>();
			_mediator = mediator;
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Verify(VerifyEmail form)
		{
			if(ModelState.IsValid)
			{
				await _mediator.Send(new VerifyEmailRequest(form.EmailAddress));

				_notificationService.Information("If you have an account with us, you should receive an email soon on how to verify your account.");

				return RedirectToAction("SignIn", "Session");
			}

			return View(form);
		}
	}
}

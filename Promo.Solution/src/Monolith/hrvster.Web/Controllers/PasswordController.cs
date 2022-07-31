using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Requests.WebRequests;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class PasswordController : Controller
	{
		private readonly ILogger<PasswordController> _logger;
		private readonly INotyfService _notificationService;
		private readonly IMediator _mediator;

		public PasswordController(ILoggerFactory loggerFactory, INotyfService notificationService, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<PasswordController>();
			_notificationService = notificationService;
			_mediator = mediator;
		}

		[HttpGet]
		public async Task<IActionResult> Forgot()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Forgot(ForgotPassword form)
		{
			if(ModelState.IsValid)
			{
				await _mediator.Send(new ForgotPasswordRequest(form.EmailAddress));

				return RedirectToAction("SignIn", "Session");
			}

			var errors = ModelState.Select(c => c.Value.Errors)
				.Where(c => c.Count > 0)
				.ToList();

			_notificationService.Error(BuildMessageFromErrors(errors));

			return View();
		}

		private string BuildMessageFromErrors(List<ModelErrorCollection> errors)
		{
			var sb = new StringBuilder();
			foreach (var error in errors)
			{
				sb.Append(error.FirstOrDefault()?.ErrorMessage);
			}

			return sb.ToString();
		}
	}
}

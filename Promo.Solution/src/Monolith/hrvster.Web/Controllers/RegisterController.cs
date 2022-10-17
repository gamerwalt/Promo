using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;

		public RegisterController(IMediator mediator, IToastNotification notificationService)
		{
			_mediator = mediator;
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<ActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Register form)
		{
			if (ModelState.IsValid)
			{
				var userUid = Guid.NewGuid();
				await _mediator.Send(new CreateAccountRequest(
					userUid,
					form.EmailAddress,
					form.UserName,
					form.PhoneNumber,
					form.FirstName,
					form.LastName,
					form.Password,
					form.AcceptedTerms));

				_notificationService.AddSuccessToastMessage("Account created successfully.");

				return RedirectToActionPermanent("SignIn", "Session");
			}

			var errors = ModelState.Select(c => c.Value.Errors)
				.Where(c => c.Count > 0)
				.ToList();

			_notificationService.AddErrorToastMessage(BuildMessageFromErrors(errors));

			return View("Create", form);
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

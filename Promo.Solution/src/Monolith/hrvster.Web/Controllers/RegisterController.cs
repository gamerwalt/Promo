using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		private readonly INotyfService _notificationService;

		public RegisterController(IMediator mediator, INotyfService notificationService)
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
			if(ModelState.IsValid)
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

				_notificationService.Success("Account created successfully.");

				return RedirectToActionPermanent("SignIn", "Session");
			}

			var errors = ModelState.Select(c => c.Value.Errors)
				.Where(c => c.Count > 0)
				.ToList();

			_notificationService.Error(BuildMessageFromErrors(errors));

			return View("Create", form);
		}

		private string BuildMessageFromErrors(List<ModelErrorCollection> errors)
		{
			var sb = new StringBuilder();
			foreach(var error in errors)
			{
				sb.Append(error.FirstOrDefault()?.ErrorMessage);
			}

			return sb.ToString();
		}
	}
}

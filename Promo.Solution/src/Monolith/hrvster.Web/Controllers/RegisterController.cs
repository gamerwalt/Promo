using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
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
		public async Task<ActionResult> Index()
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

			return View("Index", form);
		}
	}
}

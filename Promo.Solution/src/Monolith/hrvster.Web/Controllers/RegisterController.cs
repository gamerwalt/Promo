using hrvster.Core;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Extensions.ModelExtensions;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Web.Models;
using hrvster.Web.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IMediator _mediator;
		private readonly IToastNotification _notificationService;
		private readonly ILoggerFactory _loggerFactory;
		private readonly IAccountsRepository _accountsRepository;
		private readonly IAccountCompanyRepository _accountCompanyRepository;

		public RegisterController(
			ILoggerFactory loggerFactory,
			IAccountsRepository accountsRepository,
			IAccountCompanyRepository accountCompanyRepository,
			IMediator mediator,
			IToastNotification notificationService)
		{
			_loggerFactory = loggerFactory;
			_accountCompanyRepository = accountCompanyRepository;
			_accountsRepository = accountsRepository;
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
					form.AcceptedTerms,
					form.CompanyName,
					form.SubdomainName));

				_notificationService.AddSuccessToastMessage(ToastMessages.AccountCreatedSuccessfully);

				return RedirectToActionPermanent("SignIn", "Session");
			}

			_notificationService.AddErrorToastMessage(ModelState.BuildMessageFromErrors());

			return View("Create", form);
		}
	}
}

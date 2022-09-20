using AspNetCoreHero.ToastNotification.Abstractions;
using hrvster.Core.Authentication;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Extensions.ModelExtensions;
using hrvster.Models;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UpdateAccount = hrvster.Web.Models.UpdateAccount;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class ProfileController : Controller
	{
		private readonly ILogger<ProfileController> _logger;
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMediator _mediator;
		private readonly INotyfService _notificationService;

		public ProfileController(
			ILoggerFactory loggerFactory,
			HrvsterUser hrvsterUser,
			IMediator mediator,
			INotyfService notificationService)
		{
			_logger = loggerFactory.CreateLogger<ProfileController>();
			_hrvsterUser = hrvsterUser;
			_mediator = mediator;
			_notificationService = notificationService;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var account = await _mediator.Send(new GetAccountAndProfileRequest(_hrvsterUser.UserUid));
			var totalsPerAccount = await _mediator.Send(new GetTotalResourcesRequest(_hrvsterUser.UserUid));

			var model = new UpdateAccount
			{
				AccountDetails = new AccountDetails
				{
					Account = account,
					Profiles = account.Profiles,
					TotalSweepEntries = totalsPerAccount.TotalSweepEntries,
					TotalPromotions = totalsPerAccount.TotalCampaigns,
					TotalSweepstakes = totalsPerAccount.TotalSweeps
				},
				DisplayName = account.DisplayName,
				FirstName = account.FirstName,
				LastName = account.LastName,
				EmailAddress = account.EmailAddress,
				PhoneNumber = account.PhoneNumber
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> UpdateAccount()
		{
			var model = await BuildProfileModel();

			return View(model);
		}

		private async Task<UpdateAccount> BuildProfileModel()
		{
			var account = await _mediator.Send(new GetAccountAndProfileRequest(_hrvsterUser.UserUid));
			var totalsPerAccount = await _mediator.Send(new GetTotalResourcesRequest(_hrvsterUser.UserUid));

			var model = new UpdateAccount
			{
				AccountDetails = new AccountDetails
				{
					Account = account,
					Profiles = account.Profiles,
					TotalSweepEntries = totalsPerAccount.TotalSweepEntries,
					TotalPromotions = totalsPerAccount.TotalCampaigns,
					TotalSweepstakes = totalsPerAccount.TotalSweeps
				},
				DisplayName = account.DisplayName,
				FirstName = account.FirstName,
				LastName = account.LastName,
				EmailAddress = account.EmailAddress,
				PhoneNumber = account.PhoneNumber
			};
			return model;
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccount(UpdateAccount form)
		{
			if (ModelState.IsValid)
			{
				await _mediator.Send(new UpdateAccountRequest(_hrvsterUser.UserUid, form.DisplayName, form.FirstName, form.LastName));

				_notificationService.Success("Account updated successfully.", NotificationService.DismissInSeconds);

				return RedirectToAction("Index");
			}

			var errorMessages = ModelState.BuildMessageFromErrors();

			_logger.LogWarning(errorMessages);

			_notificationService.Error(errorMessages, NotificationService.DismissInSeconds);

			_notificationService.Error(ModelState.BuildMessageFromErrors());

			return View(await BuildProfileModel());
		}

		[HttpGet]
		public async Task<IActionResult> UpdateAccountDetails()
		{
			var account = await _mediator.Send(new GetAccountAndProfileRequest(_hrvsterUser.UserUid));
			var totalsPerAccount = await _mediator.Send(new GetTotalResourcesRequest(_hrvsterUser.UserUid));

			var model = new UpdateAccount
			{
				AccountDetails = new AccountDetails
				{
					Account = account,
					Profiles = account.Profiles,
					TotalSweepEntries = totalsPerAccount.TotalSweepEntries,
					TotalPromotions = totalsPerAccount.TotalCampaigns,
					TotalSweepstakes = totalsPerAccount.TotalSweeps
				},
				DisplayName = account.DisplayName,
				FirstName = account.FirstName,
				LastName = account.LastName,
				EmailAddress = account.EmailAddress,
				PhoneNumber = account.PhoneNumber
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAccountDetails(UpdateAccount form)
		{
			if(ModelState.IsValid)
			{
				await _mediator.Send(new UpdateAccountDetailsRequest(_hrvsterUser.UserUid, form.EmailAddress, form.PhoneNumber));

				_notificationService.Success("Account details updated. Please logon.");

				return RedirectToAction("SignOut", "Session");
			}
			var errorMessages = ModelState.BuildMessageFromErrors();

			_logger.LogWarning(errorMessages);

			_notificationService.Error(errorMessages, NotificationService.DismissInSeconds);

			return View(await BuildProfileModel());
		}

		[HttpPost]
		public async Task<IActionResult> Unlink(ProfileType profileType)
		{
			await _mediator.Send(new UnlinkProfileRequest(_hrvsterUser.UserUid, profileType));

			return RedirectToAction("Index");
		}
	}
}

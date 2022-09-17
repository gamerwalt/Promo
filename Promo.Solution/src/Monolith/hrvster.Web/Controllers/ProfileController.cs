using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Accounts;
using hrvster.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class ProfileController : Controller
	{
		private readonly HrvsterUser _hrvsterUser;
		private readonly IMediator _mediator;

		public ProfileController(
			HrvsterUser hrvsterUser,
			IMediator mediator)
		{
			_hrvsterUser = hrvsterUser;
			_mediator = mediator;
		}

		public async Task<IActionResult> Index()
		{
			var account = await _mediator.Send(new GetAccountAndProfileRequest(_hrvsterUser.UserUid));
			var totalsPerAccount = await _mediator.Send(new GetTotalResourcesRequest(_hrvsterUser.UserUid));

			var model = new ViewAccount
			{
				Account = account,
				TotalSweepEntries = totalsPerAccount.TotalSweepEntries,
				TotalPromotions = totalsPerAccount.TotalCampaigns,
				TotalSweepstakes = totalsPerAccount.TotalSweeps,
				UpdateAccount = new UpdateAccount
				{
					DisplayName = account.DisplayName,
					FirstName = account.FirstName,
					LastName = account.LastName
				},
				UpdateEmailAddress = new UpdateEmailAddress
				{
					EmailAddress = account.EmailAddress
				},
				Profiles = account.Profiles
			};

			return View(model);
		}
	}
}

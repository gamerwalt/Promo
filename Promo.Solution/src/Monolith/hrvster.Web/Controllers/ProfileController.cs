using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests.Accounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

			return View(account);
		}
	}
}

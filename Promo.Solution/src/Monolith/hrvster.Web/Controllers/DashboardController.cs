using hrvster.Core.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class DashboardController : Controller
	{
		private readonly HrvsterUser _hrvsterUser;
		public DashboardController(HrvsterUser hrvsterUser)
		{
			_hrvsterUser = hrvsterUser;
		}

		[HttpGet]
		public async Task<ActionResult> Index()
		{
			return View();
		}
	}
}

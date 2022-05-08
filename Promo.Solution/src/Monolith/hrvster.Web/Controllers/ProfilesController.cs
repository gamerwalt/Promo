using hrvster.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class ProfilesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

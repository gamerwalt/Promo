using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Web.Controllers
{
	public class DashboardController : Controller
	{
		public DashboardController()
		{

		}

		public IActionResult Index()
		{
			var token = HttpContext.Session.GetString("_UserToken");
			if(token != null)
			{
				return View();
			}
			else
			{
				return RedirectToAction("SignIn", "Session");
			}
		}
	}
}

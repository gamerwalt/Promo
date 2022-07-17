using Microsoft.AspNetCore.Mvc;

namespace hrvster.Web.Controllers
{
	public class TermsAndConditionsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

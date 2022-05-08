using Microsoft.AspNetCore.Mvc;

namespace hrvster.Web.Controllers
{
	public class PromotionsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

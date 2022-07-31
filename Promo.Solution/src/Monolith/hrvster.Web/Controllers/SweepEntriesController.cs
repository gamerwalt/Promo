using Microsoft.AspNetCore.Mvc;

namespace hrvster.Web.Controllers
{
	public class SweepEntriesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

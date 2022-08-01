using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class TweetsHarvestedController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> View(Guid sweepUid)
		{
			return View();
		}
	}
}

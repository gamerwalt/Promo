using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class TermsController : Controller
	{
		public async Task<IActionResult> SweepstakesTermsAndConditions()
		{
			return View();
		}
	}
}

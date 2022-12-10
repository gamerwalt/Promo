using hrvster.Core.Authentication;
using hrvster.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[FirebaseAuthorize]
	public class SubscriptionsController : Controller
	{
		public async Task<ActionResult> Index(string subscriptionStatus)
		{
			return RedirectToAction(subscriptionStatus);
		}

		public async Task<ActionResult> UnSubscribed()
		{
			return new OkObjectResult(new { message = "Unsubscribed" });
		}

		public async Task<ActionResult> Subscribed()
		{
			return new OkObjectResult(new { message = "Subscribed" });
		}
	}
}

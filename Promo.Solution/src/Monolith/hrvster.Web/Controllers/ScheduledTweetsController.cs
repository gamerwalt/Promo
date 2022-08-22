using hrvster.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class ScheduledTweetsController : Controller
	{
		public async Task<IActionResult> Index()
		{
			return View();
		}

		public async Task<IActionResult> ViewSweepRule(Guid ruleUid, Guid sweepUid)
		{
			return View(new ViewSweepRule
			{
				RuleUid = ruleUid,
				SweepUid = sweepUid
			});
		}
	}
}

using hrvster.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Controllers
{
	[Authorize]
	public class GatewayController : Controller
	{
		[HttpGet("Gateway/Game/{gameUid}/Rule/{ruleUid}/ExternalClient/{externalClient}/GameType/{gameType}")]
		public async Task<IActionResult> DirectByGameType(
			[FromQuery] Guid gameUid,
			[FromQuery] Guid ruleUid,
			[FromQuery] string externalClient,
			[FromQuery] string gameType)
		{
			var url = string.Empty;

			switch (gameType)
			{
				case GameTypes.Sweepstake:
					return RedirectToAction("PrePlay", "Sweepstakes", new
					{
						gameUid,
						ruleUid,
						externalClient
					});
				case GameTypes.MatchGame:
					return RedirectToAction("PrePlay", "Pick3Game", new
					{
						gameUid,
						ruleUid,
						externalClient
					});
			}

			return new RedirectResult(url);
		}
	}
}

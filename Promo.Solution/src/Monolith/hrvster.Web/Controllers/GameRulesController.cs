using hrvster.Core.Requests.WebRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class GameRulesController : Controller
	{
		private readonly ILogger<GameRulesController> _logger;
		private readonly IMediator _mediator;

		public GameRulesController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<GameRulesController>();
			_mediator = mediator;
		}

		//https://localhost:5001/GameRules/Sweepstakes/NG/en/0ce3d60e-99ee-4558-8551-cbb285043f3f
		[HttpGet]
		[Route("GameRules/Sweepstakes/NG/en/{sweepUid}")]
		public async Task<IActionResult> Sweepstakes([FromRoute] Guid sweepUid)
		{
			var sweepRulesAndPrizes = await _mediator.Send(new GetSweepstakeForRulesViewRequest(sweepUid));

			if(sweepRulesAndPrizes.Profile == null)
			{
				return Redirect("https://hrvster.com");
			}

			return View(sweepRulesAndPrizes);
		}

		//https://localhost:5001/GameRules/MatchGames/NG/en/20a8861d-32ac-431f-879a-029ee9b3a927
		[HttpGet]
		[Route("GameRules/MatchGames/NG/en/{gameUid}")]
		public async Task<IActionResult> MatchGames([FromRoute] Guid gameUid)
		{
			var gameAndPrizes = await _mediator.Send(new GetMatchGameAndPrizesRequest(gameUid));

			if (gameAndPrizes.Profile == null)
			{
				return Redirect("https://hrvster.com");
			}

			var saskatoonTimezone = TimeZoneInfo.FindSystemTimeZoneById("Canada/Saskatchewan");
			var watTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
			var watStartTime = TimeZoneInfo.ConvertTime(gameAndPrizes.Game.StartDate, saskatoonTimezone, watTimeZone);
			var watEndTime = TimeZoneInfo.ConvertTime(gameAndPrizes.Game.EndDate, saskatoonTimezone, watTimeZone);

			gameAndPrizes.Game.StartDateUtc = watStartTime;
			gameAndPrizes.Game.EndDateUtc = watEndTime;

			return View(gameAndPrizes);
		}
	}
}

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

		[HttpGet]
		[Route("GameRules/MatchGames/NG/en/{gameUid}")]
		public async Task<IActionResult> MatchGames([FromRoute] Guid gameUid)
		{
			//var gameAndPrizes = await _mediator.Send(new GetMatchGameAndPrizesRequest(gameUid));

			//if(gameAndPrizes.Profile == null)
			//{
			//	return Redirect("https://hrvster.com");
			//}

			//return View(gameAndPrizes);
			return new OkObjectResult("OK");
		}
	}
}

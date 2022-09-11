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
	}
}

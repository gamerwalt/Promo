using hrvster.Core.Requests;
using hrvster.Core.Requests.WebRequests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	public class GameRules : Controller
	{
		private readonly ILogger<GameRules> _logger;
		private readonly IMediator _mediator;

		public GameRules(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<GameRules>();
			_mediator = mediator;
		}

		[HttpGet]
		[Route("GameRules/Sweepstakes/NG/en/{sweepUid}")]
		public async Task<IActionResult> Sweepstakes([FromRoute] Guid sweepUid)
		{
			var sweepRulesAndPrizes = await _mediator.Send(new GetSweepstakeForRulesViewRequest(sweepUid));

			return View(sweepRulesAndPrizes);
		}
	}
}

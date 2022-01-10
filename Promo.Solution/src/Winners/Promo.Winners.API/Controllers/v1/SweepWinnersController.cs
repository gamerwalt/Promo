using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Winners.Core.RequestHandlers;
using Promo.Winners.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Winners.API.Controllers.v1
{
	[ApiController]
	[Route("v1/winners/sweeps/{sweepUid:Guid}")]
	public class SweepWinnersController : ControllerBase
	{
		private readonly ILogger<SweepWinnersController> _logger;
		private readonly IMediator _mediator;

		public SweepWinnersController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepWinnersController>();
			_mediator = mediator;

			_logger.LogInformation("Logger entered {}", nameof(SweepWinnersController));
		}

		[HttpPost("list")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Infrastructure.Models.SweepWinner>))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Infrastructure.Models.SweepWinner>>> CreateAsync([FromRoute] Guid sweepUid, [FromBody] SweepWinnerRequest request)
		{
			var winnersExists = await _mediator.Send(new GetSweepWinners(sweepUid));
			if(winnersExists != null || winnersExists.Count() > 0)
			{
				_logger.LogWarning("Sweep winners already exists for Sweep with Uid: {sweepUid}", sweepUid);
				return new BadRequestObjectResult(new { message = "Winners already exists for this sweepstakes." });
			}

			var winnersSaved = await _mediator.Send(new SaveSweepWinner(request.SweepWinners, sweepUid));

			return new OkObjectResult(winnersSaved);
		}
	}
}

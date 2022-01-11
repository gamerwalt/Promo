using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Prizes.Core.Request;
using Promo.Prizes.Core.RequestHandlers;
using Promo.Prizes.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Prizes.API.Controllers.v1
{
	[ApiController]
	[Route("v1/prizes/sweeps/{sweepUid:Guid}")]
	[Authorize]
	public class PrizesController : ControllerBase
	{
		private readonly ILogger<PrizesController> _logger;
		private readonly IMediator _mediator;

		public PrizesController(ILogger<PrizesController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		[HttpPost]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<SweepPrize>))]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<ActionResult<IEnumerable<SweepPrize>>> Create([FromRoute] Guid sweepUid, [FromBody] SweepPrizeRequest request)
		{
			var sweepPrizes = await _mediator.Send(new CreateSweepPrizeRequest(sweepUid, request.SweepPrizes));

			return new OkObjectResult(sweepPrizes);
		}

		[HttpGet]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<SweepPrize>))]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<ActionResult<IEnumerable<SweepPrize>>> GetAllSweepPrizes([FromRoute] Guid sweepUid)
		{
			var sweepPrizes = await _mediator.Send(new GetSweepPrizes(sweepUid));

			return new OkObjectResult(sweepPrizes);
		}
	}
}

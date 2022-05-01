using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[ApiController]
	[Route("v1/prizes/sweeps/{sweepUid:Guid}")]
	public class SweepPrizesController : ControllerBase
	{
		private readonly ILogger<SweepPrizesController> _logger;
		private readonly IMediator _mediator;

		public SweepPrizesController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepPrizesController>();
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

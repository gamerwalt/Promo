using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core.Models;
using Promo.Harvester.Core.RequestHandlers;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Harvester.API.Controllers.v1
{
    [ApiController]
    [Route("v1/sweeps/{sweepUid:Guid}")]
    public class SweepPrizesController : ControllerBase
    {
        private readonly ILogger<HarvestedUsersController> _logger;
        private readonly IMediator _mediator;

        public SweepPrizesController(ILogger<HarvestedUsersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("prizes")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SweepPrize))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<ActionResult<SweepPrize>> Create([FromRoute] Guid sweepUid, [FromBody] SweepPrize sweepPrize)
        {
			var sweepPrizeSaved = await _mediator.Send(new CreateSweepPrize(sweepPrize, sweepUid));

            return new OkObjectResult(sweepPrizeSaved);
        }
    }
}

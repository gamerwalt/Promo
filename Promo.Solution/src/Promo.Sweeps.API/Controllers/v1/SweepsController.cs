using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Sweeps.Core.RequestHandlers;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Sweeps.API.Controllers.v1
{
    [ApiController]
    [Route("v1")]
    public class SweepsController : ControllerBase
    {
        private readonly ILogger<SweepsController> _logger;
        private readonly IMediator _mediator;

        public SweepsController(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<SweepsController>();
            _mediator = mediator;
        }

        [HttpPost("sweeps")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Sweep>> Create([FromBody] NewSweepRequest request)
        {
            var sweep = await _mediator.Send(new CreateSweep(request));

            return Ok(sweep);
        }

        [HttpGet("sweeps/campaign/{campaignUid:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Sweep>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Sweep>>> GetByCampaign([FromRoute] Guid campaignUid)
        {
            return new OkObjectResult(await _mediator.Send(new GetSweepsForCampaignUid(campaignUid)));
        }

        [HttpGet("sweeps/{sweepUid:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize]
        public async Task<ActionResult<Sweep>> Get([FromRoute] Guid sweepUid)
        {
            var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
            if (sweep == null)
                return new NotFoundObjectResult(new { message = $"Sweep with Uid: {sweepUid} does not exist." });

            return new OkObjectResult(await _mediator.Send(new GetSweepByUid(sweepUid)));
        }

        [HttpPut("sweeps/{sweepUid:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize]
        public async Task<ActionResult<Sweep>> Update([FromRoute] Guid sweepUid, [FromBody] UpdateSweepRequest request)
        {
            var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
            if (sweep == null)
                return new NotFoundObjectResult(new { message = $"Sweep with Uid: {sweepUid} does not exist." });

            return new OkObjectResult(await _mediator.Send(new UpdateSweep(sweepUid, sweep, request)));
        }
    }
}

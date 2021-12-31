using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("v1/campaign/{campaignUid:Guid}")]
    [Authorize]
    public class CampaignSweepController : Controller
    {
        private readonly IMediator _mediator;

        public CampaignSweepController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("sweeps")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<SweepEntry>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<IEnumerable<SweepEntry>>> Enter([FromRoute] Guid campaignUid, [FromBody] NewCampaignSweepEntry request)
        {
            var sweepEntries = await _mediator.Send(new CreateCampaignSweepEntry(request, campaignUid));

            return Ok(sweepEntries);
        }
	}
}

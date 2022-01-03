using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promo.Sweeps.Core.RequestHandlers;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Sweeps.API.Controllers.v1
{
    [ApiController]
    [Route("v1/sweeps/{sweepUid:Guid}")]
    [Authorize]
    public class SweepEntriesController : Controller
    {
        private readonly IMediator _mediator;

        public SweepEntriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("entry")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SweepEntry))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<SweepEntry>> Create([FromRoute] Guid sweepUid, [FromBody] NewSweepEntryRequest request)
        {
            var sweepEntries = await _mediator.Send(new CreateSweepEntry(request, sweepUid));

            return Ok(sweepEntries);
        }
    }
}

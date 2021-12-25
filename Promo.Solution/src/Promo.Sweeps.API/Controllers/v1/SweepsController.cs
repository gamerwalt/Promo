using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Sweeps.Core.RequestHandlers;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure.Models;
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
            var sweep = await _mediator.Send(new CreateSweepRequest(request));

            return Ok(sweep);
        }
    }
}

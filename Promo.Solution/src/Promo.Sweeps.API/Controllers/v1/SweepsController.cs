using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Sweeps.Core.Requests;
using Promo.Sweeps.Infrastructure.Models;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Sweeps.API.Controllers.v1
{
    [ApiController]
    [Route("v1/sweeps")]
    public class SweepsController : ControllerBase
    {
        private readonly ILogger<SweepsController> _logger;

        public SweepsController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SweepsController>();
        }

        [HttpPost("rules")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<Sweep>> Create([FromBody] NewSweepRequest request)
        {
            return Ok(request);
        }
    }
}

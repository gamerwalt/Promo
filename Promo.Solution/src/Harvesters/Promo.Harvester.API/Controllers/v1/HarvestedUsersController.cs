using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core.Models;
using Promo.Harvester.Core.RequestHandlers;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Harvester.API.Controllers.v1
{
    [ApiController]
    [Route("v1")]
    public class HarvestedUsersController : ControllerBase
    {
        private readonly ILogger<HarvestedUsersController> _logger;
        private readonly IMediator _mediator;

        public HarvestedUsersController(ILogger<HarvestedUsersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("users/{userIdentifier}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(HarvestedUser))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<ActionResult<HarvestedUser>> CreateAsync([FromRoute] string userIdentifier)
        {
            var winnersSaved = await _mediator.Send(new GetHarvestedUser(userIdentifier));

            return new OkObjectResult(winnersSaved);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Rules.Core.RequestHandlers;
using Promo.Rules.Infrastructure.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Rules.API.Controllers
{
    [ApiController]
    [Route("v1/watchtypes")]
    [Authorize]
    public class WatchTypesController : Controller
    {
        private readonly ILogger<WatchTypesController> _logger;
        private readonly IMediator _mediator;

        public WatchTypesController(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<WatchTypesController>();
            _mediator = mediator;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<WatchTypeCampaignType>))]
        public async Task<ActionResult<IEnumerable<WatchTypeCampaignType>>> GetWatchTypeCampaignTypes()
        {
            var watchTypeCampaignTypes = await _mediator.Send(new GetWatchTypeCampaignTypes());

            return new OkObjectResult(watchTypeCampaignTypes);
        }
    }
}

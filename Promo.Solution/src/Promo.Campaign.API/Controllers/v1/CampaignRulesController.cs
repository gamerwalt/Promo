using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Campaign.Infrastructure.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Campaign.API.Controllers.v1
{
    [Route("v1/campaigns/{campaignUid:Guid}")]
    [ApiController]
    [Authorize]
    public class CampaignRulesController : ControllerBase
    {
        private readonly ILogger<CampaignRulesController> _logger;
        private readonly IMediator _mediator;

        public CampaignRulesController(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<CampaignRulesController>();
            _mediator = mediator;
        }

        [HttpPost("rules")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CampaignType))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> AddRules([FromRoute] Guid campaignUid)
        {
            return Ok(campaignUid);
        }

    }
}

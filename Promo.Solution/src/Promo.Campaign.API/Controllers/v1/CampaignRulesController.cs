using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Campaign.Core.RequestHandlers;
using Promo.Campaign.Infrastructure.Models;
using Promo.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Campaign.API.Controllers.v1
{
    [Route("v1")]
    [ApiController]
    public class CampaignRulesController : ControllerBase
    {
        private readonly ILogger<CampaignRulesController> _logger;
        private readonly IMediator _mediator;

        public CampaignRulesController(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger<CampaignRulesController>();
            _mediator = mediator;
        }

        [HttpPost("campaigns/{campaignUid:Guid}/rules")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CampaignType))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize]
        public async Task<ActionResult> AddRules([FromRoute] Guid campaignUid, [FromBody] CampaignRule rule)
        {
            await _mediator.Send(new AddCampaignRule(campaignUid, rule));

            return Ok();
        }

        [HttpGet("campaigns/watchtypes")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<WatchTypeCode>))]
        public async Task<ActionResult<IEnumerable<WatchTypeCode>>> GetWatchTypes()
        {
            var watchTypeCodes = Enum.GetValues(typeof(WatchTypeCode))
                .Cast<WatchTypeCode>().Select(c => c.ToString());
            return new OkObjectResult(watchTypeCodes);
        }

    }
}

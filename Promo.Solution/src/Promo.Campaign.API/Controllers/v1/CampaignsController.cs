using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Campaign.Core.RequestHandlers;
using Promo.Campaign.Core.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Campaign.API.Controllers.v1
{
    [ApiController]
    [Route("v1")]
    public class CampaignsController : ControllerBase
    {
        private readonly ILogger<CampaignsController> _logger;
        private readonly IMediator _mediator;

        public CampaignsController(
            ILogger<CampaignsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("campaigns")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Infrastructure.Models.Campaign))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize]
        public async Task<ActionResult<Infrastructure.Models.Campaign>> CreateAsync([FromBody] NewCampaignRequest request)
        {
            var campaignType = await _mediator.Send(new GetCampaignTypeByUidRequest(request.CampaignTypeUid));
            if (campaignType == null)
                return new BadRequestObjectResult(new { message = $"The campaign type for Uid: {request.CampaignTypeUid} does not exist." });

            var campaignUid = await _mediator.Send(new CreateCampaignRequest(request, User));

            var campaign = await _mediator.Send(new GetCampaignByUidRequest(campaignUid));

            return new OkObjectResult(campaign);
        }

        [HttpGet("campaigns")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Infrastructure.Models.Campaign>))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize]
        public async Task<ActionResult<List<Infrastructure.Models.Campaign>>> GetAllAsync()
        {
            var accountUid = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type.Equals("user_id", StringComparison.OrdinalIgnoreCase)).Value);

            var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(accountUid));

            return new OkObjectResult(campaigns);
        }

        [HttpGet("campaigns/{campaignUid:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Infrastructure.Models.Campaign))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize]
        public async Task<ActionResult<Infrastructure.Models.Campaign>> GetAsync([FromRoute] Guid campaignUid)
        {
            var campaign = await _mediator.Send(new GetCampaignByUidRequest(campaignUid));

            if (campaign == null)
                return new NotFoundObjectResult(new { message = "Campaign not found." });

            return new OkObjectResult(campaign);
        }

        [HttpPut("campaigns/{campaignUid:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Infrastructure.Models.Campaign))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Infrastructure.Models.Campaign>> UpdateAsync([FromRoute] Guid campaignUid, UpdateCampaignRequest request)
        {
            var campaign = await _mediator.Send(new GetCampaignByUidRequest(campaignUid));

            if (campaign == null)
                return new NotFoundObjectResult(new { message = "Campaign not found." });

            campaign = await _mediator.Send(new UpdateCampaignViaUidRequest(campaignUid, request));

            return new OkObjectResult(campaign);
        }
    }
}

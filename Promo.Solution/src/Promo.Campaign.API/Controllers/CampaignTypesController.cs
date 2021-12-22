using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core.CoreRequestHandlers;
using Promo.Models.Campaigns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Campaign.API.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CampaignTypesController : ControllerBase
    {
        private readonly ILogger<CampaignTypesController> _logger;
        private readonly IMediator _mediator;

        public CampaignTypesController(
            ILogger<CampaignTypesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("campaigntypes")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CampaignType))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<IEnumerable<CampaignType>>> GetAllAsync()
        {
            var campaignTypes = await _mediator.Send(new GetAllCampaignTypesRequest());

            return new OkObjectResult(campaignTypes);
        }
    }
}

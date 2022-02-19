using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core;
using Promo.Core.Models;
using Promo.Membership.Core.RequestHandlers;
using Promo.Membership.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Membership.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class CampaignRulesController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<CampaignRulesController> _logger;

		public CampaignRulesController(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<CampaignRulesController>();
			_mediator = mediator;
		}

		[HttpPost("campaigns/{campaignUid:Guid}/sweeps/{sweepUid:Guid}/rules")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Infrastructure.Models.CampaignType))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[Authorize]
		public async Task<ActionResult> AddRules([FromRoute] Guid campaignUid, [FromRoute] Guid sweepUid, [FromBody] CampaignRule rule)
		{
			await _mediator.Send(new AddCampaignRule(campaignUid, sweepUid, rule));

			return new OkObjectResult(new ApiResponse(null, 200));
		}
	}
}

using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Configuration.Extensions;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class CampaignsController : ControllerBase
	{
		private readonly ILogger<CampaignsController> _logger;
		private readonly IMediator _mediator;

		public CampaignsController(ILogger<CampaignsController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		[HttpPost("campaigns")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Campaign))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		[Authorize]
		public async Task<ActionResult<Campaign>> CreateAsync([FromBody] NewCampaignRequest request)
		{
			var campaignType = await _mediator.Send(new GetCampaignTypeByUidRequest(request.CampaignTypeUid));
			if (campaignType == null)
				return new BadRequestObjectResult(new { message = $"The campaign type for Uid: {request.CampaignTypeUid} does not exist." });

			var campaignUid = await _mediator.Send(new CreateCampaignRequest(request, User));

			var campaign = await _mediator.Send(new GetCampaign(campaignUid));

			return new OkObjectResult(new ApiResponse(campaign, 200));
		}

		[HttpGet("campaigns")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(List<Campaign>))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[Authorize]
		public async Task<ActionResult<List<Campaign>>> GetAllAsync()
		{
			var accountUid = User.GetAccountUidFromCliam("user_id");

			var campaigns = await _mediator.Send(new GetAllCampaignsForAccount(accountUid));

			return new OkObjectResult(new ApiResponse(campaigns, 200));
		}

		[HttpGet("campaigns/{campaignUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Campaign))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[Authorize]
		public async Task<ActionResult<Campaign>> GetAsync([FromRoute] Guid campaignUid)
		{
			var accountUid = User.GetAccountUidFromCliam("user_id");

			var campaign = await _mediator.Send(new GetCampaignByUidRequest(campaignUid, accountUid));

			if (campaign == null)
				return new NotFoundObjectResult(new ApiResponse(null, 404) { message = "Campaign not found." });

			return new OkObjectResult(new ApiResponse(campaign, 200));
		}

		[HttpGet("campaigns/list")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Campaign>))]
		public async Task<ActionResult<Campaign>> GetListAsync([FromBody] CampaignUidList campaignUids)
		{
			var campaigns = await _mediator.Send(new GetCampaigns(campaignUids.CampaignUids));

			return new OkObjectResult(new ApiResponse(campaigns, 200));
		}

		[HttpPut("campaigns/{campaignUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Campaign))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[Authorize]
		public async Task<ActionResult<Campaign>> UpdateAsync([FromRoute] Guid campaignUid, UpdateCampaignRequest request)
		{
			var accountUid = User.GetAccountUidFromCliam("user_id");

			var campaign = await _mediator.Send(new GetCampaignByUidRequest(campaignUid, accountUid));

			if (campaign == null)
				return new NotFoundObjectResult(new ApiResponse(null, 404) { message = "Campaign not found." });

			campaign = await _mediator.Send(new UpdateCampaignViaUidRequest(campaignUid, accountUid, request));

			return new OkObjectResult(new ApiResponse(campaign, 200));
		}
	}
}

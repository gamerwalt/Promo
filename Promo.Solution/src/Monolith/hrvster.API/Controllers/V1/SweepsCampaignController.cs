using hrvster.Core.Requests;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Configuration.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class SweepsCampaignController : Controller
	{
		private readonly ILogger<SweepsCampaignController> _logger;
		private readonly IMediator _mediator;

		public SweepsCampaignController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepsCampaignController>();
			_mediator = mediator;
		}

		[HttpGet("sweeps/campaign/{campaignUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<Sweep>))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[Authorize]
		public async Task<ActionResult<IEnumerable<Sweep>>> GetByCampaign([FromRoute] Guid campaignUid)
		{
			var accountUid = User.GetAccountUidFromCliam("user_id");

			var sweeps = await _mediator.Send(new GetSweepsForCampaignUid(campaignUid, accountVerfierUid: accountUid));

			return new OkObjectResult(new ApiResponse(sweeps, 200));
		}
	}
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core.Models;
using Promo.Games.Core.RequestHandlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Sweep = Promo.Games.Infrastructure.Models.Sweep;

namespace Promo.Games.API.Controllers.v1
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
			var sweeps = await _mediator.Send(new GetSweepsForCampaignUid(campaignUid));

			return new OkObjectResult(new ApiResponse(sweeps, 200));
		}
	}
}

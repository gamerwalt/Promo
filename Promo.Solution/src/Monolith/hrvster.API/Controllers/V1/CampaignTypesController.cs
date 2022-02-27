using hrvster.Core.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API
{
	[ApiController]
	[Route("v1")]
	public class CampaignTypesController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<CampaignTypesController> _logger;

		public CampaignTypesController(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_mediator = mediator;
			_logger = loggerFactory.CreateLogger<CampaignTypesController>();
		}

		[HttpGet("campaigntypes")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CampaignType))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		public async Task<ActionResult<IEnumerable<CampaignType>>> GetAllAsync()
		{
			var campaignTypes = await _mediator.Send(new GetCampaignTypesRequest());

			return new OkObjectResult(new ApiResponse(campaignTypes, 200));
		}
	}
}

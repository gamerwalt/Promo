using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Promo.Rules.Core.RequestHandlers;
using Promo.Rules.Core.Requests;
using Promo.Rules.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Rules.API.Controllers
{
	[Route("v1/sweeps")]
	[ApiController]
	public class DrawRuleController : ControllerBase
	{
		private readonly IMediator _mediator;

		public DrawRuleController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("drawrules/list")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DrawRule>))]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<IEnumerable<DrawRule>>> GetSweepDrawRules([FromBody] SweepList sweepUids)
		{
			var drawRules = await _mediator.Send(new GetSweepDrawRules(sweepUids.SweepUids));

			return new OkObjectResult(drawRules);
		}
	}
}

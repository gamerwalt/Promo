using hrvster.Core.Requests;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[Route("v1/sweeps")]
	[ApiController]
	public class SweepDrawRuleController : ControllerBase
	{
		private readonly IMediator _mediator;

		public SweepDrawRuleController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet("drawrules/list")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DrawRule>))]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<IEnumerable<DrawRule>>> GetSweepDrawRules([FromBody] List<Guid> sweepUids)
		{
			var drawRules = await _mediator.Send(new GetSweepDrawRules(sweepUids));

			return new OkObjectResult(drawRules);
		}
	}
}

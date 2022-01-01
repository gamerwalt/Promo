using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Reporting.Core.RequestHandlers;
using Promo.Reporting.Core.Requests;
using Promo.Reporting.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Reporting.API.Controllers
{
    [ApiController]
    [Route("v1/reporting")]
    public class ReportingController : ControllerBase
    {
		private readonly IMediator _mediator;

        public ReportingController(IMediator mediator)
        {
			_mediator = mediator;
        }

		[HttpPost("sweeps")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SweepEntryReport))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<SweepEntryReport>> AddSweepEntry([FromBody] CreateSweepEntryReport request)
		{
			await _mediator.Send(new AddSweepEntryReport(request));

			return Ok();
		}
	}
}

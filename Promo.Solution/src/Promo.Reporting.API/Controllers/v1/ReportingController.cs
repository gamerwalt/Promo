using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promo.Reporting.Core.RequestHandlers;
using Promo.Reporting.Core.Requests;
using Promo.Reporting.Infrastructure.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Reporting.API.Controllers.v1
{
	[ApiController]
	[Route("v1/reporting")]
	[Authorize]
	public class ReportingController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ReportingController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpPost("sweeps")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult> AddSweepEntry([FromBody] CreateSweepEntryReport request)
		{
			await _mediator.Send(new AddSweepEntryReport(request));

			return Ok();
		}

		[HttpGet("campaign/{campaignUid:Guid}/sweeps/{sweepUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(SweepEntryReport))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult> GetEntry([FromRoute] Guid campaignUid, [FromRoute] Guid sweepUid, [FromBody] GetEntryRequest request)
		{
			var entryReport = await _mediator.Send(new GetSweepEntryReport(request, campaignUid, sweepUid));

			return Ok(entryReport);
		}
	}
}

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promo.Reporting.Core.RequestHandlers;
using Promo.Reporting.Core.Requests;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Reporting.API.Controllers
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
	}
}

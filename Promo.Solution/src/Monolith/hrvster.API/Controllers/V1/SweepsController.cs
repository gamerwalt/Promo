using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Configuration.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class SweepsController : ControllerBase
	{
		private readonly ILogger<SweepsController> _logger;
		private readonly IMediator _mediator;

		public SweepsController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<SweepsController>();
			_mediator = mediator;
		}

		[HttpPost("sweeps")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[Authorize]
		public async Task<ActionResult<Sweep>> Create([FromBody] NewSweepRequest request)
		{
			var accountUid = User.GetAccountUidFromCliam("user_id");
			request.AccountUid = accountUid;
			
			var sweep = await _mediator.Send(new CreateSweep(request));

			return new OkObjectResult(new ApiResponse(sweep, 200));
		}

		[HttpGet("sweeps/{sweepUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[Authorize]
		public async Task<ActionResult<Sweep>> Get([FromRoute] Guid sweepUid)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
				return new NotFoundObjectResult(new { message = $"Sweep with Uid: {sweepUid} does not exist." });

			var accountUid = User.GetAccountUidFromCliam("user_id");

			if(sweep.Account?.AccountUid != accountUid)
			{
				_logger.LogWarning("Account Uid {accountUid} does not own sweep {sweepName} with Uid {sweepUid}", accountUid, sweep.Name, sweep.SweepUid);

				return new NotFoundObjectResult(new { message = $"Sweep with Uid: {sweepUid} does not exist." });
			}

			return new OkObjectResult(new ApiResponse(sweep, 200));
		}

		[HttpPut("sweeps/{sweepUid:Guid}")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Sweep))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[ProducesResponseType((int)HttpStatusCode.NotFound)]
		[Authorize]
		public async Task<ActionResult<Sweep>> Update([FromRoute] Guid sweepUid, [FromBody] UpdateSweepRequest request)
		{
			var sweep = await _mediator.Send(new GetSweepByUid(sweepUid));
			if (sweep == null)
				return new NotFoundObjectResult(new { message = $"Sweep with Uid: {sweepUid} does not exist." });

			var accountUid = User.GetAccountUidFromCliam("user_id");
			if (sweep.Account?.AccountUid != accountUid)
			{
				_logger.LogWarning("Account Uid {accountUid} does not own sweep {sweepName} with Uid {sweepUid}", accountUid, sweep.Name, sweep.SweepUid);

				return new NotFoundObjectResult(new { message = $"Sweep with Uid: {sweepUid} does not exist." });
			}

			var updatedSweep = await _mediator.Send(new UpdateSweep(sweepUid, sweep, request));

			return new OkObjectResult(new ApiResponse(updatedSweep, 200));
		}
	}
}

using hrvster.Core.Requests;
using hrvster.Core.Requests.ApiRequests;
using hrvster.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hrvster.API.Controllers.v1
{
	[ApiController]
	[Route("V1")]
	public class AccountsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly ILogger<AccountsController> _logger;

		public AccountsController(ILoggerFactory loggerFactory, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<AccountsController>();
			_mediator = mediator;
		}

		[HttpPost("accounts")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
		public async Task<ActionResult<Account>> Create([FromBody] NewAccountRequest newAccount)
		{
			var userUid = Guid.NewGuid();
			_logger.LogInformation("Create account request received for AccountUid : {accountUid}", userUid);

			await _mediator.Send(new CreateAccountRequest(newAccount, userUid));

			var account = await _mediator.Send(new GetAccountRequest(userUid));

			return new OkObjectResult(new ApiResponse(account, 200));
		}

		[HttpGet("accounts")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[Authorize]
		public async Task<ActionResult<Account>> Get()
		{
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type.Equals("user_id"));

			if (userIdClaim == null) return new BadRequestResult();

			var account = await _mediator.Send(new GetAccountRequest(Guid.Parse(userIdClaim.Value)));

			return new OkObjectResult(new ApiResponse(account, 200));
		}
	}
}

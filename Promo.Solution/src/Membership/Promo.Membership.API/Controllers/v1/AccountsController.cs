using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promo.Clients.External;
using Promo.Core.Models;
using Promo.Membership.Core.RequestHandlers;
using Promo.Membership.Core.Requests;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Account = Promo.Membership.Infrastructure.Models.Account;

namespace Promo.Membership.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class AccountsController : ControllerBase
	{
		private readonly IIdentityClient _identityClient;
		private readonly IMediator _mediator;

		public AccountsController(IIdentityClient identityClient, IMediator mediator)
		{
			_identityClient = identityClient;
			_mediator = mediator;
		}

		[HttpPost("accounts")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
		public async Task<ActionResult<Account>> AddAccount([FromBody] NewAccountRequest newAccount)
		{
			var userUid = Guid.NewGuid();
			await _mediator.Send(new CreateAccountRequest(newAccount, userUid));

			var account = await _mediator.Send(new GetAccountRequest(userUid));

			return new OkObjectResult(new ApiResponse(account, 200));
		}

		[HttpGet("accounts")]
		[Produces("application/json")]
		[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
		[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
		[Authorize]
		public async Task<ActionResult<Account>> GetAccount()
		{
			var userIdClaim = User.Claims.FirstOrDefault(c => c.Type.Equals("user_id"));

			if (userIdClaim == null) return new BadRequestResult();

			var account = await _mediator.Send<Account>(new GetAccountRequest(Guid.Parse(userIdClaim.Value)));

			return new OkObjectResult(new ApiResponse(account, 200));
		}
	}
}

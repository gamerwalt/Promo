using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Core.ApiRequests;
using Promo.Core.CoreRequestHandlers;
using Promo.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Web.API.Controllers
{
    [ApiController]
    [Route("v1")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IMediator _mediator;

        public AccountsController(
            ILogger<AccountsController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("accounts")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Account))]
        public async Task<ActionResult<Account>> AddAccount([FromBody] NewAccountRequest newAccount)
        {
            var userUid = Guid.NewGuid();
            await _mediator.Send(new CreateAccountRequest(newAccount, userUid));

            return new OkObjectResult(new Account());
        }
    }
}

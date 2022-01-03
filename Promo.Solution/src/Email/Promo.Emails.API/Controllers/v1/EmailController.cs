using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Promo.Clients.External;
using Promo.Core.Authentication;
using Promo.Core.Models;
using Promo.Emails.Core.RequestHandlers;
using System.Net;
using System.Threading.Tasks;

namespace Promo.Emails.API.Controllers.v1
{
    [ApiController]
    [Route("v1")]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;
        private readonly IIdentityClient _identityClient;

        public EmailController(IMediator mediator, IConfiguration config, IIdentityClient identityClient)
        {
            _mediator = mediator;
            _config = config;
            _identityClient = identityClient;
        }

        [HttpPost("emails")]
        [Produces("application/json")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Email))]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize]
        public async Task<ActionResult<Email>> GetAllAsync([FromBody] Email email)
        {
            await _mediator.Send(new CreateEmailRequest(email));

            return new OkObjectResult(email);
        }
    }
}

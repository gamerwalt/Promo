using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Promo.Clients.External;
using Promo.Core.Authentication;
using System.Threading.Tasks;

namespace Promo.Membership.API.Controllers.v1
{
	[ApiController]
	[Route("v1")]
	public class SessionsController : Controller
	{
		private readonly ILogger<SessionsController> _logger;
		private readonly IIdentityClient _identityClient;

		public SessionsController(ILoggerFactory loggerFactory, IIdentityClient identityClient)
		{
			_identityClient = identityClient;
			_logger = loggerFactory.CreateLogger<SessionsController>();
		}

		[HttpPost("sessions")]
		[Produces("application/json")]
		public async Task<ActionResult> SignIn([FromBody] AuthenticateRequest request)
		{
			request.ReturnSecureToken = true;

			var response = await _identityClient.SignIn(request);

			return response != null ? new OkObjectResult(response) : new UnauthorizedResult();
		}
	}
}

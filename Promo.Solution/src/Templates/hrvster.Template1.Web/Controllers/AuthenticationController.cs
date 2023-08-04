using hrvster.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace hrvster.Template1.Web.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly ILogger<AuthenticationController> _logger;
		private readonly ISessionManager _sessionManager;

		public AuthenticationController(ILoggerFactory loggerFactory, ISessionManager sessionManager)
		{
			_logger = loggerFactory.CreateLogger<AuthenticationController>();
			_sessionManager = sessionManager;
		}

		[HttpGet("/login/external/{providerCode}")]
		public async Task<IActionResult> ExternalLogin([FromQuery] string providerCode)
		{
			return new OkObjectResult(new { message = "ExternalLogin" });
		}
	}
}

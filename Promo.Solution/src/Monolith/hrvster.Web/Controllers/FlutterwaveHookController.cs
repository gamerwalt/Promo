using hrvster.Models.Extensions;
using hrvster.Models.Options;
using hrvster.Models.Subscriptions.Flutterwave;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[Route("api/flutterwave/webhook")]
	[ApiController]
	public class FlutterwaveHookController : ControllerBase
	{
		private readonly ILogger<FlutterwaveHookController> _logger;
		private readonly FlutterwaveConfigurations _flutterwaveConfigurations;

		public FlutterwaveHookController(ILoggerFactory loggerFactory, FlutterwaveConfigurations flutterwaveConfigurations)
		{
			_logger = loggerFactory.CreateLogger<FlutterwaveHookController>();
			_flutterwaveConfigurations = flutterwaveConfigurations;
		}

		[HttpGet]
		[Route("ping")]
		public async Task<IActionResult> Ping()
		{
			return new OkObjectResult(new { message = "All good." });
		}

		[HttpPost]
		[Route("/events")]
		public async Task<IActionResult> Receive(FlutterwaveEvent flutterwaveEvent)
		{
			var secretHash = Request.Headers.FirstOrDefault(c => c.Key.Equals("verif-hash")).Value;

			_logger.LogInformation("Secret hash {secretHash} received from flutterwave.", secretHash);

			if (string.IsNullOrWhiteSpace(secretHash)) return Ok();

			if (secretHash != _flutterwaveConfigurations.SecretWebhookHash) return Ok();

			_logger.LogInformation("Received information from Flutterwave: Details: {flutterwaveEvent}", flutterwaveEvent.ToJsonString());

			return Ok();
		}
	}
}

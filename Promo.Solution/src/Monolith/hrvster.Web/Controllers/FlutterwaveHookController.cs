using hrvster.ApplicationServices.Subscription;
using hrvster.Models.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[ApiController]
	public class FlutterwaveHookController : Controller
	{
		private readonly ILogger<FlutterwaveHookController> _logger;
		private readonly FlutterwaveConfigurations _flutterwaveConfigurations;
		private readonly ISubscriptionService _subscriptionService;

		public FlutterwaveHookController(
			ILoggerFactory loggerFactory,
			FlutterwaveConfigurations flutterwaveConfigurations,
			ISubscriptionService subscriptionService)
		{
			_logger = loggerFactory.CreateLogger<FlutterwaveHookController>();
			_flutterwaveConfigurations = flutterwaveConfigurations;
			_subscriptionService = subscriptionService;
		}

		[HttpGet]
		[Route("/FlutterwaveHook/ping")]
		public async Task<IActionResult> Ping()
		{
			_logger.LogInformation("Received Ping Request");
			return new OkObjectResult(new { message = "OK" });

		}

		[HttpPost]
		[Route("/FlutterwaveHook/events")]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Receive([FromBody] dynamic transactionEvent)
		{
			var secretHash = Request.Headers.FirstOrDefault(c => c.Key.Equals("verif-hash")).Value.FirstOrDefault();

			_logger.LogInformation("Secret hash {secretHash} received from flutterwave.", secretHash);

			if (string.IsNullOrWhiteSpace(secretHash)) return Ok();

			if (secretHash != _flutterwaveConfigurations.SecretWebhookHash) return Ok();

			var flutterwaveEventString = (string)Convert.ToString(transactionEvent);

			_logger.LogInformation("Received the event {flutterwaveEvent} from Flutterwave", flutterwaveEventString);

			await _subscriptionService.SaveFlutterwaveEventAsync(flutterwaveEventString);

			_logger.LogInformation("The flutterwave event {flutterwaveEvent} has been saved.", flutterwaveEventString);

			return Ok(transactionEvent);
		}
	}
}

using hrvster.ApplicationServices.Stripe;
using hrvster.Core.Requests.WebRequests.Stripe;
using hrvster.Models.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Web.Controllers
{
	[ApiController]
	public class StripeHookController : Controller
	{
		private readonly ILogger<StripeHookController> _logger;
		private readonly StripeOptions _stripeOptions;
		private readonly IMediator _mediator;

		public StripeHookController(ILoggerFactory loggerFactory, StripeOptions stripeOptions, IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<StripeHookController>();
			_stripeOptions = stripeOptions;
			_mediator = mediator;
		}

		[HttpGet]
		[Route("/stripehook/ping")]
		public async Task<IActionResult> Ping()
		{
			_logger.LogInformation("Received Ping Request");

			return new OkObjectResult(new { message = "OK" });
		}

		[HttpPost]
		[Route("/stripehook/events")]
		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Receive([FromBody] dynamic transactionEvent, CancellationToken cancellationToken)
		{
			var stripeEvent = (string)Convert.ToString(transactionEvent);

			_logger.LogInformation("Received an event from Stripe");

			_logger.LogInformation("Saving the event into the database.");

			var rawEvent = EventUtility.ConstructEvent(stripeEvent, Request.Headers["Stripe-Signature"].ToString(), _stripeOptions.WebhookEndpointSecret);

			await _mediator.Send(new ProcessStripeEventRequest(rawEvent, cancellationToken));

			return Ok(stripeEvent);
		}
	}
}

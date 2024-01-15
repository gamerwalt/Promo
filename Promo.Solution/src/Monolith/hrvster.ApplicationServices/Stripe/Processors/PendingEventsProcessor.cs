using hrvster.Infrastructure.Stripe;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using Stripe;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Stripe.Processors
{
	public interface IPendingEventsProcessor
	{
		Task ProcessAsync(CancellationToken token);
	}

	public class PendingEventsProcessor : IPendingEventsProcessor
	{
		private readonly ILogger<PendingEventsProcessor> _logger;
		private readonly IStripeEventRepository _repository;
		private readonly StripeOptions _stripeOptions;

		public PendingEventsProcessor(ILoggerFactory loggerFactory, IStripeEventRepository repository, StripeOptions stripeOptions)
		{
			_logger = loggerFactory.CreateLogger<PendingEventsProcessor>();
			_repository = repository;
			_stripeOptions = stripeOptions;
		}

		public async Task ProcessAsync(CancellationToken token)
		{
			_logger.LogInformation("Retrieving pending stripe events from database.");

			var events = await _repository.GetEventsByStatus(StripeEventStatus.New);

			var eventList = events.ToList();

			_logger.LogInformation("Retrieved a total of {total} events to process.", eventList.Count);

			foreach (var stripeEvent in eventList)
			{
				_logger.LogInformation("Started processing the event with uid {eventUid}", stripeEvent.Uid);

				var rawEvent = EventUtility.ConstructEvent(stripeEvent.RawEvent, stripeEvent.SignatureHeader, _stripeOptions.WebhookEndpointSecret);

				_logger.LogInformation("Finished processing the event with uid {eventUid}", stripeEvent.Uid);
			}
		}
	}
}

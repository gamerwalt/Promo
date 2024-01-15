using hrvster.ApplicationServices.Stripe.Processors;
using hrvster.Infrastructure.Stripe;
using hrvster.Models.Stripe;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Stripe
{
	public interface IStripeEventService
	{
		Task CreateAsync(string stripeEvent, Microsoft.Extensions.Primitives.StringValues signatureHeaders);
		Task ProcessPendingEventsAsync(CancellationToken token);
	}

	public class StripeEventService : IStripeEventService
	{
		private readonly ILogger<StripeEventService> _logger;
		private readonly IStripeEventRepository _repository;
		private readonly IPendingEventsProcessor _pendingEventsProcessor;

		public StripeEventService(ILoggerFactory logger, IStripeEventRepository repository, IPendingEventsProcessor pendingEventsProcessor)
		{
			_logger = logger.CreateLogger<StripeEventService>();
			_repository = repository;
			_pendingEventsProcessor = pendingEventsProcessor;
		}

		public async Task CreateAsync(string stripeEvent, Microsoft.Extensions.Primitives.StringValues signatureHeaders)
		{
			var eventUid = Guid.NewGuid();

			_logger.LogInformation("Saving the stripe event into the database with uid {uid}", eventUid);

			var @event = new StripeEvent
			{
				Uid = eventUid,
				Status = Models.StripeEventStatus.New,
				RawEvent = stripeEvent,
			};

			await _repository.SaveAsync(@event, signatureHeaders.ToString());

			_logger.LogInformation("The stripe event has been saved with the uid {uid}", eventUid);
		}

		public async Task ProcessPendingEventsAsync(CancellationToken token)
		{
			_logger.LogInformation($"Started processing pending events.");

			await _pendingEventsProcessor.ProcessAsync(token);

			_logger.LogInformation($"Finished processing pending events");
		}
	}
}

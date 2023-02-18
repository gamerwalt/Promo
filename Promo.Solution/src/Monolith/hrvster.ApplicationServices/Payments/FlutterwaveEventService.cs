using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Payments
{
	public interface IFlutterwaveEventService
	{
		Task ProcessFlutterwaveEventsAsync(CancellationToken cancellationToken);
	}

	public class FlutterwaveEventService : IFlutterwaveEventService
	{
		private readonly ILogger<FlutterwaveEventService> _logger;
		private readonly IFlutterwaveEventRepository _flutterwaveEventRepository;
		private readonly IFlutterwaveCardTransactionService _cardService;
		private readonly IFlutterwaveAccountSubscriptionRepository _flutterwaveAccountSubscriptionRepository;

		public FlutterwaveEventService(
			ILoggerFactory loggerFactory,
			IFlutterwaveEventRepository flutterwaveEventRepository,
			IFlutterwaveCardTransactionService cardService,
			IFlutterwaveAccountSubscriptionRepository flutterwaveAccountSubscriptionRepository)
		{
			_logger = loggerFactory.CreateLogger<FlutterwaveEventService>();
			_flutterwaveEventRepository = flutterwaveEventRepository;
			_cardService = cardService;
			_flutterwaveAccountSubscriptionRepository = flutterwaveAccountSubscriptionRepository;
		}

		public async Task ProcessFlutterwaveEventsAsync(CancellationToken cancellationToken)
		{
			var newEvents = await _flutterwaveEventRepository.GetNewEventsAsync(cancellationToken);

			_logger.LogInformation("Retrieved a total of {eventCount} flutterwave events to process.", newEvents.Count());

			await ProcessNewEventsAsync(newEvents, cancellationToken);
		}

		private async Task ProcessNewEventsAsync(IEnumerable<FlutterwaveEvent> newEvents, CancellationToken cancellationToken)
		{
			var exceptions = new List<Exception>();

			foreach(var flutterwaveEvent in newEvents)
			{
				var eventUid = flutterwaveEvent.FlutterwaveEventUid;

				try
				{

					_logger.LogInformation("Trying to deserialize the event {event} for Event Uid {uid}",
						flutterwaveEvent.Payload,
						eventUid);

					await _flutterwaveEventRepository.UpdateEventStatusAsync(
						eventUid, FlutterwaveEventStatus.Processing, cancellationToken);

					var flutterwaveCardTransaction = default(FlutterwaveCardTransaction);
				
					flutterwaveCardTransaction = JsonConvert.DeserializeObject<FlutterwaveCardTransaction>(flutterwaveEvent.Payload);

					if (flutterwaveCardTransaction == null)
					{
						//retry another deserialization
					}
					else
					{
						//Save customer details here as well as external Ids that can be used later for unsubscriptions
						//await _flutterwaveAccountSubscriptionRepository.CreateAsync(GetClientAccountSubscriptionFrom(flutterwaveCardTransaction), cancellationToken);

						if (await _cardService.ProcessAsync(eventUid, flutterwaveCardTransaction, cancellationToken))
							await _flutterwaveEventRepository.UpdateEventStatusAsync(
								eventUid, FlutterwaveEventStatus.Success, cancellationToken);
						else
							await _flutterwaveEventRepository.UpdateEventStatusAsync(
								eventUid, FlutterwaveEventStatus.Failed, cancellationToken);
					}
				}
				catch (Exception ex)
				{
					exceptions.Add(ex);

					await _flutterwaveEventRepository.UpdateEventStatusAsync(
						eventUid, FlutterwaveEventStatus.Failed, cancellationToken);
				}

				
			}

			if(exceptions.Count > 0)
			{
				_logger.LogWarning("There was a total of {count} exceptions that occurred.", exceptions.Count);
				foreach(var exception in exceptions)
				{
					_logger.LogWarning(exception.Message);
				}
			}
		}
	}
}

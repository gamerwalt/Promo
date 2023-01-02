using hrvster.ApplicationServices.Subscription;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Models.Subscriptions.Flutterwave;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Payments
{
	public interface IFlutterwaveCardTransactionService
	{
		Task<bool> ProcessAsync(Guid flutterwaveEventUid, FlutterwaveCardTransaction cardTransaction, CancellationToken cancellationToken);
	}

	public class FlutterwaveCardTransactionService : IFlutterwaveCardTransactionService
	{
		private readonly ILogger<FlutterwaveCardTransactionService> _logger;
		private ISubscriptionService _subscriptionService;
		private readonly IFlutterwaveCardTransactionRepository _cardTransactionRepository;

		public FlutterwaveCardTransactionService(
			ILoggerFactory loggerFactory,
			ISubscriptionService subscriptionService,
			IFlutterwaveCardTransactionRepository cardTransactionRepository)
		{
			_logger = loggerFactory.CreateLogger<FlutterwaveCardTransactionService>();
			_subscriptionService = subscriptionService;
			_cardTransactionRepository = cardTransactionRepository;
		}

		public async Task<bool> ProcessAsync(Guid flutterwaveEventUid, FlutterwaveCardTransaction cardTransaction, CancellationToken cancellationToken)
		{
			_logger.LogWarning("Processing the flutterwave event with uid : {flutterwaveEventUid}", flutterwaveEventUid);

			var existingCardTransaction = await _cardTransactionRepository.GetAsync(cardTransaction.Id, cardTransaction.TxRef, cardTransaction.FlwRef, cardTransaction.OrderRef, cancellationToken);

			if(existingCardTransaction != null)
			{
				_logger.LogWarning("We have already processed the flutterwave event with uid {flutterwaveEventUid} we received. Card Transaction: {cardTranscation}, Existing Card Transaction Processed: {existingCardTransaction}",
					flutterwaveEventUid,
					cardTransaction.ToJsonString(),
					existingCardTransaction.ToJsonString());

				return true;
			}

			var cardTransactionUid = Guid.NewGuid();
			await _cardTransactionRepository.CreateAsync(cardTransactionUid, cardTransaction.Id, cardTransaction.TxRef, cardTransaction.FlwRef, cardTransaction.OrderRef, cancellationToken);

			_logger.LogInformation("Continuing to process the flutterwave event with uid {flutterwaveEventUid}.", flutterwaveEventUid);

			await _subscriptionService.ProcessFlutterwaveTransactionAsync(flutterwaveEventUid, cardTransaction);

			_logger.LogInformation("Finished processing the flutterwave event with uid {flutterwaveEventUid}.", flutterwaveEventUid);

			return true;
		}
	}
}

using hrvster.Infrastructure.Extensions;
using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IFlutterwaveCardTransactionRepository
	{
		Task CreateAsync(Guid cardTransactionUid, long id, string txRef, string flwRef, string orderRef, long amount, long chargedAmount, string status, string currency, long paymentPlan, long paymentPage, CancellationToken cancellationToken);
		Task<FlutterwaveCardTransaction> GetAsync(long id, string txRef, string flwRef, string orderRef, CancellationToken cancellationToken);
	}

	public class FlutterwaveCardTransactionRepository : IFlutterwaveCardTransactionRepository
	{
		private readonly IDatabase _database;
		public FlutterwaveCardTransactionRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(
			Guid cardTransactionUid, 
			long id, 
			string txRef, 
			string flwRef, 
			string orderRef, 
			long amount, 
			long chargedAmount, 
			string status, 
			string currency, 
			long paymentPlan, 
			long paymentPage, 
			CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "flutterwave_card_transactions_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("uid", cardTransactionUid)
				.AddParameter("id", id)
				.AddParameter("transactionReference", txRef)
				.AddParameter("flowReference", flwRef)
				.AddParameter("orderReference", orderRef)
				.AddParameter("amount", amount)
				.AddParameter("chargedAmount", chargedAmount)
				.AddParameter("transactionStatus", status)
				.AddParameter("currency", currency)
				.AddParameter("paymentPlan", paymentPlan)
				.AddParameter("paymentPage", paymentPage);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<FlutterwaveCardTransaction> GetAsync(
			long id, 
			string txRef, 
			string flwRef, 
			string orderRef, 
			CancellationToken cancellationToken)
		{
			var account = default(FlutterwaveCardTransaction);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "flutterwave_card_transactions_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("id", id)
					.AddParameter("txRef", txRef)
					.AddParameter("flwRef", flwRef)
					.AddParameter("orderRef", orderRef);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					account = CreateCardTransactionFrom(reader);
				}
			}

			return account;
		}

		private FlutterwaveCardTransaction CreateCardTransactionFrom(MySqlDataReader reader)
		{
			return new FlutterwaveCardTransaction
			{
				Id = reader.GetLongValue("id"),
				TxRef = reader.GetStringValue("transaction_reference"),
				FlwRef = reader.GetStringValue("flow_reference"),
				OrderRef = reader.GetStringValue("order_reference")
			};
		}
	}
}

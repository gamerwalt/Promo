using hrvster.Infrastructure.Extensions;
using hrvster.Models.Subscriptions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IFlutterwaveAccountSubscriptionRepository
	{
		Task CreateAsync(ClientAccountSubscription model, CancellationToken cancellationToken);
		Task<ClientAccountSubscription> GetAccountSubscriptionAsync(Guid subscriptionUid, CancellationToken cancellationToken);
	}

	public class FlutterwaveAccountSubscriptionRepository : IFlutterwaveAccountSubscriptionRepository
	{
		private readonly IDatabase _database;
		public FlutterwaveAccountSubscriptionRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(ClientAccountSubscription model, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_subscription_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("Uid", model.Uid)
				.AddParameter("subscriptionUid", model.SubscriptionUid)
				.AddParameter("externalAccountId", model.ExternalAccountId)
				.AddParameter("externalSubscriptionId", model.ExternalSubscriptionId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<ClientAccountSubscription> GetAccountSubscriptionAsync(Guid subscriptionUid, CancellationToken cancellationToken)
		{
			var account = default(ClientAccountSubscription);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "flutterwave_account_subscription_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("subscriptionUid", subscriptionUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					account = CreateFrom(reader);
				}
			}

			return account;
		}

		private ClientAccountSubscription CreateFrom(MySqlDataReader reader)
		{
			return new ClientAccountSubscription
			{
				Id = reader.GetInt32("Id"),
				Uid = reader.GetGuid("uid"),
				SubscriptionUid = reader.GetGuid("subscription_uid"),
				ExternalAccountId = reader.GetString("external_account_id"),
				ExternalSubscriptionId = reader.GetString("external_subscription_id")
			};
		}
	}
}

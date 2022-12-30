using hrvster.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;

namespace hrvster.Infrastructure
{
	public interface IPaymentServiceRepository
	{
		IEnumerable<PaymentServiceModel> GetPaymentServices();
	}

	public class PaymentServiceRepository : IPaymentServiceRepository
	{
		private readonly IDatabase _database;

		public PaymentServiceRepository(IDatabase database)
		{
			_database = database;
		}

		public IEnumerable<PaymentServiceModel> GetPaymentServices()
		{
			var paymentServices = new List<PaymentServiceModel>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "payment_services_get";
				command.CommandType = CommandType.StoredProcedure;

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					paymentServices.Add(CreateServiceFrom(reader));
				}
			}

			return paymentServices;
		}

		private PaymentServiceModel CreateServiceFrom(MySqlDataReader reader)
		{
			return new PaymentServiceModel
			{
				PaymentServiceId = reader.GetInt32("payment_service_id"),
				PaymentServiceUid = reader.GetGuid("payment_service_uid"),
				PaymentServiceName = reader.GetString("payment_service_name"),
				PaymentServiceNamespace = reader.GetString("payment_service_namespace"),
				EnabledIndicator = reader.GetBoolean("enabled_ind")
			};
		}
	}
}

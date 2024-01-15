using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Stripe;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Stripe
{
	public interface IStripeEventRepository
	{
		Task<IEnumerable<StripeEvent>> GetEventsByStatus(StripeEventStatus eventStatus);
		Task SaveAsync(StripeEvent stripeEvent, string signatureHeaders);
		Task SaveAsync(StripeEvent rawEvent);
		Task CreateRawEventAsync(StripeRawEvent stripeRawEvent);
	}

	public class StripeEventRepository : IStripeEventRepository
	{
		private readonly IDatabase _database;

		public StripeEventRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateRawEventAsync(StripeRawEvent stripeRawEvent)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "stripe_raw_event_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("uid", stripeRawEvent.Uid)
				.AddParameter("externalId", stripeRawEvent.ExternalId)
				.AddParameter("objectType", stripeRawEvent.ObjectType)
				.AddParameter("dataObjectType", stripeRawEvent.DataObjectType)
				.AddParameter("currency", stripeRawEvent.Currency)
				.AddParameter("customerEmail", stripeRawEvent.CustomerEmail)
				.AddParameter("externalCustomerId", stripeRawEvent.ExternalCustomerId)
				.AddParameter("currentPeriodStart", stripeRawEvent.CurrentPeriodStart)
				.AddParameter("currentPeriodEnd", stripeRawEvent.CurrentPeriodEnd)
				.AddParameter("externalPlanId", stripeRawEvent.ExternalPlanId)
				.AddParameter("amount", stripeRawEvent.Amount)
				.AddParameter("externalProductId", stripeRawEvent.ExternalProductId)
				.AddParameter("externalInvoiceId", stripeRawEvent.ExternalInvoiceId)
				.AddParameter("externalStatus", stripeRawEvent.ExternalStatus);

			await conn.OpenAsync();
			await command.ExecuteReaderAsync();
		}

		public async Task<IEnumerable<StripeEvent>> GetEventsByStatus(StripeEventStatus eventStatus)
		{
			var events = new List<StripeEvent>();

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "stripe_events_get_by_status";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("eventStatus", eventStatus.ToString());

			await conn.OpenAsync();
			using (var reader = await command.ExecuteReaderAsync())
			{
				while (await reader.ReadAsync())
				{
					events.Add(CreateStripeEventFrom(reader));
				}
			}

			return events;
		}

		public async Task SaveAsync(StripeEvent stripeEvent, string signatureHeaders)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "stripe_event_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("stripeEventUid", stripeEvent.Uid)
				.AddParameter("rawEvent", stripeEvent.RawEvent)
				.AddParameter("eventStatus", stripeEvent.Status)
				.AddParameter("signatureHeader", signatureHeaders);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task SaveAsync(StripeEvent rawEvent)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "stripe_event_create_details";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("stripeEventUid", rawEvent.Uid)
				.AddParameter("rawEvent", rawEvent.RawEvent)
				.AddParameter("eventStatus", rawEvent.Status);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private StripeEvent CreateStripeEventFrom(MySqlDataReader reader)
		{
			return new StripeEvent
			{
				Uid = reader.GetGuid("stripe_event_uid"),
				Id = reader.GetInt32("stripe_event_id"),
				RawEvent = reader.GetStringValue("stripe_raw_event"),
				SignatureHeader = reader.GetStringValue("signature_header"),
				Status = (StripeEventStatus)Enum.Parse(typeof(StripeEventStatus), reader.GetString("stripe_event_status")),
				CreatedDate = reader.GetNullableDateTimeValue("created_date").GetValueOrDefault(),
				UpdatedDate = reader.GetNullableDateTimeValue("updated_date").GetValueOrDefault()
			};
		}
	}
}

using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Subscriptions;
using hrvster.Models.Subscriptions.Flutterwave;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Subscriptions
{
	public interface IFlutterwaveEventRepository
	{
		Task CreateAsync(FlutterwaveEvent flutterwaveEvent);
		Task<IEnumerable<FlutterwaveEvent>> GetNewEventsAsync(CancellationToken cancellationToken);
		Task UpdateEventStatusAsync(Guid flutterwaveEventUid, FlutterwaveEventStatus status, CancellationToken cancellationToken);
	}

	public class FlutterwaveEventRepository : IFlutterwaveEventRepository
	{
		private readonly IDatabase _database;
		public FlutterwaveEventRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(FlutterwaveEvent flutterwaveEvent)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "flutterwave_events_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("flutterWaveEventUid", flutterwaveEvent.FlutterwaveEventUid)
				.AddParameter("payload", flutterwaveEvent.Payload)
				.AddParameter("eventStatus", flutterwaveEvent.EventStatus.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<FlutterwaveEvent>> GetNewEventsAsync(CancellationToken cancellationToken)
		{
			var events = new List<FlutterwaveEvent>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "flutterwave_events_get_by_status";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("eventStatus", FlutterwaveEventStatus.New.ToString());

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					events.Add(CreateFlutterwaveEventFrom(reader));
				}
			}

			return events;
		}

		public async Task UpdateEventStatusAsync(
			Guid flutterwaveEventUid,
			FlutterwaveEventStatus status, 
			CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "flutterwave_events_update_status";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("flutterWaveEventUid", flutterwaveEventUid)
				.AddParameter("eventStatus", status.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private FlutterwaveEvent CreateFlutterwaveEventFrom(MySqlDataReader reader)
		{
			return new FlutterwaveEvent
			{
				Payload = reader.GetString("payload_received"),
				EventStatus = Enum.Parse<FlutterwaveEventStatus>(reader.GetString("event_status")),
				FlutterwaveEventUid = reader.GetGuid("flutterwave_event_uid"),
				FlutterwaveEventId = reader.GetInt32("flutterwave_event_id")
			};
		}
	}
}

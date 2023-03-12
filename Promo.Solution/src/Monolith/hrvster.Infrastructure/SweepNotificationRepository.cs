using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface ISweepNotificationRepository
	{
		Task<SweepNotification> GetSweepNotificationAsync(int sweepId, CancellationToken cancellationToken);
		Task ToggleDrawAttemptedAsync(int sweepId, CancellationToken cancellationToken);
		Task ToggleDrawHappenedAsync(int sweepId, CancellationToken cancellationToken);
		Task ToggleMaxEntriesAsync(int sweepId, CancellationToken cancellationToken);
		Task ToggleNotEnoughUniqueEntrantsAsync(int sweepId, CancellationToken cancellationToken);
		Task ToggleSweepsGoingLiveAsync(int sweepId, CancellationToken cancellationToken);
		Task ToggleSweepsLiveAsync(int sweepId, CancellationToken cancellationToken);
	}

	public class SweepNotificationRepository : ISweepNotificationRepository
	{
		private readonly IDatabase _database;

		public SweepNotificationRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<SweepNotification> GetSweepNotificationAsync(int sweepId, CancellationToken cancellationToken)
		{
			var sweepNotification = new SweepNotification();

			using var conn = new MySqlConnection(_database.GetConnectionString());
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_notification_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("sweepId", sweepId);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepNotification = CreateRecordFrom(reader);
					}
				}
			}

			return sweepNotification;
		}

		private SweepNotification CreateRecordFrom(MySqlDataReader reader)
		{
			return new SweepNotification
			{
				SweepNotificationId = reader.GetInt32("sweep_notification_id"),
				Sweep = new Sweep { SweepId = reader.GetInt32("sweep_id")},
				SweepsGoingLive = reader.GetBoolean("sweeps_going_live"),
				SweepsLive = reader.GetBoolean("sweeps_live"),
				DrawAttemptHappened = reader.GetBoolean("draw_attempted"),
				DrawHappened = reader.GetBoolean("draw_happened"),
				NotEnoughUniqueEntrants = reader.GetBoolean("not_enough_unique_entrants"),
				MaxEntries = reader.GetBoolean("max_entries")
			};
		}

		public async Task ToggleDrawAttemptedAsync(int sweepId, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_notification_draw_attempted_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepId", sweepId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task ToggleDrawHappenedAsync(int sweepId, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_notification_draw_happened_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepId", sweepId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task ToggleNotEnoughUniqueEntrantsAsync(int sweepId, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_notification_not_enough_unique_entrants_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepId", sweepId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task ToggleSweepsGoingLiveAsync(int sweepId, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_notification_sweep_going_live_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepId", sweepId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task ToggleSweepsLiveAsync(int sweepId, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_notification_sweep_live_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepId", sweepId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task ToggleMaxEntriesAsync(int sweepId, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_notification_max_entries_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("sweepId", sweepId);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}

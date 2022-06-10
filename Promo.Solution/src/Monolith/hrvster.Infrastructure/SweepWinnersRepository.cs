using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using hrvster.Infrastructure.Extensions;

namespace hrvster.Infrastructure
{
	public interface ISweepWinnersRepository
	{
		Task SaveSweepWinnerAsync(SweepWinner sweepWinner);
		Task<SweepWinner> GetSweepWinnerAsync(Guid sweepWinnerUid);
		Task SaveSweepBackupWinnerAsync(SweepWinner sweepWinner);
		Task<IEnumerable<SweepBackupWinner>> GetSweepBackupWinnersAsync(Guid sweepWinnerUid);
		Task<IEnumerable<SweepWinner>> GetSweepWinnersAsync(Guid sweepUid);
	}

	public class SweepWinnersRepository : ISweepWinnersRepository
	{
		private readonly IDatabase _database;

		public SweepWinnersRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task SaveSweepWinnerAsync(SweepWinner sweepWinner)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_winners_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepId", sweepWinner.Sweep.SweepId)
				.AddParameter("sweepWinnerUid", sweepWinner.SweepWinnerUid)
				.AddParameter("sweepEntryUid", sweepWinner.SweepEntry.SweepEntryUid)
				.AddParameter("assignedId", sweepWinner.AssignedId)
				.AddParameter("userIdentifier", sweepWinner.UserIdentifier)
				.AddParameter("userIdentifierType", sweepWinner.UserIdentifierType.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SweepWinner> GetSweepWinnerAsync(Guid sweepWinnerUid)
		{
			var sweepWinner = default(SweepWinner);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_winner_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepWinnerUid), sweepWinnerUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepWinner = CreateSweepWinnerFrom(reader);
					}
				}
			}

			return sweepWinner;
		}

		private SweepWinner CreateSweepWinnerFrom(MySqlDataReader reader)
		{
			return new SweepWinner
			{
				SweepWinnerId = reader.GetInt32("sweep_winner_id"),
				SweepWinnerUid = reader.GetGuid("sweep_winner_uid"),
				SweepEntry = new SweepEntry
				{
					SweepEntryUid = reader.GetGuid("sweep_entry_uid")
				},
				Sweep = new Sweep
				{
					SweepId = reader.GetInt32("sweep_id")
				},				
				AssignedId = reader.GetInt32("assigned_id"),
				FulfilledInd = reader.GetBoolean("fulfilled_ind"),
				UserIdentifier = reader.GetString("user_identifier"),
				UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), reader.GetString("user_identifier_type")),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		private DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime(fieldName);
		}

		public async Task SaveSweepBackupWinnerAsync(SweepWinner sweepWinner)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_winner_backup_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("sweepId", sweepWinner.Sweep?.SweepId)
				.AddParameter("sweepWinnerUid", sweepWinner.SweepWinnerUid)
				.AddParameter("sweepEntryUid", sweepWinner.SweepEntry?.SweepEntryUid)
				.AddParameter("assignedId", sweepWinner.AssignedId)
				.AddParameter("userIdentifier", sweepWinner.UserIdentifier)
				.AddParameter("userIdentifierType", sweepWinner.UserIdentifierType.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<SweepBackupWinner>> GetSweepBackupWinnersAsync(Guid sweepWinnerUid)
		{
			var backupWinners = new List<SweepBackupWinner>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_winner_backups_get_by_sweep_winner_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepWinnerUid), sweepWinnerUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						backupWinners.Add(CreateBackupSweepWinnerFrom(reader));
					}
				}
			}

			return backupWinners;
		}

		private SweepBackupWinner CreateBackupSweepWinnerFrom(MySqlDataReader reader)
		{
			return new SweepBackupWinner
			{
				SweepWinnerBackupId = reader.GetInt32("sweep_winner_backup_id"),
				SweepWinnerId = reader.GetInt32("sweep_winner_id"),
				SweepEntry = new SweepEntry
				{
					SweepEntryUid = reader.GetGuid("sweep_entry_uid")
				},
				Sweep = new Sweep
				{
					SweepId = reader.GetInt32("sweep_id")
				},
				UserIdentifier = reader.GetString("user_identifier"),
				UserIdentifierType = (UserIdentifierType)Enum.Parse(typeof(UserIdentifierType), reader.GetString("user_identifier_type")),
				Selected = reader.GetBoolean("selected_ind"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		public async Task<IEnumerable<SweepWinner>> GetSweepWinnersAsync(Guid sweepUid)
		{
			var sweepWinners = new List<SweepWinner>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "sweep_winners_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(sweepUid), sweepUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						sweepWinners.Add(CreateSweepWinnerFrom(reader));
					}
				}
			}

			return sweepWinners;
		}
	}
}

using hrvster.Infrastructure.Extensions;
using hrvster.Models.SpinToWin;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.SpinToWinRepository
{
	public interface ISpinToWinRepository
	{
		Task CreateSpinToWinGameAsync(SpinToWinGame spinToWinGame, CancellationToken cancellationToken);
		IEnumerable<SpinToWinGame> GetAllForAccount(Guid userUid);
		Task<IEnumerable<SpinToWinGame>> GetAllForAccountAsync(Guid userUid);
	}

	public class SpinToWinRepository : ISpinToWinRepository
	{
		private readonly IDatabase _database;

		public SpinToWinRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<SpinToWinGame>> GetAllForAccountAsync(Guid userUid)
		{
			var spinToWinGames = new List<SpinToWinGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "spin_to_win_game_get_all_for_account";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", userUid);

				await conn.OpenAsync(default);
				using var reader = await command.ExecuteReaderAsync(default);
				while (await reader.ReadAsync(default))
				{
					spinToWinGames.Add(CreateFrom(reader));
				}
			}

			return spinToWinGames;
		}

		public IEnumerable<SpinToWinGame> GetAllForAccount(Guid userUid)
		{
			var spinToWinGames = new List<SpinToWinGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "spin_to_win_game_get_all_for_account";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", userUid);

				conn.Open();
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					spinToWinGames.Add(CreateFrom(reader));
				}
			}

			return spinToWinGames;
		}

		private SpinToWinGame CreateFrom(MySqlDataReader reader)
		{
			return new SpinToWinGame { };
		}

		public async Task CreateSpinToWinGameAsync(SpinToWinGame spinToWinGame, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "spin_to_win_create";
			command.CommandType = CommandType.StoredProcedure;
			//command.AddParameter();

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}
	}
}

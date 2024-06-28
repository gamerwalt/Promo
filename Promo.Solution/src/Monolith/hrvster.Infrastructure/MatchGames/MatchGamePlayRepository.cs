using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchGamePlayRepository
	{
		Task CreateAsync(Guid gameplayUid, int id, string screenName, Models.UserIdentifierType handle, CancellationToken cancellationToken);
		Task FulfillPrizeAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchPlay>> GetForGamePlayAsync(Guid gameUid, CancellationToken cancellationToken);
		Task<int> GetTotalGamePlaysAsync(Guid gameUid, CancellationToken cancellationToken);
		Task MarkAsPlayedAndLossAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
		Task MarkAsPlayedAndWonAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken);
	}

	public class MatchGamePlayRepository : IMatchGamePlayRepository
	{
		private readonly IDatabase _database;

		public MatchGamePlayRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task FulfillPrizeAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_play_fulfill_prize";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("gameplayUid", gameplayUid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<int> GetTotalGamePlaysAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var totalPlays = 0;

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_gameplays_get_total";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameUid", gameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				if (await reader.ReadAsync(cancellationToken))
				{
					totalPlays = reader.GetInt32("total_plays");
				}
			}

			return totalPlays;
		}

		public async Task CreateAsync(Guid gameplayUid, int id, string screenName, Models.UserIdentifierType handleType, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_play_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("gameplayUid", gameplayUid)
				.AddParameter("gameId", id)
				.AddParameter("screenName", screenName)
				.AddParameter("userIdentifierType", handleType);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<MatchPlay> GetAsync(Guid gameplayUid, CancellationToken cancellationToken)
		{
			var matchPlay = default(MatchPlay);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_gameplay_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameplayUid", gameplayUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchPlay = CreateFrom(reader);
				}
			}

			return matchPlay;
		}

		public async Task<IEnumerable<MatchPlay>> GetForGamePlayAsync(Guid gameUid, CancellationToken cancellationToken)
		{
			var matchPlay = new List<MatchPlay>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_gameplay_get_by_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameUid", gameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchPlay.Add(CreateFrom(reader));
				}
			}

			return matchPlay;
		}

		public async Task MarkAsPlayedAndLossAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_play_mark_as_played_and_loss";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("gameplayUid", gameplayUid)
				.AddParameter("selectedTiles", selectedTiles);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task MarkAsPlayedAndWonAsync(Guid gameplayUid, string selectedTiles, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_play_mark_as_played_and_won";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("gameplayUid", gameplayUid)
				.AddParameter("selectedTiles", selectedTiles);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		private MatchPlay CreateFrom(MySqlDataReader reader)
		{
			var gameplay = new MatchPlay
			{
				Id = reader.GetInt32("game_play_id"),
				Uid = reader.GetGuid("game_play_uid"),
				PlayedIndicator = reader.GetBoolean("played_ind"),
				WinIndicator = reader.GetBoolean("win_ind"),
				SelectedTiles = reader.GetStringValue("selected_tiles"),
				MatchBoard = new MatchBoard
				{
					Id = reader.GetInt32("board_id"),
					Uid = reader.GetGuid("board_uid"),
					IsWinnable = reader.GetBoolean("is_winnable"),
					PlayedIndicator = reader.GetBoolean("board_played_ind"),
					IsReserved = reader.GetBoolean("is_reserved")
				},
				PrizeName = reader.GetStringValue("prize_name"),
				Fulfilled = reader.GetBoolean("fulfilled_ind"),
				DateFulfilled = reader.GetNullableDateTimeValue("date_fulfilled"),
				ScreenName = reader.GetString("screen_name"),
				UserIdentifierType = Enum.Parse<UserIdentifierType>(reader.GetString("user_identifier_type"))
			};

			if(gameplay.WinIndicator)
			{
				gameplay.PrizeUid = reader.GetGuid("prize_uid");
				gameplay.PrizeId = reader.GetInt32("prize_id");
			}

			return gameplay;
		}
	}
}

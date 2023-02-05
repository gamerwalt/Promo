using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchTilesRepository
	{
		Task CreateAsync(MatchTile tile, CancellationToken cancellationToken);
		Task<IEnumerable<MatchTile>> GetForBoardAsync(Guid uid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchTile>> GetForMatchGameAsync(Guid matchGameUid, CancellationToken cancellationToken);
	}

	public class MatchTilesRepository : IMatchTilesRepository
	{
		private readonly IDatabase _database;

		public MatchTilesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(MatchTile tile, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_tiles_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("tileUid", tile.Uid)
				.AddParameter("boardUid", tile.Board.Uid)
				.AddParameter("imageUid", tile.Image.Uid)
				.AddParameter("winIndicator", tile.WinIndicator);

			if (tile.WinIndicator)
				command.AddParameter("prizeUid", tile.Prize.Uid);
			else
				command.AddParameter("prizeUid", Guid.Empty);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<IEnumerable<MatchTile>> GetForBoardAsync(Guid boardUid, CancellationToken cancellationToken)
		{
			var matchTiles = new List<MatchTile>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_tiles_get_by_board_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("boardUid", boardUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchTiles.Add(CreateFrom(reader));
				}
			}

			return matchTiles;
		}

		public async Task<IEnumerable<MatchTile>> GetForMatchGameAsync(
			Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchTiles = new List<MatchTile>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_tiles_get_by_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchTiles.Add(CreateFrom(reader));
				}
			}

			return matchTiles;
		}

		private MatchTile CreateFrom(MySqlDataReader reader)
		{
			var winIndicator = reader.GetBoolean("win_indicator");
			return new MatchTile
			{
				Id = reader.GetInt32("tile_id"),
				Uid = reader.GetGuid("tile_uid"),
				WinIndicator = reader.GetBoolean("win_indicator"),
				Board = new MatchBoard
				{
					Uid = reader.GetGuid("board_uid"),
					Id = reader.GetInt32("board_id")
				},
				Prize = winIndicator ? new MatchPrize
				{
					Uid = reader.GetGuid("prize_uid"),
					Id = reader.GetInt32("prize_id")
				} : null,
				Image = new MatchImage
				{
					Uid = reader.GetGuid("image_uid"),
					Id = reader.GetInt32("image_id")
				}
			};
		}
	}
}

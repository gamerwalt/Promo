using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchGamesRepository
	{
		Task<IEnumerable<MatchGame>> GetByAccountAsync(Guid accountUid, CancellationToken cancellationToken);
	}

	public class MatchGamesRepository : IMatchGamesRepository
	{
		private readonly IDatabase _database;

		public MatchGamesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<MatchGame>> GetByAccountAsync(Guid accountUid, CancellationToken cancellationToken)
		{
			var matchGames = new List<MatchGame>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_games_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchGames.Add(CreateFrom(reader));
				}
			}

			return matchGames;
		}

		private MatchGame CreateFrom(MySqlDataReader reader)
		{
			return new MatchGame
			{
				Id = reader.GetInt32("match_game_id"),
				Uid = reader.GetGuid("match_game_uid"),
				CampaignId = reader.GetInt32("campaign_id"),
				CampaignUid = reader.GetGuid("campaign_uid"),
				Name = reader.GetString("match_game_name"),
				Description= reader.GetString("match_game_description"),
				NumberOfBoards = reader.GetInt32("number_of_boards"),
				TileCountPerBoard = reader.GetInt32("tile_count_per_board"),
				NumberOfWinnableBoards = reader.GetInt32("number_of_winnable_boards"),
				WinProbability = reader.GetInt32("win_probability_per_board"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc")
			};
		}
	}
}

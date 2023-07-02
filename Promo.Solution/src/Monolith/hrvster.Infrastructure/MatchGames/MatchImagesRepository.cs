using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace hrvster.Infrastructure.MatchGames
{
	public interface IMatchImagesRepository
	{
		Task CreateAsync(MatchImage image, CancellationToken cancellationToken);
		Task DeleteAsync(Guid uid, CancellationToken cancellationToken);
		Task<MatchImage> GetAsync(Guid uid, CancellationToken cancellationToken);
		Task<IEnumerable<MatchImage>> GetForMatchGamesAsync(Guid matchGameUid, CancellationToken cancellationToken);
		Task UpdateAsync(Guid matchImageUid, MatchImage matchImage, CancellationToken cancellationToken);
		Task UpdateImageAsLossImageAsync(MatchImage image, CancellationToken cancellationToken);
	}

	public class MatchImagesRepository : IMatchImagesRepository
	{
		private readonly IDatabase _database;

		public MatchImagesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(MatchImage image, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_image_create";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("imageUid", image.Uid)
				.AddParameter("matchGameUid", image.MatchGame.Uid)
				.AddParameter("imageName", image.Name)
				.AddParameter("locationUrl", image.LocationUrl);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task UpdateImageAsLossImageAsync(MatchImage image, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_image_set_as_loss_image";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("imageUid", image.Uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task DeleteAsync(Guid uid, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_image_delete";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("imageUid", uid);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<MatchImage> GetAsync(Guid matchImageUid, CancellationToken cancellationToken)
		{
			var matchImage = new MatchImage();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_image_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchImageUid", matchImageUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchImage = CreateFrom(reader);
				}
			}

			return matchImage;
		}

		public async Task<IEnumerable<MatchImage>> GetForMatchGamesAsync(Guid matchGameUid, CancellationToken cancellationToken)
		{
			var matchImages = new List<MatchImage>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "match_images_get_by_match_game_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("matchGameUid", matchGameUid);

				await conn.OpenAsync(cancellationToken);
				using var reader = await command.ExecuteReaderAsync(cancellationToken);
				while (await reader.ReadAsync(cancellationToken))
				{
					matchImages.Add(CreateFrom(reader));
				}
			}

			return matchImages;
		}

		public async Task UpdateAsync(Guid matchImageUid, MatchImage matchImage, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "match_image_update";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("imageUid", matchImageUid)
				.AddParameter("locationUrl", matchImage.LocationUrl);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		private MatchImage CreateFrom(MySqlDataReader reader)
		{
			return new MatchImage
			{
				Id = reader.GetInt32("image_id"),
				Uid = reader.GetGuid("image_uid"),
				LocationUrl = reader.GetString("location_url"),
				Name = reader.GetString("image_name"),
				IsLoss = reader.GetBoolean("is_loss"),
				MatchGame = new MatchGame
				{
					Uid = reader.GetGuid("match_game_uid"),
					Id = reader.GetInt32("match_game_id")
				}
			};
		}
	}
}

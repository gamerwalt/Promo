using hrvster.Infrastructure.Extensions;
using hrvster.Models.Harvester;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Harvester
{
	public interface IIncomingTweetsRepository
	{
		Task CreateAsync(Guid incomingTweetUid, string jsonTweet, IncomingTweetStatus status);
		Task UpdateStatusAsync(Guid incomingTweetUid, IncomingTweetStatus status);
		Task UpdateAsProcessedAsync(Guid incomingTweetUid, IncomingTweetStatus status);
		Task<IEnumerable<IncomingTweet>> GetByStatusAsync(IncomingTweetStatus status);
		Task<IncomingTweet> GetByUidAsync(Guid incomingTweetUid);
	}

	public class IncomingTweetsRepository : IIncomingTweetsRepository
	{
		private readonly IDatabase _database;

		public IncomingTweetsRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Guid incomingTweetUid, string jsonTweet, IncomingTweetStatus status)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "incoming_tweets_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("incomingTweetUid", incomingTweetUid)
				.AddParameter("tweet", jsonTweet)
				.AddParameter("incomingStatus", status.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<IncomingTweet>> GetByStatusAsync(IncomingTweetStatus status)
		{
			var tweets = new List<IncomingTweet>();

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "incoming_tweets_get_by_status";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("incomingStatus", status);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					tweets.Add(CreateRecordFrom(reader));
				}
			}

			return tweets;
		}

		public async Task<IncomingTweet> GetByUidAsync(Guid incomingTweetUid)
		{
			var incomingTweet = default(IncomingTweet);

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "incoming_tweets_get_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("incomingTweetUid", incomingTweetUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					incomingTweet = CreateRecordFrom(reader);
				}
			}

			return incomingTweet;
		}

		public async Task UpdateAsProcessedAsync(Guid incomingTweetUid, IncomingTweetStatus status)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "incoming_tweets_update_as_processed";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("incomingTweetUid", incomingTweetUid)
				.AddParameter("incomingStatus", status.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateStatusAsync(Guid incomingTweetUid, IncomingTweetStatus status)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "incoming_tweets_update_status";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("incomingTweetUid", incomingTweetUid)
				.AddParameter("incomingStatus", status.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private IncomingTweet CreateRecordFrom(MySqlDataReader reader)
		{
			return new IncomingTweet
			{
				IncomingTweetId = reader.GetInt32("incoming_tweet_id"),
				IncomingTweetUid = reader.GetGuid("incoming_tweet_uid"),
				Tweet = reader.GetString("tweet"),
				Status = Enum.Parse<IncomingTweetStatus>(reader.GetString("incoming_status")),
				ProcessedIndicator = reader.GetBoolean("processed_ind"),
				CreatedDate = reader.GetNullableDateTimeValue("created_date").GetValueOrDefault(),
				UpdatedDate = reader.GetNullableDateTimeValue("updated_date").GetValueOrDefault()
			};
		}
	}
}

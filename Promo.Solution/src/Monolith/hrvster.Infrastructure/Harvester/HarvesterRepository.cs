using hrvster.Models.Harvester;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;
using Tweetinvi.Models;
using hrvster.Infrastructure.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace hrvster.Infrastructure.Harvester
{
	public interface IHarvesterRepository
	{
		Task CreateHarvestedRecordAsync(Guid harvestedUid, Guid ruleUid, string json);
		Task CreateTweetUserAsync(Guid userUid, IUser user);
		Task CreateTweetRecordAsync(Guid tweetUid, Guid harvestedUid, Guid ruleUid, Guid userUid, ITweet tweet);
		Task<HarvestedUser> GetHarvestedUserByUserIdentifierAsync(string userIdentifier);
		Task BeginTransaction();
		Task CommitTransaction();
		Task RollbackTransaction();
		Task<IEnumerable<Harvested>> GetTweetsAsync(Guid ruleUid);
	}

	public class HarvesterRepository : IHarvesterRepository
	{
		private readonly IDatabase _database;
		private MySqlConnection myConnection;
		private MySqlTransaction myTransaction;

		public HarvesterRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task BeginTransaction()
		{
			myConnection = new MySqlConnection(_database.GetHarvesterConnectionString());
			await myConnection.OpenAsync();

			myTransaction = await myConnection.BeginTransactionAsync();
		}

		public async Task CommitTransaction()
		{
			await myTransaction.CommitAsync();
			await myConnection.CloseAsync();
		}

		public async Task RollbackTransaction()
		{
			if (myTransaction != null)
			{
				await myTransaction.RollbackAsync();
				await myConnection.CloseAsync();
			}
		}

		public async Task CreateHarvestedRecordAsync(Guid harvestedUid, Guid ruleUid, string json)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "harvested_record_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter(nameof(harvestedUid), harvestedUid)
				.AddParameter(nameof(ruleUid), ruleUid)
				.AddParameter("harvestedValue", json);

			await command.ExecuteNonQueryAsync();
		}

		public async Task CreateTweetRecordAsync(Guid tweetUid, Guid harvestedUid, Guid ruleUid, Guid userUid, ITweet tweet)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "tweet_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter(nameof(tweetUid), tweetUid)
				.AddParameter(nameof(harvestedUid), harvestedUid)
				.AddParameter(nameof(userUid), userUid)
				.AddParameter("externalId", tweet.IdStr)
				.AddParameter("body", tweet.FullText)
				.AddParameter("retweeted", tweet.Retweeted)
				.AddParameter("externalCreatedAt", tweet.CreatedAt);

			await command.ExecuteNonQueryAsync();
		}

		public async Task CreateTweetUserAsync(Guid userUid, IUser user)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "tweet_user_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("userUid", userUid)
				.AddParameter("externalId", user.IdStr)
				.AddParameter("profileImageUrl", user.ProfileImageUrlFullSize)
				.AddParameter("verifiedInd", user.Verified)
				.AddParameter("location", user.Location)
				.AddParameter("description", user.Description)
				.AddParameter("screenName", user.ScreenName);

			await command.ExecuteNonQueryAsync();
		}

		public async Task<HarvestedUser> GetHarvestedUserByUserIdentifierAsync(string userIdentifier)
		{
			var harvestedUser = default(HarvestedUser);

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "users_get_by_screen_name";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(userIdentifier), userIdentifier);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						harvestedUser = CreateHarvestedUserFrom(reader);
					}
				}
			}

			return harvestedUser;
		}

		private HarvestedUser CreateHarvestedUserFrom(MySqlDataReader reader)
		{
			//Todo: Add remaining fields
			return new HarvestedUser
			{
				UserId = reader.GetInt32("user_id"),
				UserUid = reader.GetGuid("user_uid"),
				ExternalId = reader.GetString("external_id")
			};
		}

		public async Task<IEnumerable<Harvested>> GetTweetsAsync(Guid ruleUid)
		{
			var harvested = new List<Harvested>();

			using (var conn = new MySqlConnection(_database.GetHarvesterConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "tweets_get_harvested";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(ruleUid), ruleUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					harvested.Add(CreateHarvestedFrom(reader));
				}
			}

			return harvested;
		}

		private Harvested CreateHarvestedFrom(MySqlDataReader reader)
		{
			return new Harvested
			{
				HarvestedId = reader.GetInt32("harvested_id"),
				RuleUid = reader.GetGuid("rule_uid"),
				HarvestedUid = reader.GetGuid("harvested_uid"),
				ExternalTweetId = reader.GetStringValue("tweet_external_id"),
				TweetUid = reader.GetGuid("tweet_uid"),
				TweetText = reader.GetStringValue("tweet_text"),
				UserUid = reader.GetGuid("user_uid"),
				ExternalUserId = reader.GetStringValue("user_external_id"),
				ProfileImageUrl = reader.GetStringValue("profile_image_url"),
				Location = reader.GetStringValue("location"),
				Description = reader.GetStringValue("description"),
				ExternalScreenName = reader.GetStringValue("screen_name")
			};
		}
	}
}

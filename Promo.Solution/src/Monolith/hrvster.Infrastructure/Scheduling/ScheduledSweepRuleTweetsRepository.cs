using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Profiles;
using hrvster.Models.Scheduling;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Scheduling
{
	public interface IScheduledSweepRuleTweetsRepository
	{
		Task<List<ScheduledSweepRuleTweet>> GetAllToSendAsync(DateTime utcNow);
		Task<List<ScheduledSweepRuleTweetImage>> GetAllImagesAsync(int scheduledId);
		Task UpdateStatus(ScheduledSweepRuleTweet scheduledTweet, ScheduledStatus status);
	}

	public class ScheduledSweepRuleTweetsRepository : IScheduledSweepRuleTweetsRepository
	{
		private readonly IDatabase _database;

		public ScheduledSweepRuleTweetsRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<List<ScheduledSweepRuleTweetImage>> GetAllImagesAsync(int scheduledId)
		{
			var images = new List<ScheduledSweepRuleTweetImage>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "scheduled_sweep_rule_tweet_images_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(scheduledId), scheduledId);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					images.Add(CreateImageRecordFrom(reader));
				}
			}

			return images;
		}

		private ScheduledSweepRuleTweetImage CreateImageRecordFrom(MySqlDataReader reader)
		{
			return new ScheduledSweepRuleTweetImage
			{
				ImageLocation = reader.GetString("image_location")
			};
		}

		public async Task<List<ScheduledSweepRuleTweet>> GetAllToSendAsync(DateTime utcDate)
		{
			var scheduledSweepRuleTweets = new List<ScheduledSweepRuleTweet>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "scheduled_sweep_rule_tweets_get_not_sent";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(utcDate), utcDate);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					scheduledSweepRuleTweets.Add(CreateRecordFrom(reader));
				}
			}

			return scheduledSweepRuleTweets;
		}

		private ScheduledSweepRuleTweet CreateRecordFrom(MySqlDataReader reader)
		{
			return new ScheduledSweepRuleTweet()
			{
				ScheduledId = reader.GetInt32("scheduled_id"),
				ScheduledUid = reader.GetGuid("scheduled_uid"),
				Rule = new Models.Rule
				{
					RuleId = reader.GetInt32("rule_id"),
					RuleUid = reader.GetGuid("rule_uid")
				},
				Text = reader.GetString("tweet_text"),
				ScheduledTime = reader.GetDateTime("scheduled_time"),
				ScheduledTimeUtc = reader.GetDateTime("scheduled_time_utc"),
				Status = (ScheduledStatus)Enum.Parse(typeof(ScheduledStatus), reader.GetString("status")),
				CreatedDate = reader.GetDateTime("created_date"),
				Profile = new Profile
				{
					ProfileUid = reader.GetGuid("profile_uid"),
					AccessToken = reader.GetString("access_token"),
					AccessTokenSecret = reader.GetString("access_token_secret")
				}
			};
		}

		public async Task UpdateStatus(ScheduledSweepRuleTweet scheduledTweet, ScheduledStatus status)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "scheduled_sweep_rule_tweet_update_status";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("scheduledUid", scheduledTweet.ScheduledUid)
				.AddParameter("newStatus", status);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}

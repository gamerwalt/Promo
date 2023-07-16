using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Scheduling;
using hrvster.Models.WebModels.Sweeps;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Tweetinvi.Core.Models.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace hrvster.Infrastructure.Scheduling
{
	public interface IScheduledPostRepository
	{
		Task BeginTransactionAsync();
		Task RollbackTransactionAsync();
		Task CommitTransactionAsync();
		Task CreateScheduledPostAsync(
			Guid scheduledPostUid, 
			Guid profileUid, 
			int ruleId, 
			string gameType, 
			string post, 
			DateTime scheduledDate, 
			string urlLink, 
			ScheduledStatus scheduledStatus,
			string timezoneId,
			bool isDaylight);
		Task CreateScheduledPostImageAsync(
			Guid scheduledPostUid, 
			Guid scheduledPostImageUid, 
			string imageLocation);
		Task<ScheduledPost> GetScheduledPostAsync(int ruleId);
		Task<IEnumerable<ScheduledPostImage>> GetAllImagesAsync(int postId);
		Task UpdateScheduledPostAsync(Guid scheduledUid, DateTime scheduledTime, string text, string timeZoneId, bool isDaylight);
		Task UpdateScheduledPostImageAsync(int imageId, string location);
		Task<IEnumerable<ScheduledPost>> GetAllToSendAsync();
		Task UpdateStatusAsync(ScheduledPost scheduledPost, ScheduledStatus status);
		Task DeleteScheduledPostAsync(Guid scheduledUid);
		Task<ScheduledPost> GetAsync(Guid scheduledUid);
		Task DeleteSweepRuleByIdAsync(int ruleId);
		Task DeleteMatchGameRuleByIdAsync(int ruleId);
	}

	public class ScheduledPostRepository : IScheduledPostRepository
	{
		private readonly IDatabase _database;
		private readonly ILogger _logger;

		private MySqlConnection myConnection;
		private MySqlTransaction myTransaction;

		public ScheduledPostRepository(IDatabase database, ILoggerFactory loggerFactory)
		{
			_database = database;
			_logger = loggerFactory.CreateLogger<ScheduledPostRepository>();
		}

		public async Task BeginTransactionAsync()
		{
			myConnection = new MySqlConnection(_database.GetConnectionString());
			await myConnection.OpenAsync();

			myTransaction = await myConnection.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			await myTransaction.CommitAsync();
		}

		public async Task CreateScheduledPostAsync(
			Guid scheduledPostUid, 
			Guid profileUid, 
			int ruleId, 
			string gameType, 
			string post,
			DateTime scheduledDate,
			string urlLink, 
			ScheduledStatus scheduledStatus,
			string timezoneId,
			bool isDaylight)
		{
			try
			{
				var scheduledTimeUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(scheduledDate, TimeZoneInfo.Local.Id, timezoneId);

				if (!isDaylight)
				{
					scheduledTimeUtc = scheduledTimeUtc.AddHours(-2);
				}

				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "scheduled_post_create";
				command.CommandType = CommandType.StoredProcedure;
				command
					.AddParameter("profileUid", profileUid)
					.AddParameter("ruleId", ruleId)
					.AddParameter("scheduledUid", scheduledPostUid)
					.AddParameter("gameType", gameType)
					.AddParameter("tweetText", post)
					.AddParameter("urlLink", urlLink)
					.AddParameter("scheduledTime", scheduledDate)
					.AddParameter("scheduledTimeUtc", scheduledTimeUtc)
					.AddParameter("scheduledStatus", scheduledStatus);

				//await myConnection.OpenAsync();
				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to save the scheduled post. An error occurred. Message: {message}. Inner Exception {inner}", 
					ex.Message,
					ex.InnerException?.Message);
			}
		}

		public async Task CreateScheduledPostImageAsync(
			Guid scheduledPostUid,
			Guid scheduledPostImageUid,
			string imageLocation)
		{
			try
			{
				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "scheduled_post_image_create";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("scheduledPostUid", scheduledPostUid)
					.AddParameter("scheduledPostImageUid", scheduledPostImageUid)
					.AddParameter("imageLocation", imageLocation);


				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to save the scheduled post. An error occurred. Message: {message}", ex.Message);
			}
		}

		public async Task DeleteSweepRuleByIdAsync(int ruleId)
		{
			try
			{
				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "sweep_rule_delete_by_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("ruleId", ruleId);

				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to delete the sweep rule. An error occurred. Message: {message}", ex.Message);
			}
		}

		public async Task DeleteMatchGameRuleByIdAsync(int ruleId)
		{
			try
			{
				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "match_game_rule_delete_by_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("ruleId", ruleId);

				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to delete the match rule. An error occurred. Message: {message}", ex.Message);
			}
		}

		public async Task DeleteScheduledPostAsync(Guid scheduledUid)
		{
			try
			{
				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "scheduled_post_image_delete";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("scheduledUid", scheduledUid);

				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to delete the scheduled post. An error occurred. Message: {message}", ex.Message);
			}
		}

		public async Task UpdateScheduledPostAsync(
			Guid scheduledUid, 
			DateTime scheduledTime,
			string text,
			string timeZoneId, 
			bool isDaylight)
		{
			try
			{
				var scheduledTimeUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(scheduledTime, TimeZoneInfo.Local.Id, timeZoneId);

				if (!isDaylight)
				{
					scheduledTimeUtc = scheduledTimeUtc.AddHours(-2);
				}

				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "scheduled_post_update";
				command.CommandType = CommandType.StoredProcedure;
				command
					.AddParameter("scheduledUid", scheduledUid)
					.AddParameter("tweetText", text)
					.AddParameter("scheduledTime", scheduledTime)
					.AddParameter("scheduledTimeUtc", scheduledTimeUtc);

				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while tryiing to update a scheduled post. Exception Message: {message}", ex.Message);
			}
		}

		public async Task UpdateScheduledPostImageAsync(int imageId, string location)
		{
			try
			{
				var command = myConnection.CreateCommand();
				command.Transaction = myTransaction;
				command.CommandText = "scheduled_post_image_update";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("imageId", imageId)
					.AddParameter("imageLocation", location);

				await command.ExecuteNonQueryAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while tryiing to update a scheduled post image. Exception Message: {message}", ex.Message);
			}
		}

		public async Task UpdateStatusAsync(ScheduledPost scheduledPost, ScheduledStatus status)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "scheduled_post_update_status";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("scheduledPostUid", scheduledPost.Uid)
				.AddParameter("newStatus", status);

			await conn.OpenAsync();
			await command.ExecuteReaderAsync();
		}

		public async Task<IEnumerable<ScheduledPostImage>> GetAllImagesAsync(int postId)
		{
			var scheduledPostImage = new List<ScheduledPostImage>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "scheduled_post_images_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("scheduledPostId", postId);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					scheduledPostImage.Add(CreateScheduledPostImageFrom(reader));
				}
			}

			return scheduledPostImage;
		}

		private ScheduledPostImage CreateScheduledPostImageFrom(MySqlDataReader reader)
			=> new ScheduledPostImage
			{
				Id = reader.GetInt32("id"),
				Uid = reader.GetGuid("uid"),
				ImageLocation = reader.GetStringValue("image_location")
			};

		public async Task<ScheduledPost> GetScheduledPostAsync(int ruleId)
		{
			var scheduledPost = default(ScheduledPost);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "scheduled_post_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(ruleId), ruleId);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					scheduledPost = CreateRecordFrom(reader);
					if(scheduledPost != null)
					{
						scheduledPost.Rule = new Models.Rule
						{
							RuleId = reader.GetInt32("rule_id")
						};
					}
				}
			}

			return scheduledPost;
		}

		public async Task<ScheduledPost> GetAsync(Guid scheduledUid)
		{
			var scheduledPost = default(ScheduledPost);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "scheduled_post_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(scheduledUid), scheduledUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					scheduledPost = CreateRecordFrom(reader);
					if (scheduledPost != null)
					{
						scheduledPost.Rule = new Models.Rule
						{
							RuleId = reader.GetInt32("rule_id")
						};
					}
				}
			}

			return scheduledPost;
		}

		public async Task<IEnumerable<ScheduledPost>> GetAllToSendAsync()
		{
			var scheduledPosts = new List<ScheduledPost>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "scheduled_posts_get_not_sent";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while(await reader.ReadAsync())
				{
					var scheduledPost = CreateRecordFrom(reader);
					if(scheduledPost != null)
					{
						scheduledPost.Rule = new Models.Rule
						{
							RuleId = reader.GetInt32("rule_id")
						};
					}
					scheduledPosts.Add(scheduledPost);
				}
			}

			return scheduledPosts;
		}


		private ScheduledPost CreateRecordFrom(MySqlDataReader reader)
			=> new ScheduledPost
				{
					Id = reader.GetInt32("id"),
					Uid = reader.GetGuid("uid"),
					Profile = new Models.Profiles.Profile
					{
						ProfileUid = reader.GetGuid("profile_uid"),
						ProfileId = reader.GetInt32("profile_id")
					},
					GameType = Enum.Parse<GameTypesEnum>(reader.GetString("game_type")),
					Post = reader.GetStringValue("tweet_text"),
					ScheduledTime = reader.GetDateTime("scheduled_time"),
					ScheduledTimeUtc = reader.GetDateTime("scheduled_time_utc"),
					UrlLink = reader.GetStringValue("url_link"),
					Status = Enum.Parse<ScheduledStatus>(reader.GetString("status")),
				};

		public async Task RollbackTransactionAsync()
		{
			if (myTransaction != null)
			{
				await myTransaction.RollbackAsync();
			}
		}
	}
}

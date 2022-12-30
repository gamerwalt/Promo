using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Scheduling;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Scheduling
{
	public interface ISweepTweetSchedulerRepository
	{
		Task BeginTransaction();
		Task CommitTransaction();
		Task RollbackTransaction();
		Task CreateRuleAsync(Models.Rule rule);
		Task CreateScheduledSweepRuleTweetAsync(ScheduledSweepRuleTweet scheduledSweepRuleTweet, string timezoneId, bool isDaylight);
		Task CreateScheduledSweepRuleImage(Guid scheduledUid, ScheduledSweepRuleTweetImage image);
		Task UpdateScheduledTweetAsync(Guid scheduledUid, DateTime scheduledTime, string text, string timezoneId, bool isDaylight);
		Task UpdateScheduledTweetImageAsync(int scheduledImageId, string location);
		Task DeleteScheduledSweepRuleTweetAsync(Guid scheduledUid);
		Task DeleteRuleAsync(Guid ruleUid);
	}

	public class SweepTweetSchedulerRepository : ISweepTweetSchedulerRepository
	{
		private readonly IDatabase _database;

		private MySqlConnection myConnection;
		private MySqlTransaction myTransaction;

		public SweepTweetSchedulerRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task BeginTransaction()
		{
			myConnection = new MySqlConnection(_database.GetConnectionString());
			await myConnection.OpenAsync();

			myTransaction = await myConnection.BeginTransactionAsync();
		}

		public async Task CommitTransaction()
		{
			await myTransaction.CommitAsync();
		}

		public async Task RollbackTransaction()
		{
			if (myTransaction != null)
			{
				await myTransaction.RollbackAsync();
			}
		}

		public async Task CreateScheduledSweepRuleTweetAsync(ScheduledSweepRuleTweet model, string timezoneId, bool isDaylight)
		{
			var scheduledTimeUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(model.ScheduledTime, TimeZoneInfo.Local.Id, timezoneId);

			if (!isDaylight)
			{
				scheduledTimeUtc = scheduledTimeUtc.AddHours(-2);
			}

			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "scheduled_sweep_rule_tweet_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("profileUid", model.Profile.ProfileUid)
				.AddParameter("ruleUid", model.Rule.RuleUid)
				.AddParameter("scheduledUid", model.ScheduledUid)
				.AddParameter("tweetText", model.Text)
				.AddParameter("scheduledTime", model.ScheduledTime)
				.AddParameter("scheduledTimeUtc", scheduledTimeUtc)
				.AddParameter("status", model.Status.ToString());

			//await myConnection.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task CreateRuleAsync(Models.Rule rule)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "sweep_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("ruleUid", rule.RuleUid)
				.AddParameter("sweepUid", rule.Sweep.SweepUid)
				.AddParameter("watchTypeCode", rule.WatchTypeCode.ToString())
				.AddParameter("watchValue", rule.WatchValue)
				.AddParameter("externalId", rule.ExternalId)
				.AddParameter("activeInd", rule.ActiveInd)
				.AddParameter("scheduledInd", rule.ScheduledInd)
				.AddParameter("mustBeFollowing", rule.MustBeFollowing);

			//await myConnection.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task CreateScheduledSweepRuleImage(Guid scheduledUid, ScheduledSweepRuleTweetImage image)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "scheduled_sweep_rule_image_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("scheduledUid", scheduledUid)
				.AddParameter("imageLocation", image.ImageLocation);

			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateScheduledTweetAsync(Guid scheduledUid, DateTime scheduledTime, string text, string timezoneId, bool isDaylight)
		{
			var scheduledTimeUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(scheduledTime, TimeZoneInfo.Local.Id, timezoneId);

			if (isDaylight)
			{
				scheduledTimeUtc = scheduledTimeUtc.AddHours(-2);
			}

			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "scheduled_sweep_rule_tweet_update";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("scheduledUid", scheduledUid)
				.AddParameter("scheduledTime", scheduledTime)
				.AddParameter("scheduledTimeUtc", scheduledTimeUtc)
				.AddParameter("tweetText", text);

			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateScheduledTweetImageAsync(int scheduledImageId, string location)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "scheduled_sweep_rule_tweet_image_update";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("scheduledImageId", scheduledImageId)
				.AddParameter("imageLocation", location);

			await command.ExecuteNonQueryAsync();
		}

		public async Task DeleteScheduledSweepRuleTweetAsync(Guid scheduledUid)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "scheduled_sweep_rule_tweet_delete";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("scheduledUid", scheduledUid);

			await command.ExecuteNonQueryAsync();
		}

		public async Task DeleteRuleAsync(Guid ruleUid)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "sweep_rule_delete";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("ruleUid", ruleUid);

			await command.ExecuteNonQueryAsync();
		}
	}
}

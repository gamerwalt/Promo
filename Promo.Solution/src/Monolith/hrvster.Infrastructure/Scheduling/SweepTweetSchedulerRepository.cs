using hrvster.Models;
using hrvster.Models.Scheduling;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hrvster.Extensions;

namespace hrvster.Infrastructure.Scheduling
{
	public interface ISweepTweetSchedulerRepository
	{
		Task BeginTransaction();
		Task CommitTransaction();
		Task RollbackTransaction();
		Task CreateRuleAsync(Models.Rule rule);
		Task CreateScheduledSweepRuleTweetAsync(ScheduledSweepRuleTweet scheduledSweepRuleTweet);
		Task CreateScheduledSweepRuleImage(Guid scheduledUid, ScheduledSweepRuleTweetImage image);
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
			if(myTransaction != null)
			{
				await myTransaction.RollbackAsync();
			}
		}

		public async Task CreateScheduledSweepRuleTweetAsync(ScheduledSweepRuleTweet model)
		{
			var command = myConnection.CreateCommand();
			command.Transaction = myTransaction;
			command.CommandText = "scheduled_sweep_rule_tweet_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("ruleUid", model.Rule.RuleUid)
				.AddParameter("scheduledUid", model.ScheduledUid)
				.AddParameter("tweetText", model.Text)
				.AddParameter("scheduledTime", model.ScheduledTime)
				.AddParameter("scheduledTimeUtc", model.ScheduledTimeUtc)
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

			//await myConnection.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}

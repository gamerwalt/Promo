using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using System;
using System.Data;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace Promo.Harvester.Infrastructure
{
    public interface IHarvesterRepository
    {
        Task CreateHarvestedRecordAsync(Guid harvestedUid, Guid ruleUid, string json);
        Task CreateTweetUserAsync(Guid userUid, IUser user);
        Task CreateTweetRecordAsync(Guid tweetUid, Guid harvestedUid, Guid ruleUid, Guid userUid, ITweet tweet);
    }

    public class HarvesterRepository : IHarvesterRepository
    {
        private readonly IDatabase _database;

        public HarvesterRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task CreateHarvestedRecordAsync(Guid harvestedUid, Guid ruleUid, string json)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
            command.CommandText = "harvested_record_create";
            command.CommandType = CommandType.StoredProcedure;
            command
                .AddParameter(nameof(harvestedUid), harvestedUid)
                .AddParameter(nameof(ruleUid), ruleUid)
                .AddParameter("harvestedValue", json);

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task CreateTweetRecordAsync(Guid tweetUid, Guid harvestedUid, Guid ruleUid, Guid userUid, ITweet tweet)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
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

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task CreateTweetUserAsync(Guid userUid, IUser user)
        {
            using var conn = new MySqlConnection(_database.GetConnectionString());
            var command = conn.CreateCommand();
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

            await conn.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}

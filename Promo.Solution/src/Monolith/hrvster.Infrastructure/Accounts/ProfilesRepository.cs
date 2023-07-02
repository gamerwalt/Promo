using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Profiles;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Accounts
{
	public interface IProfilesRepository
	{
		Task UpsertProfileAsync(Profile profile);
		Task<Profile> GetProfileAsync(Guid userUid, ProfileType profileType);
		Task<IEnumerable<Profile>> GetProfilesAsync(Guid userUid);
		Task<Profile> GetProfileByCampaignUid(Guid campaignUid, ProfileType profileType);
		Task<Profile> GetProfileBelongingToSweepRuleAsync(Guid ruleUid, ProfileType profileType	);
		Task RemoveProfileAsync(Guid accountUid, ProfileType profileType);
		Task CreateProfileAsync(Profile profile);
	}

	public class ProfilesRepository : IProfilesRepository
	{
		private readonly IDatabase _database;

		public ProfilesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<Profile> GetProfileAsync(Guid userUid, ProfileType profileType)
		{
			var account = default(Profile);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "profile_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("userUid", userUid)
					.AddParameter("profileType", profileType);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					account = CreateRecordFrom(reader);
				}
			}

			return account;
		}

		private Profile CreateRecordFrom(MySqlDataReader reader)
		{
			return new Profile
			{
				ProfileUid = reader.GetGuid("profile_uid"),
				ProfileType = (ProfileType)Enum.Parse(typeof(ProfileType), reader.GetString("profile_type")),
				ExternalProfileId = reader.GetStringValue("external_profile_id"),
				DisplayName = reader.GetStringValue("display_name"),
				ScreenName = reader.GetStringValue("screen_name"),
				Description = reader.GetStringValue("profile_description"),
				RefreshToken = reader.GetStringValue("refresh_token"),
				AccessToken = reader.GetStringValue("access_token"),
				AccessTokenSecret = reader.GetStringValue("access_token_secret"),
				ConsumerKey = GetStringValue(reader, "consumer_key"),
				ConsumerSecret = GetStringValue(reader, "consumer_secret"),
				Location = reader.GetStringValue("location"),
				ProfileImageUrl = reader.GetStringValue("profile_image_url"),
				Account = new Account
				{
					AccountId = reader.GetInt32("account_id"),
					AccountUid = reader.GetGuid("account_uid")
				},
				UpdatedDate = reader.GetNullableDateTimeValue("updated_date").GetValueOrDefault()
			};
		}

		public async Task UpsertProfileAsync(Profile profile)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "profile_upsert";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("profileUid", profile.ProfileUid)
				.AddParameter("externalProfileId", profile.ExternalProfileId)
				.AddParameter("accountUid", profile.Account.AccountUid)
				.AddParameter("profileType", profile.ProfileType)
				.AddParameter("displayName", profile.DisplayName)
				.AddParameter("screenName", profile.ScreenName)
				.AddParameter("description", profile.Description)
				.AddParameter("refreshToken", profile.RefreshToken)
				.AddParameter("accessToken", profile.AccessToken)
				.AddParameter("accessTokenSecret", profile.AccessTokenSecret)
				.AddParameter("consumerKey", profile.ConsumerKey)
				.AddParameter("consumerSecret", profile.ConsumerSecret)
				.AddParameter("location", profile.Location)
				.AddParameter("profileImageUrl", profile.ProfileImageUrl);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<Profile>> GetProfilesAsync(Guid userUid)
		{
			var profiles = new List<Profile>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "profiles_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("userUid", userUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					profiles.Add(CreateRecordFrom(reader));
				}
			}

			return profiles;
		}

		private string GetStringValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return string.Empty;
			}

			return reader.GetString(fieldName);
		}

		public async Task<Profile> GetProfileByCampaignUid(Guid campaignUid, ProfileType profileType)
		{
			var profile = default(Profile);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "profile_get_by_campaign_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(campaignUid), campaignUid)
					.AddParameter("profileType", profileType);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					profile = CreateRecordFrom(reader);
				}
			}

			return profile;
		}

		public async Task<Profile> GetProfileBelongingToSweepRuleAsync(Guid ruleUid, ProfileType profileType)
		{
			var profile = default(Profile);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "profile_get_by_rule_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(ruleUid), ruleUid)
					.AddParameter("profileType", profileType);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					profile = CreateRecordFrom(reader);
				}
			}

			return profile;
		}

		public async Task RemoveProfileAsync(Guid accountUid, ProfileType profileType)
		{
			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "profile_remove";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(accountUid), accountUid)
					.AddParameter("profileType", profileType);

				await conn.OpenAsync();
				await command.ExecuteNonQueryAsync();
			}
		}

		public async Task CreateProfileAsync(Profile profile)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "profile_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("profileUid", profile.ProfileUid)
				.AddParameter("externalProfileId", profile.ExternalProfileId)
				.AddParameter("accountUid", profile.Account.AccountUid)
				.AddParameter("profileType", profile.ProfileType)
				.AddParameter("displayName", profile.DisplayName)
				.AddParameter("screenName", profile.ScreenName)
				.AddParameter("description", profile.Description)
				.AddParameter("refreshToken", profile.RefreshToken)
				.AddParameter("accessToken", profile.AccessToken)
				.AddParameter("accessTokenSecret", profile.AccessTokenSecret)
				.AddParameter("consumerKey", profile.ConsumerKey)
				.AddParameter("consumerSecret", profile.ConsumerSecret)
				.AddParameter("location", profile.Location)
				.AddParameter("profileImageUrl", profile.ProfileImageUrl);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}

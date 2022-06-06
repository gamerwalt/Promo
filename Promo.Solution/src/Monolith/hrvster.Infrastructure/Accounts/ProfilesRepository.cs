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
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						account = CreateRecordFrom(reader);
					}
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
				ExternalProfileId = reader.GetString("external_profile_id"),
				DisplayName = reader.GetString("display_name"),
				ScreenName = reader.GetString("screen_name"),
				Description = reader.GetString("profile_description"),
				AccessToken = reader.GetString("access_token"),
				AccessTokenSecret = reader.GetString("access_token_secret"),
				Location = reader.GetString("location"),
				ProfileImageUrl = reader.GetString("profile_image_url"),
				Account = new Account
				{
					AccountId = reader.GetInt32("account_id"),
					AccountUid = reader.GetGuid("account_uid")
				}
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
				.AddParameter("accessToken", profile.AccessToken)
				.AddParameter("accessTokenSecret", profile.AccessTokenSecret)
				.AddParameter("location", profile.Location)
				.AddParameter("profileImageUrl", profile.ProfileImageUrl);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<Profile>> GetProfilesAsync(Guid userUid)
		{
			var profiles = default(List<Profile>);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "profiles_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("userUid", userUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						profiles.Add(CreateRecordFrom(reader));
					}
				}
			}

			return profiles;
		}
	}
}

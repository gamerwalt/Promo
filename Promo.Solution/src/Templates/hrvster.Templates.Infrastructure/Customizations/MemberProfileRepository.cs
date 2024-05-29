using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Extensions;
using hrvster.Templates.Models.Members;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Data;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface IMemberProfileRepository
	{
		Task CreateMemberProfileAsync(Guid accountUid, MemberProfile memberProfile);
		Task<MemberProfile?> GetByAccessTokenAsync(string authKey);
		Task<MemberProfile> GetByExternalProfileIdAsync(Guid accountUid, string externalProfileId);
		Task UpdateMemberProfileAsync(MemberProfile memberProfile);
	}

	public class MemberProfileRepository : IMemberProfileRepository
	{
		private readonly ILogger<MemberProfileRepository> _logger;
		private readonly hrvster.Infrastructure.IDatabase _database;

		public MemberProfileRepository(hrvster.Infrastructure.IDatabase database, ILoggerFactory loggerFactory)
		{
			_database = database;
			_logger = loggerFactory.CreateLogger<MemberProfileRepository>();
		}

		public async Task CreateMemberProfileAsync(Guid accountUid, MemberProfile memberProfile)
		{
			_logger.LogInformation("Creating the user with {User}", memberProfile.ToJsonString());

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "member_profile_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", accountUid)
				.AddParameter("memberUid", memberProfile.Member.Uid)
				.AddParameter("memberProfileUid", memberProfile.Uid)
				.AddParameter("externalProfileId", memberProfile.ExternalProfileId)
				.AddParameter("profileType", memberProfile.ProfileType.ToString())
				.AddParameter("displayName", memberProfile.Name)
				.AddParameter("screenName", memberProfile.ScreenName)
				.AddParameter("description", memberProfile.Description)
				.AddParameter("accessToken", memberProfile.AccessToken)
				.AddParameter("refreshToken", memberProfile.RefreshToken);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<MemberProfile?> GetByAccessTokenAsync(string authKey)
		{
			var memberProfile = default(MemberProfile);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "member_profile_get_by_access_token";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accessToken", authKey);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					memberProfile = ReadMemberProfile(reader);
				}
			}

			return memberProfile;
		}

		public async Task<MemberProfile> GetByExternalProfileIdAsync(Guid accountUid, string externalProfileId)
		{
			var memberProfile = default(MemberProfile);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "member_profile_get_by_external_profile_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("externalProfileId", externalProfileId)
					.AddParameter("accountUid", accountUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					memberProfile = ReadMemberProfile(reader);
				}
			}

			return memberProfile;
		}

		public async Task UpdateMemberProfileAsync(MemberProfile memberProfile)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "member_profile_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("memberProfileUid", memberProfile.Uid)
				.AddParameter("externalProfileId", memberProfile.ExternalProfileId)
				.AddParameter("profileType", memberProfile.ProfileType.ToString())
				.AddParameter("displayName", memberProfile.Name)
				.AddParameter("screenName", memberProfile.ScreenName)
				.AddParameter("description", memberProfile.Description)
				.AddParameter("accessToken", memberProfile.AccessToken)
				.AddParameter("refreshToken", memberProfile.RefreshToken);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		private MemberProfile? ReadMemberProfile(MySqlDataReader reader)
		{
			return new MemberProfile
			{
				Id = reader.GetInt32("member_profile_id"),
				Uid = reader.GetGuid("member_profile_uid"),
				Member = new Member { Id = reader.GetInt32("member_id") },
				ExternalProfileId = reader.GetStringValue("external_profile_id"),
				ProfileType = Enum.Parse<ProfileType>(reader.GetStringValue("profile_type")),
				ScreenName = reader.GetStringValue("profile_name"),
				Name = reader.GetStringValue("display_name"),
				Description = reader.GetStringValue("description"),
				PictureUrl = reader.GetStringValue("member_picture_url"),
				AccessToken = reader.GetStringValue("access_token"),
				RefreshToken = reader.GetStringValue("refresh_token")
			};
		}
	}
}

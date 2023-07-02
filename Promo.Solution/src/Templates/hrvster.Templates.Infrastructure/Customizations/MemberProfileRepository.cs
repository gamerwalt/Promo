using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Models.Members;
using Microsoft.AspNetCore.Http;
using MySqlConnector;
using System.Data;
using Constants = hrvster.Templates.Core.Common.Constants;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface IMemberProfileRepository
	{
		Task CreateMemberProfileAsync(MemberProfile memberProfile);
		Task<MemberProfile> GetByExternalProfileIdAsync(string externalProfileId);
		Task UpdateMemberProfileAsync(MemberProfile memberProfile);
	}

	public class MemberProfileRepository : IMemberProfileRepository
	{
		private readonly IConnectionStringProvider _connectionStringProvider;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public MemberProfileRepository(
			IConnectionStringProvider connectionStringProvider,
			IHttpContextAccessor httpContextAccessor)
		{
			_connectionStringProvider = connectionStringProvider;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task CreateMemberProfileAsync(MemberProfile memberProfile)
		{
			await _connectionStringProvider.GetConnectionString();

			object? connectionString, subDomain;

			SetDetailsNeeded(out connectionString, out subDomain);

			using var conn = new MySqlConnection(connectionString.ToString());
			var command = conn.CreateCommand();
			command.CommandText = "member_profile_create";
			command.CommandType = CommandType.StoredProcedure;
			command
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

		public async Task<MemberProfile> GetByExternalProfileIdAsync(string externalProfileId)
		{
			var memberProfile = default(MemberProfile);

			await _connectionStringProvider.GetConnectionString();

			object? connectionString, subDomain;

			SetDetailsNeeded(out connectionString, out subDomain);

			using (var conn = new MySqlConnection(connectionString.ToString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "member_profile_get_by_external_profile_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("externalProfileId", externalProfileId);

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
			await _connectionStringProvider.GetConnectionString();

			object? connectionString, subDomain;

			SetDetailsNeeded(out connectionString, out subDomain);

			using var conn = new MySqlConnection(connectionString.ToString());
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

		private void SetDetailsNeeded(out object? connectionString, out object? subDomain)
		{
			_httpContextAccessor.HttpContext.Items.TryGetValue(Constants.ContextItemsConnectionString, out connectionString);
			_httpContextAccessor.HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out subDomain);
		}
	}
}

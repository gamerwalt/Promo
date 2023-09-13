using hrvster.Infrastructure.Extensions;
using hrvster.Templates.Models.Members;
using MySqlConnector;
using System.Data;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface IMemberRepository
	{
		Task CreateMemberAsync(Guid accountUid, Member newMember);
		Task<Member> GetMemberViaExternalProfileIdAsync(Guid accountUid, string profileType, string externalProfileId);
	}

	public class MemberRepository : IMemberRepository
	{
		private readonly hrvster.Infrastructure.IDatabase _database;

		public MemberRepository(hrvster.Infrastructure.IDatabase database)
		{
			_database = database;
		}

		public async Task CreateMemberAsync(Guid accountUid, Member member)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "member_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", accountUid)
				.AddParameter("memberUid", member.Uid)
				.AddParameter("emailAddress", member.EmailAddress)
				.AddParameter("firstName", member.FirstName)
				.AddParameter("lastName", member.LastName)
				.AddParameter("phoneNumber", member.PhoneNumber);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<Member> GetMemberViaExternalProfileIdAsync(
			Guid accountUid,
			string profileType,
			string externalProfileId)
		{
			var member = default(Member);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "member_get_by_external_profile_id";
				command.CommandType = CommandType.StoredProcedure;
				command
					.AddParameter("accountUid", accountUid)
					.AddParameter("externalProfileId", externalProfileId)
					.AddParameter("profileType", profileType);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					member = CreateMemberFrom(reader);
				}
			}

			return member;
		}

		private Member? CreateMemberFrom(MySqlDataReader reader)
		{
			return new Member
			{
				Id = reader.GetInt32("member_id"),
				Uid = reader.GetGuid("member_uid"),
				FirstName = reader.GetStringValue("first_name"),
				LastName = reader.GetStringValue("last_name"),
				EmailAddress = reader.GetStringValue("email_address"),
				PhoneNumber = reader.GetStringValue("phone_number"),
				Profiles = new List<MemberProfile>
				{
					new MemberProfile
					{
						ExternalProfileId = reader.GetStringValue("external_profile_id"),
						Uid = reader.GetGuid("member_profile_uid"),
						Id = reader.GetInt32("member_profile_id"),
						ScreenName = reader.GetStringValue("profile_name"),
						Name = reader.GetStringValue("display_name"),
						AccessToken = reader.GetStringValue("access_token"),
						RefreshToken = reader.GetStringValue("refresh_token")
					}
				}
			};
		}
	}
}

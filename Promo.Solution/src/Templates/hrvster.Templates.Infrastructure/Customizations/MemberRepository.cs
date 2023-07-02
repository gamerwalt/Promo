using hrvster.Infrastructure;
using hrvster.Infrastructure.Extensions;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Models.Members;
using Microsoft.AspNetCore.Http;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface IMemberRepository
	{
		Task CreateMemberAsync(Member newMember);
		Task<Member> GetMemberViaExternalProfileIdAsync(string profileType, string externalProfileId);
	}

	public class MemberRepository : IMemberRepository
	{
		private readonly IConnectionStringProvider _connectionStringProvider;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public MemberRepository(
			IConnectionStringProvider connectionStringProvider,
			IHttpContextAccessor httpContextAccessor)
		{
			_connectionStringProvider = connectionStringProvider;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task CreateMemberAsync(Member member)
		{
			await _connectionStringProvider.GetConnectionString();

			object? connectionString, subDomain;

			SetDetailsNeeded(out connectionString, out subDomain);

			using var conn = new MySqlConnection(connectionString.ToString());
			var command = conn.CreateCommand();
			command.CommandText = "member_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("memberUid", member.Uid)
				.AddParameter("emailAddress", member.EmailAddress)
				.AddParameter("firstName", member.FirstName)
				.AddParameter("lastName", member.LastName)
				.AddParameter("address1", member.Address1)
				.AddParameter("address2", member.Address2)
				.AddParameter("phoneNumber", member.PhoneNumber)
				.AddParameter("state", member.State)
				.AddParameter("city", member.City);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<Member> GetMemberViaExternalProfileIdAsync(string profileType, string externalProfileId)
		{
			var member = default(Member);

			await _connectionStringProvider.GetConnectionString();

			object? connectionString, subDomain;

			SetDetailsNeeded(out connectionString, out subDomain);

			using (var conn = new MySqlConnection(connectionString.ToString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "member_get_by_external_profile_id";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("externalProfileId", externalProfileId)
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
				Address1 = reader.GetStringValue("address1"),
				Address2 = reader.GetStringValue("address2"),
				PhoneNumber = reader.GetStringValue("phone_number"),
				State = reader.GetStringValue("state"),
				City = reader.GetStringValue("city"),
				Country = reader.GetStringValue("country"),
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

		private void SetDetailsNeeded(out object? connectionString, out object? subDomain)
		{
			_httpContextAccessor.HttpContext.Items.TryGetValue(Constants.ContextItemsConnectionString, out connectionString);
			_httpContextAccessor.HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out subDomain);
		}
	}
}

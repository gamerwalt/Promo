using hrvster.Models.Profiles;
using MySqlConnector;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Accounts
{
	public interface IProfilesRepository
	{
		Task UpsertProfileAsync(Profile profile);
	}

	public class ProfilesRepository : IProfilesRepository
	{
		private readonly IDatabase _database;

		public ProfilesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task UpsertProfileAsync(Profile profile)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "sweep_rule_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("profileUid", profile.ProfileUid)
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
	}
}

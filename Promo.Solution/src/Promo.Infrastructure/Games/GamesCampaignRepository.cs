using MySqlConnector;
using Promo.Core;
using Promo.Core.Extensions;
using Promo.Core.MessageContracts;
using System.Data;
using System.Threading.Tasks;

namespace Promo.Infrastructure.Games
{
	public interface IGamesCampaignRepository
	{
		Task CreateAsync(ICampaignCreated campaignCreated);
	}

	public class GamesCampaignRepository : IGamesCampaignRepository
	{
		private readonly IDatabase _database;
		public GamesCampaignRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(ICampaignCreated campaignCreated)
		{
			using var conn = new MySqlConnection(_database.GetGamesConnectionstring());
			var command = conn.CreateCommand();
			command.CommandText = "campaign_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("accountUid", campaignCreated.AccountUid)
				.AddParameter("campaignUid", campaignCreated.CampaignUid)
				.AddParameter("campaignTypeCode", campaignCreated.CampaignTypeCode)
				.AddParameter("activeInd", campaignCreated.ActiveInd)
				.AddParameter("startDate", campaignCreated.StartDate)
				.AddParameter("endDate", campaignCreated.EndDate);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}
	}
}

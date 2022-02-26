using hrvster.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface ICampaignTypesRepository
	{
		Task<IEnumerable<CampaignType>> GetAllAsync();
		Task<CampaignType> GetCampaignTypeByUidAsync(Guid campaignTypeUid);
	}

	public class CampaignTypesRepository : ICampaignTypesRepository
	{
		private readonly IDatabase _database;
		public CampaignTypesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<IEnumerable<CampaignType>> GetAllAsync()
		{
			var campaignTypes = new List<CampaignType>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaign_types_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					campaignTypes.Add(CreateCampaignTypeFrom(reader));
				}
			}

			return campaignTypes;
		}

		public async Task<CampaignType> GetCampaignTypeByUidAsync(Guid campaignTypeUid)
		{
			var campaignType = default(CampaignType);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "campaign_type_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("campaignTypeUid", campaignTypeUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					campaignType = CreateCampaignTypeFrom(reader);
				}
			}

			return campaignType;
		}

		private CampaignType CreateCampaignTypeFrom(MySqlDataReader reader)
		{
			return new CampaignType
			{
				CampaignTypeId = reader.GetInt32("campaign_type_id"),
				CampaignTypeUid = reader.GetGuid("campaign_type_uid"),
				CampaignTypeCode = (CampaignTypeCode)Enum.Parse(typeof(CampaignTypeCode), reader.GetString("campaign_type_code")),
				CampaignTypeName = reader.GetString("campaign_type_name"),
				CamapaignTypeDescription = reader.GetString("campaign_type_description"),
				IsActive = reader.GetBoolean("active_ind"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = GetNullableDateTimeValue(reader, "updated_date")
			};
		}

		private DateTime? GetNullableDateTimeValue(MySqlDataReader reader, string fieldName)
		{
			var value = reader[fieldName].ToString();
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			return reader.GetDateTime("updated_date");
		}
	}
}

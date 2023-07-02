using hrvster.Infrastructure.Extensions;
using hrvster.Models.Company;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Company
{
	public interface ICompanyImagesRepository
	{
		Task<ClientImage> GetCompanyImages(Guid accountUid);
	}

	public class CompanyImagesRepository : ICompanyImagesRepository
	{
		private readonly IDatabase _database;

		public CompanyImagesRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task<ClientImage> GetCompanyImages(Guid accountUid)
		{
			var clientImage = default(ClientImage);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "company_images_get_by_account_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						clientImage = CreateRecordFrom(reader);
					}
				}
			}

			return clientImage;
		}

		private ClientImage CreateRecordFrom(MySqlDataReader reader)
		{
			return new ClientImage
			{
				Id = reader.GetInt32("id"),
				Uid = reader.GetGuid("uid"),
				Company = new AccountCompany { Id = reader.GetInt32("company_id") },
				LogoLocation = reader.GetStringValue("logo_location"),
				HeaderLocation = reader.GetStringValue("header_location"),
				FooterLocation = reader.GetStringValue("footer_location")
			};
		}
	}
}

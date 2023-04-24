using hrvster.Games.Match.Models;
using hrvster.Infrastructure.Extensions;
using hrvster.Models.Facebook;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Facebook
{
	public interface IFacebookPageRepository
	{
		Task CreateAsync(Guid userUid, FacebookPage page);
		Task DeleteAllPagesAsync(Guid userUid);
		Task<IEnumerable<FacebookPage>> GetAllPagesAsync(Guid userUid);
		Task<FacebookPage> GetPageAsync(string pageId);
	}

	public class FacebookPageRepository : IFacebookPageRepository
	{
		private readonly IDatabase _database;

		public FacebookPageRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(Guid userUid, FacebookPage page)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_profile_pages_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("pageUid", Guid.NewGuid())
				.AddParameter("pageId", page.Id)
				.AddParameter("pageName", page.Name)
				.AddParameter("pageAccessToken", page.PageAccessToken)
				.AddParameter("accountUid", userUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task DeleteAllPagesAsync(Guid userUid)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "account_profile_pages_delete";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("accountUid", userUid);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<FacebookPage>> GetAllPagesAsync(Guid userUid)
		{
			var pages = new List<FacebookPage>();

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_profile_pages_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", userUid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					pages.Add(CreateFrom(reader));
				}
			}

			return pages;
		}

		public async Task<FacebookPage> GetPageAsync(string pageId)
		{
			var page = default(FacebookPage);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "account_profile_page_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("pageId", pageId);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					page = CreateFrom(reader);
				}
			}

			return page;
		}

		private FacebookPage CreateFrom(MySqlDataReader reader)
		{
			return new FacebookPage
			{
				Uid = reader.GetGuid("page_uid"),
				Id = reader.GetStringValue("page_id"),
				Name = reader.GetStringValue("page_name"),
				PageAccessToken = reader.GetStringValue("page_access_token")
			};
		}
	}
}

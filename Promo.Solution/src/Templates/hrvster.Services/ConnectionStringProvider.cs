using hrvster.Infrastructure.Extensions;
using hrvster.Templates.Core.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;

namespace hrvster.Services
{
	public interface IConnectionStringProvider
	{
		Task SetConnectionString();
	}

	public class ConnectionStringProvider : IConnectionStringProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _configuration;

		public ConnectionStringProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}

		public async Task SetConnectionString()
		{
			string connectionString = string.Empty;

			var subdomain = _httpContextAccessor.HttpContext.Items[Constants.ContextItemsSubdomain];

			using (var connection = new MySqlConnection(_configuration.GetConnectionString("hrvsterAdmin")))
			{
				var command = connection.CreateCommand();
				command.CommandText = "connection-string_get_by_subdomain";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter(nameof(subdomain), subdomain);

				await connection.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					connectionString = GetConnectionStringFrom(reader);
				}
			}

			_httpContextAccessor.HttpContext.Items.Add(Constants.ContextItemsConnectionString, connectionString);
		}

		private string GetConnectionStringFrom(MySqlDataReader reader)
		{
			return reader.GetStringValue("connectionString");
		}
	}
}
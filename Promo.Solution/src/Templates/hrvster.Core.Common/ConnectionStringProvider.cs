using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using hrvster.Infrastructure.Extensions;
using hrvster.Templates.Core.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace hrvster.Templates.Core.Common
{
	public interface IConnectionStringProvider
	{
		Task GetConnectionString();
	}

	public class ConnectionStringProvider : IConnectionStringProvider
	{
		private readonly ILogger<ConnectionStringProvider> _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _configuration;

		public ConnectionStringProvider(
			ILoggerFactory loggerFactory,
			IHttpContextAccessor httpContextAccessor, 
			IConfiguration configuration)
		{
			_logger = loggerFactory.CreateLogger<ConnectionStringProvider>();
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}

		public async Task GetConnectionString()
		{
			string connectionString = string.Empty;
			var hrvsterConnectionString = string.Empty;

			var subdomain = _httpContextAccessor.HttpContext.Items[Constants.ContextItemsSubdomain];

			_logger.LogInformation("Getting connection string for subdomain {subdomain}", subdomain);

			try
			{
				hrvsterConnectionString = _configuration.GetConnectionString("hrvster");
				
				using (var connection = new MySqlConnection(hrvsterConnectionString))
				{
					var command = connection.CreateCommand();
					command.CommandText = "connectionStringGetBySubdomain";
					command.CommandType = CommandType.StoredProcedure;
					command.AddParameter(nameof(subdomain), subdomain);

					await connection.OpenAsync();
					using var reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						connectionString = GetConnectionStringFrom(reader);
					}
				}

				if (_httpContextAccessor.HttpContext.Items.ContainsKey(Constants.ContextItemsConnectionString))
					_httpContextAccessor.HttpContext.Items.Remove(Constants.ContextItemsConnectionString);

				_httpContextAccessor.HttpContext.Items.TryAdd(Constants.ContextItemsConnectionString, connectionString);
			}
			catch (Exception ex)
			{
				_logger.LogError("Unable to get the connection string for the subdomain {subdomain}", subdomain);
				_logger.LogError("Error: {errorMessage}", ex.Message);

				throw;
			}

			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ConnectionStringException("Unable to retrieve the connection string of this subdomain.");
		}

		private string GetConnectionStringFrom(MySqlDataReader reader)
		{
			return reader.GetStringValue("connection_string");
		}
	}
}
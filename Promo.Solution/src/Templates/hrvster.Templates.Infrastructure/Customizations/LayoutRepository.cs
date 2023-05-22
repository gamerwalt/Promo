using hrvster.Infrastructure.Extensions;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Models.Layouts;
using Microsoft.AspNetCore.Http;
using MySqlConnector;
using System.Data;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface ILayoutRepository
	{
		Task<LayoutProperty> GetLayoutProperties(CancellationToken cancellationToken);
	}

	public class LayoutRepository : ILayoutRepository
	{
		private readonly IConnectionStringProvider _connectionStringProvider;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public LayoutRepository(IConnectionStringProvider connectionStringProvider, IHttpContextAccessor httpContextAccessor)
		{
			_connectionStringProvider = connectionStringProvider;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<LayoutProperty> GetLayoutProperties(CancellationToken cancellationToken)
		{
			await _connectionStringProvider.GetConnectionString();

			object? connectionString, subDomain;
			
			SetDetailsNeeded(out connectionString, out subDomain);

			var layoutProperty = default(LayoutProperty);

			using (var conn = new MySqlConnection(connectionString.ToString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "layout_property_get";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					layoutProperty = ReadLayoutPropertyFrom(reader);
				}
			}

			return layoutProperty;
		}

		private LayoutProperty? ReadLayoutPropertyFrom(MySqlDataReader reader)
		{
			return new LayoutProperty
			{
				LogoLocation = reader.GetStringValue("logo_location"),
				LogoType = reader.GetStringValue("logo_type"),
				HeaderLocation = reader.GetStringValue("header_location"),
				HeaderType = reader.GetStringValue("header_type"),
				FooterLocation = reader.GetStringValue("footer_location"),
				FooterType = reader.GetStringValue("footer_type")
			};
		}

		private void SetDetailsNeeded(out object? connectionString, out object? subDomain)
		{
			_httpContextAccessor.HttpContext.Items.TryGetValue(Constants.ContextItemsConnectionString, out connectionString);
			_httpContextAccessor.HttpContext.Items.TryGetValue(Constants.ContextItemsSubdomain, out subDomain);
		}
	}
}

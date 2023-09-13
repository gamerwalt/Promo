using hrvster.Infrastructure.Extensions;
using hrvster.Templates.Models.Layouts;
using MySqlConnector;
using System.Data;

namespace hrvster.Templates.Infrastructure.Customizations
{
	public interface ILayoutRepository
	{
		Task<LayoutProperty> GetLayoutProperties(Guid accountUid, CancellationToken cancellationToken);
	}

	public class LayoutRepository : ILayoutRepository
	{
		private readonly hrvster.Infrastructure.IDatabase _database;
		public LayoutRepository(hrvster.Infrastructure.IDatabase database)
		{
			_database = database;
		}

		public async Task<LayoutProperty> GetLayoutProperties(Guid accountUid, CancellationToken cancellationToken)
		{
			var layoutProperty = default(LayoutProperty);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "layout_property_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("accountUid", accountUid);

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
				HeaderLocation = reader.GetStringValue("header_location"),
				FooterLocation = reader.GetStringValue("footer_location")
			};
		}
	}
}

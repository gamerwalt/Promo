using FluentAssertions;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System.Data;

namespace TestUtilities.Common.Steps
{
	public static class SweepRepositorySteps
	{
		public static TFeature SweepsTableShouldContain<TFeature>(
			this IFeature<TFeature> feature,
			string fieldName,
			string value) where TFeature : class
		{
			var sweep = default(Sweep);

			var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
			using var conn = new MySqlConnection(hrvsterConnectionString);
			var command = conn.CreateCommand();
			command.CommandText = $"SELECT * FROM sweep WHERE {fieldName} = '{value}'";
			command.CommandType = CommandType.Text;
			conn.Open();
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				sweep = CreateSweepFrom(reader);
			}

			sweep.Should().NotBeNull();

			return feature as TFeature;
		}

		private static Sweep CreateSweepFrom(MySqlDataReader reader)
		{
			return new Sweep
			{
				SweepId = reader.GetInt32("sweep_id"),
				SweepUid = reader.GetGuid("sweep_uid"),
				Name = reader.GetString("sweep_name"),
				RulesUrl = reader.GetStringValue("rules_url"),
				StartDate = reader.GetDateTime("start_date"),
				StartDateUtc = reader.GetDateTime("start_date_utc"),
				EndDate = reader.GetDateTime("end_date"),
				EndDateUtc = reader.GetDateTime("end_date_utc"),
				DrawDate = reader.GetNullableDateTimeValue("draw_date").GetValueOrDefault(),
				DrawDateUtc = reader.GetNullableDateTimeValue("draw_date_utc").GetValueOrDefault()
			};
		}
	}
}

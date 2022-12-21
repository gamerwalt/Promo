using FluentAssertions;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Core;
using MySqlConnector;
using System;
using System.Data;

namespace TestUtilities.Common.Steps
{
	public static class UserActivityRepositorySteps
	{
		public static TFeature ActivitiesTableShouldContaine<TFeature>(this IFeature<TFeature> feature,
			string fieldName, string value) where TFeature : class
		{
			var activity = default(UserActivity);

			var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
			using var conn = new MySqlConnection(hrvsterConnectionString);
			var command = conn.CreateCommand();
			command.CommandText = $"SELECT * FROM user_activities WHERE {fieldName} = '{value}'";
			command.CommandType = CommandType.Text;
			conn.Open();
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				activity = CreateActivityFrom(reader);
			}

			activity.Should().NotBeNull();

			return feature as TFeature;
		}

		private static UserActivity CreateActivityFrom(MySqlDataReader reader)
		{
			return new UserActivity
			{
				ActivityId = reader.GetInt32("activity_id"),
				ActivityUid = reader.GetGuid("activity_uid"),
				Account = new hrvster.Models.Account
				{
					AccountId = reader.GetInt32("account_id")
				},
				LinkedUid = reader.GetGuid("linked_uid"),
				ActivityType = Enum.Parse<ActivityType>(reader.GetStringValue("activity_type")),
				ActivityTableBaseName = reader.GetStringValue("activity_table_base_name"),
				CreatedDate = reader.GetNullableDateTimeValue("created_date").Value,
				CreatedDateUTC = reader.GetNullableDateTimeValue("created_date_utc").Value
			};
		}
	}
}

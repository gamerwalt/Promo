using FluentAssertions;
using hrvster.Models;
using MySqlConnector;
using System.Data;

namespace TestUtilities.Common.Steps
{
    public static class SweepNotificationRepositoryStep
    {
        public static TFeature SweepNotificationTableShouldContainFieldEqualTo<TFeature>(this IFeature<TFeature> feature, string fieldName, string value) where TFeature : class
        {
            var sweepNotification = default(SweepNotification);

            var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
            using var conn = new MySqlConnection(hrvsterConnectionString);
            var command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM sweep_notifications WHERE {fieldName} = '{value}'";
            command.CommandType = CommandType.Text;
            conn.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                sweepNotification = CreateFrom(reader);
            }

            sweepNotification.Should().NotBeNull();

            return feature as TFeature;
        }

        private static SweepNotification CreateFrom(MySqlDataReader reader)
        {
            return new SweepNotification
            {
                Sweep = new Sweep { SweepId = reader.GetInt32("sweep_id") },
                DrawAttemptHappened = reader.GetBoolean("draw_attempted"),
                DrawHappened = reader.GetBoolean("draw_happened"),
                NotEnoughUniqueEntrants = reader.GetBoolean("not_enough_unique_entrants"),
                SweepsGoingLive = reader.GetBoolean("sweeps_going_live"),
                SweepsLive = reader.GetBoolean("sweeps_live")
            };
        }
    }
}

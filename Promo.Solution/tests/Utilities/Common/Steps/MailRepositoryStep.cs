using FluentAssertions;
using hrvster.Models;
using MySqlConnector;
using System.Data;

namespace TestUtilities.Common.Steps
{
    public static class MailRepositoryStep
    {
        public static TFeature ThenTheMailTableShouldContainAnEmailNotificationFor<TFeature>(
            this IWebFeature<TFeature> feature,
            string emailAddress) where TFeature : class
        {
            var email = default(Email);

            var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
            using var conn = new MySqlConnection(hrvsterConnectionString);
            var command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM emails WHERE email_address_to = '{emailAddress}'";
            command.CommandType = CommandType.Text;
            conn.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                email = CreateEmailFrom(reader);
            }

            email.Should().NotBeNull();
            email.EmailAddress.Should().Be(emailAddress);

            return feature as TFeature;
        }

		public static TFeature EmailsTableShouldContainFieldEqualTo<TFeature>(this IFeature<TFeature> feature,
			string fieldName, string value) where TFeature : class
		{
			var email = default(Email);

			var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
			using var conn = new MySqlConnection(hrvsterConnectionString);
			var command = conn.CreateCommand();
			command.CommandText = $"SELECT * FROM emails WHERE {fieldName} = '{value}'";
			command.CommandType = CommandType.Text;
			conn.Open();
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				email = CreateEmailFrom(reader);
			}

			email.Should().NotBeNull();

			return feature as TFeature;
		}

        public static TFeature EmailsTableShouldContain<TFeature>(this IFeature<TFeature> feature,
            string fieldName, string value) where TFeature : class
        {
			var email = default(Email);

			var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
			using var conn = new MySqlConnection(hrvsterConnectionString);
			var command = conn.CreateCommand();
			command.CommandText = $"SELECT * FROM emails WHERE {fieldName} LIKE '%{value}%'";
			command.CommandType = CommandType.Text;
			conn.Open();
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				email = CreateEmailFrom(reader);
			}

			email.Should().NotBeNull();

			return feature as TFeature;
		}

		private static Email CreateEmailFrom(MySqlDataReader reader)
        {
            return new Email
            {
                EmailAddress = reader.GetString("email_address_to"),
            };
        }
    }
}

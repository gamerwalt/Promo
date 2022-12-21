using FluentAssertions;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System;
using System.Data;

namespace TestUtilities.Common.Steps
{
    public static class AccountRepositorySteps
    {
        public static TFeature ThenTheAccountsTableShouldContainEmailOf<TFeature>(
            this IWebFeature<TFeature> feature,
            string emailAddress) where TFeature : class
        {
            var account = feature.AccountsRepository.GetUserByEmail(emailAddress);

            account.Should().NotBeNull();
            account.EmailAddress.Should().Be(emailAddress);

            return feature as TFeature;
        }

        public static TFeature AccountsTableShouldContain<TFeature>(this IFeature<TFeature> feature,
            string fieldName, string value) where TFeature : class
        {
			var account = default(Account);

			var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
			using var conn = new MySqlConnection(hrvsterConnectionString);
			var command = conn.CreateCommand();
			command.CommandText = $"SELECT * FROM accounts WHERE {fieldName} = '{value}'";
			command.CommandType = CommandType.Text;
			conn.Open();
			using var reader = command.ExecuteReader();
			while (reader.Read())
			{
				account = CreateAccountFrom(reader);
			}

			account.Should().NotBeNull();

			return feature as TFeature;
		}

		private static Account CreateAccountFrom(MySqlDataReader reader)
		{
			return new Account
			{
				AccountId = reader.GetInt32("account_id"),
				AccountUid = reader.GetGuid("account_uid"),
				EmailAddress = reader.GetString("email_address"),
				PhoneNumber = reader.GetStringValue("phone_number"),
				EmailVerified = reader.GetBoolean("email_address_verified"),
				PhoneNumberVerified = reader.GetBoolean("phone_number_verified"),
				FirstName = reader.GetStringValue("first_name"),
				LastName = reader.GetStringValue("last_name"),
				DisplayName = reader.GetStringValue("display_name"),
				Pin = reader.GetStringValue("pin"),
				TermsAccepted = reader.GetBoolean("terms_accepted"),
				TwitterUserName = reader.GetStringValue("twitter_user_name"),
				CreatedDate = reader.GetDateTime("created_date"),
				UpdatedDate = reader.GetNullableDateTimeValue("updated_date")
			};
		}
	}
}

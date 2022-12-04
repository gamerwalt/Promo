using FluentAssertions;
using MySqlConnector;
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
    }
}

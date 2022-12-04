using FluentAssertions;
using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using System.Data;

namespace TestUtilities.Common.Steps
{
    public static class PromotionRepositorySteps
    {
        public static TFeature ThenTheCampaignsTableShouldContainPromotionName<TFeature>(
            this IWebFeature<TFeature> feature,
            string promotionName) where TFeature : class
        {
            var promotion = default(Campaign);

            var hrvsterConnectionString = feature.Configuration["ConnectionStrings:hrvster"];
            using var conn = new MySqlConnection(hrvsterConnectionString);
            var command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM campaigns WHERE campaign_name = '{promotionName}'";
            command.CommandType = CommandType.Text;
            conn.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                promotion = CreateCampaignFrom(reader);
            }

            promotion.Should().NotBeNull();
            promotion.CampaignName.Should().Be(promotionName);

            return feature as TFeature;
        }

        private static Campaign CreateCampaignFrom(MySqlDataReader reader)
        {
            return new Campaign
            {
                CampaignName = reader.GetStringValue("campaign_name")
            };
        }
    }
}

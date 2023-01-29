using hrvster.Models;
using hrvster.Models.Profiles;
using System;
using TestUtilities.Data;

namespace TestUtilities.Builders.Resources
{
    public class ProfileBuilder
    {
        public Account Account { get; set; }
        public string AccessTokenSecret { get; set; }
        public string AccessToken { get; set; }
        public string Description { get; set; }
        public string ScreenName { get; set; }
        public string DisplayName { get; set; }
        public ProfileType ProfileType { get; set; }
        public Guid ProfileUid { get; set; }
        public string ExternalProfileId { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public ProfileBuilder(Guid profileUid, Account account, ProfileType profileType)
        {
            ProfileUid = profileUid;
            Account = account;
            ProfileType = profileType;
        }

        public ProfileBuilder Create(Guid profileUid, Account account, ProfileType profileType)
        {
            return new ProfileBuilder(profileUid, account, profileType)
                .WithAccessTokenSecret(TestData.AccessTokenSecret)
                .WithAccessToken(TestData.AccessToken)
                .WithDescription(TestData.ProfileDescription)
                .WithScreenName(TestData.ScreenName)
                .WithDisplayName(TestData.DisplayName)
                .WithExternalProfileId(TestData.ExternalProfileId)
                .WithConsumerKey(TestData.ConsumerKey)
                .WithConsumerSecret(TestData.ConsumerKeySecret);
        }

        public ProfileBuilder WithConsumerSecret(string consumerSecret)
        {
            ConsumerSecret = consumerSecret;

            return this;
        }

        public ProfileBuilder WithConsumerKey(string consumerKey)
        {
            ConsumerKey = consumerKey;

            return this;
        }

        public ProfileBuilder WithExternalProfileId(string externalProfileId)
        {
            ExternalProfileId = externalProfileId;

            return this;
        }

        public ProfileBuilder WithDisplayName(string displayName)
        {
            ScreenName = displayName;

            return this;
        }

        public ProfileBuilder WithScreenName(string screenName)
        {
            ScreenName = screenName;

            return this;
        }

        public ProfileBuilder WithDescription(string description)
        {
            Description = description;

            return this;
        }

        public ProfileBuilder WithAccessToken(string accessToken)
        {
            AccessToken = accessToken;

            return this;
        }

        public ProfileBuilder WithAccessTokenSecret(string accessTokenSecret)
        {
            AccessTokenSecret = accessTokenSecret;

            return this;
        }

        public Profile Build()
            => new Profile
            {

            };
    }
}

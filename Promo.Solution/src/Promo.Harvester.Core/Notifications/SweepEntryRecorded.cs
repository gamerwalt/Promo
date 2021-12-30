using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Parameters;

namespace Promo.Harvester.Core.Notifications
{
    /// <summary>
    /// Todo: Need to send tweet notifications to campaign owner and tweeter
    /// We should use this information to record entries into a reporting system or send a notification to the account holder
    /// of the campaign that a new sweep entry has been recorded
    /// We should also send a tweet notification to the user handle that we received their entry into the campaign
    /// </summary>
    public class SweepEntryRecorded : INotification
    {
        private Guid campaignUid;
        private string screenName;
        private string userIdentifierType;
        private string tweetUserId;
        private string tweetId;

        public SweepEntryRecorded(Guid campaignUid, string screenName, string tweetUserId, string tweetId, string userIdentifierType)
        {
            this.campaignUid = campaignUid;
            this.screenName = screenName;
            this.userIdentifierType = userIdentifierType;
            this.tweetUserId = tweetUserId;
            this.tweetId = tweetId;
        }

        public Guid CampaignUid
        {
            get { return campaignUid; }
        }

        public string ScreenName
        {
            get { return screenName; }
        }

        public string UserIdentifierType
        {
            get { return userIdentifierType; }
        }

        public string TweetUserId
        {
            get { return tweetUserId; }
        }

        public string TweetId
        {
            get { return tweetId; }
        }
    }

    public class SendNotificationToUser : INotificationHandler<SweepEntryRecorded>
    {
        private readonly ITwitterClient _twitterClient;

        public SendNotificationToUser(ITwitterClient twitterClient)
        {
            _twitterClient = twitterClient;
        }

        public async Task Handle(SweepEntryRecorded notification, CancellationToken cancellationToken)
        {
            
            var userId = notification.TweetUserId;
            var tweetId = notification.TweetId;

            var tweet = await _twitterClient.Tweets.GetTweetAsync(long.Parse(tweetId));

            var text = $"@{tweet.CreatedBy} You just got entered into a sweepstakes entry to win (Prize details will be placed here :)).";

            await _twitterClient.Tweets.PublishTweetAsync(new PublishTweetParameters (text){ InReplyToTweet = tweet });
        }
    }
}

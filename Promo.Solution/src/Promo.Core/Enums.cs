namespace Promo.Core
{
    public enum EmailStatus
    {
        New,
        Sending,
        Sent,
        Failed
    }

    public enum EmailTemplate
    {
        WelcomeEmail,
        VerificationEmail
    }

    public enum CampaignTypeCode
    {
        AutoAll,
        AutoReplyTweet,
        AutoReplyDM,
        AutoReplyHashtag,
        AutoReplyLikedTweet,
        AutoEnterSweepsFromTweet,
        AutoEnterSweepsFromHashtag
    }

    public enum UserIdentifierType
    {
        TwitterHandle,
        InstagramHandle,
        Email,
        Phone
    }
}

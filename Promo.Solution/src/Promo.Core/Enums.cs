using System.Runtime.Serialization;

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
		[EnumMember(Value = "AutoAll")]
		AutoAll,
		[EnumMember(Value = "AutoReplyTweet")]
		AutoReplyTweet,
		[EnumMember(Value = "AutoReplyDM")]
		AutoReplyDM,
		[EnumMember(Value = "AutoReplyHashtag")]
		AutoReplyHashtag,
		[EnumMember(Value = "AutoReplyLikedTweet")]
		AutoReplyLikedTweet,
		[EnumMember(Value = "AutoEnterSweepsFromTweet")]
		AutoEnterSweepsFromTweet,
		[EnumMember(Value = "AutoEnterSweepsFromHashtag")]
		AutoEnterSweepsFromHashtag
    }

    public enum UserIdentifierType
    {
        TwitterHandle,
        InstagramHandle,
        Email,
        Phone
    }

    public enum WatchTypeCode
    {
        Tweet,
        Hashtag,
        DM
    }
}

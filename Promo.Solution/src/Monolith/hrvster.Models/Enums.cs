namespace hrvster.Models
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
		AutoEnterSweepsFromHashtag,
		AutoEnterSweepsFromTweetReplyHashtag
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

	public enum ProfileType
	{
		Twitter
	}
}

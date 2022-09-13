namespace hrvster.Models
{
	public enum ExternalClientType
	{
		Twitter,
		Facebook,
		Instagram
	}

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
		VerificationEmail,
		PasswordResetEmail
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

	public enum ScheduledStatus
	{
		New,
		Sent,
		Failed
	}

	public enum RateLimiter
	{
		UserRateLimiter
	}
}

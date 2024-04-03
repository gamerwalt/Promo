namespace hrvster.Models
{
	public enum ExternalClientType
	{
		Client,
		Twitter,
		Facebook,
		Instagram
	}

	public enum FlutterwaveEventStatus
	{
		New,
		Processing,
		Success,
		Failed
	}

	public enum FlutterwaveCardTransactionStatus
	{
		Successful,
		Failed
	}

	public enum PaymentClient
	{
		Flutterwave,
		Stripe,
		Paystack,
		Okra
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
		PasswordResetEmail,
		EmailAddressUpdateRequest,
		SweepEntriesNotEnoughEmail,
		NotEnoughSweepstakeDistinctUsers,
		SweepsGoingLiveSoon,
		SweepsGoingLiveSoonMissingProperty,
		SweepLive,
		DrawCompleted,
		SweepstakeMaxEntries,
		NotifyAccountOfUserWinningPick3WinEmail,
		NotifyMemberOfPick3WinEmail,
		AccountSubscriptionUpdateEmail
	}

	public enum CampaignTypeCode
	{
		AutoAll,
		AutoReplyTweet,
		AutoReplyDM,
		AutoReplyHashtag,
		AutoReplyLikedTweet,
		AutoPlayFromTweet,
		AutoPlayFromHashtag,
		AutoPlayFromTweetReplyHashtag
	}

	public enum UserIdentifierType
	{
		TwitterHandle,
		FacebookHandle,
		InstagramHandle,
		Email,
		Phone,
		Client
	}

	public enum WatchTypeCode
	{
		Tweet,
		Hashtag,
		DM
	}

	public enum ProfileType
	{
		Client,
		Twitter,
		Facebook
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

	public enum SubscriptionStatus
	{
		Subscribed,
		UnSubscribed,
		PendingPayment,
		EndOfSubscription
	}

	public enum ActivityType
	{
		LoggedIn,
		LoggedOut,
		CreatedPromotion,
		CreatedPromotionByWizard,
		UpdatedPromotion,
		CreatedSweepstakes,
		UpdatedSweepstakes,
		AddedSweepstakesEntryRule,
		UpdatedSweepstakesEntryRule,
		AddedPrize,
		UpdatedPrize,
		DeletedPrize,
		AddedSweepstakesDrawRule,
		UpdatedSweepstakeDrawRule,
		CreatedSweepstakesTweet,
		UpdatedSweepstakesTweet,
		ActivatedSweepstakesTweet,
		DeactivatedSweepstakesTweet,
		ScheduledSweepstakesTweet,
		LinkedProfileToTwitter,
		Subscribed,
		Unsubscribed
	}

	public enum HarvesterServiceCommand
	{
		start,
		stop
	}

	public enum WebJobStatus
	{
		Initializing,
		Starting,
		Running,
		PendingRestart,
		Stopped,
		Aborted,
		Success,
		Failed
	}

	public enum SubscriptionPlans
	{
		Basic,
		Starter,
		Advanced
	}

	public enum GameTypesEnum
	{
		Sweeps,
		MatchGames
	}

	public enum StripeEventStatus
	{
		New,
		Processing,
		Processed
	}
}

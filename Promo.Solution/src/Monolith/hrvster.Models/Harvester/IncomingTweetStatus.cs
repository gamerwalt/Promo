namespace hrvster.Models.Harvester
{
	public enum IncomingTweetStatus
	{
		New,
		Processing,
		Retrying,
		CompletedUserNotFollowing,
		CompletedCampaignOrGameNotRunning,
		CompletedTooManyTweets,
		Completed,
		Failed
	}
}

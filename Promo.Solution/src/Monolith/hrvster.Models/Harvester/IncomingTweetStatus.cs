namespace hrvster.Models.Harvester
{
	public enum IncomingTweetStatus
	{
		New,
		Processing,
		Retrying,
		CompletedUserNotFollowing,
		CompletedCampaignOrSweepstakeNotRunning,
		Completed,
		Failed
	}
}

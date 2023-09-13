namespace hrvster.Exceptions
{
	public class MissingCampaignException : Exception
	{
		public MissingCampaignException() { }

		public MissingCampaignException(string message)
			: base(message) { }
	}
}
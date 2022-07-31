namespace hrvster.Models.Options
{
	public class SendGridOptions
	{
		public const string SectionName = "SendGrid";
		public string ApiKeyName { get; set; }
		public string ApiKey { get; set; }
		public string Mail { get; set; }
		public string DisplayName { get; set; }
	}
}

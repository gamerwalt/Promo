namespace hrvster.Models.TemplateMailRequest
{
	public class AccountSubscriptionUpdateEmailRequest
	{
		public EmailTemplate EmailTemplate { get; set; }
		public string EmailAddress { get; set; }
		public string CurrentAccountSubscriptionStatus { get; set; }
		public string NewAccountSubscriptionStatus { get; set; }
	}
}

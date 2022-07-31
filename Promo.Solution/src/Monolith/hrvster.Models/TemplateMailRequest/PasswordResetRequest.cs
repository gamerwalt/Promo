namespace hrvster.Models.TemplateMailRequest
{
	public class PasswordResetRequest
	{
		public string EmailAddress { get; set; }
		public string PasswordResetLink { get; set; }
		public EmailTemplate EmailTemplate { get; set; }
	}
}

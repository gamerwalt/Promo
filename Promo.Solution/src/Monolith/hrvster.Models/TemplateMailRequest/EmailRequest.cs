namespace hrvster.Models.TemplateMailRequest
{
	public abstract class EmailRequest
	{
		public string EmailAddress { get; set; }
		public string DisplayName { get; set; }
		public virtual EmailTemplate EmailTemplate { get; set; }
	}
}

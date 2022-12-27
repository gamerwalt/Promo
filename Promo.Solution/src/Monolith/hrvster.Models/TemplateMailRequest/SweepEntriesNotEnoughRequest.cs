namespace hrvster.Models.TemplateMailRequest
{
	public class SweepEntriesNotEnoughRequest
	{
		public string EmailAddress { get; set; }
		public EmailTemplate EmailTemplate { get; set; }
		public string SweepstakeName { get; set; }
	}
}

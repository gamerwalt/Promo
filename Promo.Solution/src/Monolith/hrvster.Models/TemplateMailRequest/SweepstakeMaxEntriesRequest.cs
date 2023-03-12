namespace hrvster.Models.TemplateMailRequest
{
	public class SweepstakeMaxEntriesRequest
	{
		public EmailTemplate EmailTemplate { get; set; }
		public string EmailAddress { get; set; }
		public string SweepstakeName { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
	}
}

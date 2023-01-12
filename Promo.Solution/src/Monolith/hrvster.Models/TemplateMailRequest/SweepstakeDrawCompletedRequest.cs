namespace hrvster.Models.TemplateMailRequest
{
	public class SweepstakeDrawCompletedRequest
	{
		public EmailTemplate EmailTemplate { get; set; }
		public string EmailAddress { get; set; }
		public string SweepstakeName { get; set; }
		public string DisplayName { get; set; }
		public string DashboardLink { get; set; }
		public string StartDate { get; set; }
		public string EndDate { get; set; }
		public string DrawDate { get; set; }
	}
}

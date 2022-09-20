namespace hrvster.Models.TemplateMailRequest
{
	public class EmailAddressUpdateRequest : EmailRequest
	{
		public string OldEmailAddress { get; set; }
		public string NewEmailAddress { get; set; }
		public string SupportLink { get; set; }
	}
}

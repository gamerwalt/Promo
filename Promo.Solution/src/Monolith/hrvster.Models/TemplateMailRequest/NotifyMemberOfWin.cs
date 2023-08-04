namespace hrvster.Models.TemplateMailRequest
{
	public class NotifyMemberOfWin
	{
		public EmailTemplate EmailTemplate { get; set; }
		public string EmailAddress { get; set; }
		public string GameName { get; set; }
		public string GamePlayUid { get; set; }
		public string PrizeName { get; set; }
		public string PrizeValue { get; set; }
	}
}

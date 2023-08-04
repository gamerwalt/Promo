namespace hrvster.Models.TemplateMailRequest
{
	public class NotifyAccountOfuserWinningPick3WinRequest
	{
		public EmailTemplate EmailTemplate { get; set; }
		public string EmailAddress { get; set; }
		public string GameName { get; set; }
		public string GameGuid { get; set; }
		public string ScreenName { get; set; }
		public string PrizeName { get; set; }
	}
}

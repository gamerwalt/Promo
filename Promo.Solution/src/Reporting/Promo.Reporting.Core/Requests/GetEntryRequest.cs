namespace Promo.Reporting.Core.Requests
{
	public class GetEntryRequest
	{
		public string ScreenName { get; set; }
		public string UserIdentifierType { get; set; }
		public string ExternalUserId { get; set; }
	}
}

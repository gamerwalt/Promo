using System;

namespace Promo.Reporting.Core.Requests
{
	public class CreateSweepEntryReport
	{
		public int Id { get; set; }
		public Guid SweepUid { get; set; }
		public Guid CampaignUid { get; set; }
		public string ScreenName { get; set; }
		public string UserIdentifierType { get; set; }
		public string ExternalId { get; set; }
		public string ExternalUserId { get; set; }
		public int Entries { get; set; }
	}
}

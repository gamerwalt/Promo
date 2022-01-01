using System;

namespace Promo.Reporting.Infrastructure.Models
{
	public class SweepEntryReport
	{
		public int Id { get; set; }
		public Guid SweepEntryReportUid { get; set; }
		public Guid SweepUid { get; set; }
		public Guid CampaignUid { get; set; }
		public string ScreenName { get; set; }
		public string UserIdentifierType { get; set; }
		public string ExternalId { get; set; }
		public string ExternalUserId { get; set; }
		public int Entries { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
	}
}

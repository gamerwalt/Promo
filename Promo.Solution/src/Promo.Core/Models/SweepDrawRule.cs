using System;

namespace Promo.Core.Models
{
	public class SweepDrawRule
	{
		public int SweepId { get; set; }
		public Guid SweepUid { get; set; }
		public string SweepName { get; set; }
		public Guid CampaignUid { get; set; }
		public string CampaignName { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime DrawDate { get; set; }
		public Guid SweepDrawRuleUid { get; set; }
		public int WinnerCount { get; set; }
		public int WinnerBackupCount { get; set; }
		public Uri RulesUrl { get; set; }
	}
}

using hrvster.Models;
using System;

namespace hrvster.Web.Models
{
	public class DeleteSweepPrize
	{
		public Sweep Sweep { get; set; }
		public int SweepId { get; set; }
		public string PrizeName { get; set; }
		public Guid PrizeUid { get; set; }
		public Prize Prize { get; set; }
		public decimal MonetaryValue { get; set; }
		public string PrizeDescription { get; set; }
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
		public Guid CampaignUid { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
	}
}

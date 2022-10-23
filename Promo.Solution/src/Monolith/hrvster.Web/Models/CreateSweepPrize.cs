using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateSweepPrize
	{
		public int SweepId { get; set; }
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
		public Guid CampaignUid { get; internal set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public int Position { get; set; }
		public decimal MonetaryValue { get; set; }
		public bool DisableAll { get; internal set; }
		public IEnumerable<Prize> Prizes { get; set; }
	}
}

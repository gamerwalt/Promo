using hrvster.Models;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class SweepPrizes
	{
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
		public int SweepId { get; set; }
		public Guid CampaignUid { get; set; }
		public int WinnerCount { get; set; }
		public List<SweepPrize> Prizes { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
	}
}

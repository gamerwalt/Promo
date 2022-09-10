using System;
using System.Collections.Generic;

namespace hrvster.Models.WebModels.Sweeps
{
	public class SweepPrize
	{
		public string SweepName { get; set; }
		public Guid SweepUid { get; set; }
		public int SweepId { get; set; }
		public Guid CampaignUid { get; set; }
		public int WinnerCount { get; set; }
		public List<Models.SweepPrize> Prizes { get; set; }
		public string ReturnAction { get; set; }
		public string ReturnController { get; set; }
	}
}

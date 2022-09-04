using hrvster.Models.WebModels;
using System;

namespace hrvster.Web.Models
{
	public class UpdateSweepstake
	{
		public Guid SweepUid { get; set; }
		public Guid CampaignUid { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime DrawDate { get; set; }
	}
}

using System;

namespace hrvster.Web.Models
{
	public class MatchImage
	{
		public Guid Uid { get; set; }
		public string LocationUrl { get; set; }
		public string Name { get; set; }
		public Guid CampaignUid { get; set; }
		public string MatchGameName { get; set; }
	}
}

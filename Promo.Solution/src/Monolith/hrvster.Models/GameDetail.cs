using System;

namespace hrvster.Models
{
	public class GameDetail
	{
		public string Name { get; set; }
		public Guid Uid { get; set; }
		public int Id { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Guid CampaignUid { get; set; }
	}
}

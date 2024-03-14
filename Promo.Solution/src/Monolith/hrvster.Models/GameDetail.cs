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
		public DateTime StartDateUtc { get; set; }
		public DateTime EndDateUtc { get; set; }
		public bool IsGenerated { get; set; } = false;

		public bool GameBoardsHasBeenGenerated()
		{
			return this.IsGenerated;
		}
	}
}

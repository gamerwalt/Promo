using System;

namespace hrvster.Models
{
	public class EntryRule
	{
		public int Id { get; set; }
		public Guid EntryRuleUid { get; set; }
		public Guid CampaignUid { get; set; }
		public Guid GameUid { get; set; }
		public int GameId { get; set; }
		public string GameName { get; set; }
		public int PlayCountPerUser { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

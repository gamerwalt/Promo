using System;

namespace hrvster.Web.Models
{
	public class DeleteMatchPrize
	{
		public string Description { get; set; }
		public string Name { get; set; }
		public Guid MatchPrizeUid { get; set; }
		public int Quantity { get; set; }
		public decimal Value { get; set; }
		public string MatchGameName { get; set; }
		public Guid MatchGameUid { get; set; }
		public string ImageLocationUrl { get; set; }
		public string ActionName { get; internal set; }
		public string ControllerName { get; internal set; }
	}
}

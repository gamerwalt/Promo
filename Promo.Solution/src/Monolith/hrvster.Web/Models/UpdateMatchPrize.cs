using System.Collections.Generic;
using System;

namespace hrvster.Web.Models
{
	public class UpdateMatchPrize
	{
		public Guid MatchPrizeUid { get; set; }
		public string MatchGameName { get; set; }
		public Guid MatchGameUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Value { get; set; }
		public int Quantity { get; set; }
		public Guid ImageUid { get; set; }
		public List<MatchImage> MatchImages { get; set; }
		public string ActionName { get; set; }
		public string ControllerName { get; set; }
	}
}

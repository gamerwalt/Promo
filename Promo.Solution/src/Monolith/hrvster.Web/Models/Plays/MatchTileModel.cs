using System;

namespace hrvster.Web.Models.Plays
{
	public class MatchTileModel
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string ImageLocationUrl { get; set; }
		public string PrizeName { get; set; }
	}
}

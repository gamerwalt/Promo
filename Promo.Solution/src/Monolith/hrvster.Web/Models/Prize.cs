using System;

namespace hrvster.Web.Models
{
	public class Prize
	{
		public Guid PrizeUid { get; set; }
		public bool Enabled { get; set; }
		public int PrizePosition { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public decimal PrizeValue { get; set; }
		public string Locale { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

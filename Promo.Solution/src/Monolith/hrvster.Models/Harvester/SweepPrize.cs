using System;

namespace hrvster.Models.Harvester
{
	public class SweepPrize
	{
		public int PrizeId { get; set; }
		public Guid PrizeUid { get; set; }
		public Guid SweepUid { get; set; }
		public bool Enabled { get; set; }
		public int PrizePosition { get; set; }
		public string PrizeName { get; set; }
		public Decimal PrizeValue { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

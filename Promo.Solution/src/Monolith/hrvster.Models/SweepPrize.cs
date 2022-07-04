using System;

namespace hrvster.Models
{
	public class SweepPrize
	{
		public int PrizeId { get; set; }
		public Guid PrizeUid { get; set; }
		public Guid SweepUid { get; set; }
		public bool Enabled { get; set; }
		public int PrizePosition { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public Decimal PrizeValue { get; set; }
		public string Locale { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public Sweep Sweep { get; set; }

		public string GetPrizePositionLiteral()
		{
			if(PrizePosition > 0 && PrizePosition < 4)
			{
				if (PrizePosition == 1) return "1st";
				if (PrizePosition == 2) return "2nd";
				if (PrizePosition == 3) return "3rd";
			}
			
			return $"{PrizePosition}th";
		}
	}
}

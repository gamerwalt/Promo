using System;

namespace Promo.Core.Models
{
	public class SweepPrize
	{
		public Guid PrizeUid { get; set; }
		public Guid SweepUid { get; set; }
		public bool Enabled { get; set; }
		public int PrizePosition { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public Decimal PrizeValue { get; set; }
		public string Locale { get; set; }
	}
}

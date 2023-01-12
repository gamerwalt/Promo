using hrvster.Models;
using System;
using System.Globalization;

namespace TestUtilities.Builders.Resources
{
	public class SweepPrizeBuilder
	{
		public Guid PrizeUid { get; set; }
		public Guid SweepUid { get; set; }
		public int PrizePosition { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public Decimal PrizeValue { get; set; }
		public Sweep Sweep { get; set; }

		public SweepPrizeBuilder(
			Guid prizeUid,
			Guid sweepUid,
			string prizeName,
			string prizeDescription,
			decimal prizeValue,
			int prizePosition)
		{
			PrizeUid = prizeUid;
			SweepUid = sweepUid;
			PrizeName = prizeName;
			PrizeDescription = prizeDescription;
			PrizeValue = prizeValue;
			PrizePosition = prizePosition;
		}

		public SweepPrizeBuilder Create(
			Guid prizeUid,
			Guid sweepUid,
			string prizeName,
			string prizeDescription,
			decimal prizeValue,
			int prizePosition)
			=> new SweepPrizeBuilder(prizeUid, sweepUid, prizeName, prizeDescription, prizeValue, prizePosition);

		public SweepPrize Build()
			=> new SweepPrize
			{
				PrizeUid = PrizeUid,
				SweepUid = SweepUid,
				PrizePosition = PrizePosition,
				PrizeName = PrizeName,
				PrizeDescription = PrizeDescription,
				PrizeValue = PrizeValue,
				Sweep = new Sweep { SweepUid = SweepUid },
				Locale = CultureInfo.CurrentCulture.EnglishName
			};
	}
}

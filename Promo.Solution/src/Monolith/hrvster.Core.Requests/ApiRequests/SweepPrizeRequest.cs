using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.ApiRequests
{
	public class SweepPrizeRequest
	{
		public List<CreateSweepPrize> SweepPrizes { get; set; }
	}

	public class CreateSweepPrize
	{
		public int PrizePosition { get; set; }
		public bool Enabled { get; set; }
		public string PrizeName { get; set; }
		public string PrizeDescription { get; set; }
		public decimal PrizeValue { get; set; }
		public string Locale { get; set; }
	}
}

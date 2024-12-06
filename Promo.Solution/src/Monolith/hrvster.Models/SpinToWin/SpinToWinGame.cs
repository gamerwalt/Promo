using System;

namespace hrvster.Models.SpinToWin
{
	public class SpinToWinGame
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public Campaign Campaign { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int MaximumSpins { get; set; }
		public int MaximumWinsForPhysicalPrizes { get; set; }
		public int MaximumWinsForPointPrizes { get; set; }
		public int PointsForFreePlay { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime PieGenerateDate { get; set; }
		public DateTime StartDateUTC { get; set; }
		public DateTime EndDateUTC { get; set; }
		public DateTime PieGenerateDateUTC { get; set; }
	}
}

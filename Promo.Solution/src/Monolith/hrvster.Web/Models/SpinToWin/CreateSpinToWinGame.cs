using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models.SpinToWin
{
	public class CreateSpinToWinGame
	{
		public IEnumerable<SelectListItem> Promotions { get; set; }
		public Guid PromotionUid { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime PieGenerateDate { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int NumberOfSpins { get; set; }
		public int MaximumWinForPhysicalPrizes { get; set; }
		public int MaximumWinForPointPrizes { get; set; }
		public int PointsForFreePlay { get; set; }
		public bool AcceptTerms { get; set; }
		public bool SpreadMainPrizesPerDay { get; set; }
	}
}

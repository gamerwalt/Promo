using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateMatchGame
	{
		public IEnumerable<SelectListItem> Promotions { get; set; }
		public Guid PromotionUid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int NumberOfBoards { get; set; }
		public int WinnableBoards { get; set; }
		public int WinProbability { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool AcceptTerms { get; set; }
	}
}

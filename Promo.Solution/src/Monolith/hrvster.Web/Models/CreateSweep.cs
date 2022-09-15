using hrvster.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateSweep
	{
		public hrvster.Models.Campaign Campaign { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime DrawDate { get; set; }
		public bool AcceptTerms { get; set; }
	}

	public class CreateNewSweep : CreateSweep
	{
		public IEnumerable<SelectListItem> Promotions { get; internal set; }
	}
}

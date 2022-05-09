using hrvster.Models;
using System;

namespace hrvster.Web.Models
{
	public class CreateSweep
	{
		public Campaign Campaign { get; set; }
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime DrawDate { get; set; }
	}
}

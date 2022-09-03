using hrvster.Models.WebModels;
using System;

namespace hrvster.Web.Models
{
	public class UpdateSweepstake
	{
		public string Name { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}

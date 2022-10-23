using System;

namespace hrvster.Web.Models
{
	public class ViewSweep
	{
		public int SweepId { get; set; }
		public Guid SweepUid { get; set; }
		public string Name { get; set; }
		public string RulesUrl { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime? DrawDate { get; set; }
		public bool Drawn { get; set; }
		public DateTime? DateDrawn { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

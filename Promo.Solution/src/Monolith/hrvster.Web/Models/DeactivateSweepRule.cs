using hrvster.Models.WebModels.Sweeps;
using System;

namespace hrvster.Web.Models
{
	public class DeactivateSweepRule
	{
		public string SweepstakeName { get; set; }
		public Guid SweepstakeUid { get; set; }
		public int SweepstakeId { get; set; }
		public Rule Rule { get; set; }
		public string ReturnAction { get; internal set; }
		public string ReturnController { get; internal set; }
	}
}

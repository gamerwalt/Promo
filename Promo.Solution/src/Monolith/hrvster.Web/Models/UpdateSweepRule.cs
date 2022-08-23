using hrvster.Models;

namespace hrvster.Web.Models
{
	public class UpdateSweepRule
	{
		public WatchTypeCode WatchTypeCode { get; set; }
		public string WatchValue { get; set; }
		public long ExternalId { get; set; }
		public bool ActiveInd { get; set; }
		public bool MustBeFollowing { get; set; }
		public bool NotifyWinners { get; set; }
		public Sweep Sweepstake { get; set; }
		public Rule SweepRule { get; set; }
		public string ControllerName { get; set; }
		public string ActionName { get; set; }
	}
}

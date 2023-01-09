using System;

namespace hrvster.Models
{
	public class SweepNotification
	{
		public Sweep Sweep { get; set; }
		public int SweepNotificationId { get; set; }
		public Guid SweepNotificationUid { get; set; }
		public bool NotEnoughUniqueEntrants { get; set; }
		public bool SweepsGoingLive { get; set; }
		public bool SweepsLive { get; set; }
		public bool DrawHappened { get; set; }
		public bool DrawAttemptHappened { get; set; }
	}
}

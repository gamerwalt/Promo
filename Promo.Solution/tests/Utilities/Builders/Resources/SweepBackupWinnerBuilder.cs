using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
	public class SweepBackupWinnerBuilder
	{
		public int SweepWinnerBackupId { get; set; }
		public int SweepWinnerId { get; set; }
		public Guid SweepWinnerUid { get; set; }
		public Sweep Sweep { get; set; }
		public SweepEntry SweepEntry { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public bool Selected { get; set; }

		public SweepBackupWinnerBuilder(int sweepWinnerId, Guid sweepWinnerUid, Sweep sweep, string userIdentifier)
		{
			SweepWinnerId = sweepWinnerId;
			Sweep = sweep;
			SweepWinnerUid = sweepWinnerUid;
			UserIdentifier = userIdentifier;
			UserIdentifierType = UserIdentifierType.TwitterHandle;
			SweepEntry = new SweepEntry { SweepEntryUid = Guid.NewGuid() };
		}

		public SweepBackupWinnerBuilder Create(int sweepWinnerId, Guid sweepWinnerUid, Sweep sweep, string userIdentifier)
			=> new SweepBackupWinnerBuilder(sweepWinnerId, sweepWinnerUid, sweep, userIdentifier);

		public SweepBackupWinner Build()
			=> new SweepBackupWinner
			{
				SweepWinnerId = SweepWinnerId,
				SweepWinnerUid = SweepWinnerUid,
				Sweep = Sweep,
				SweepEntry = SweepEntry,
				UserIdentifier = UserIdentifier,
				UserIdentifierType = UserIdentifierType,
				Selected = Selected
			};
	}
}

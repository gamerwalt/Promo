using System;
using System.Collections.Generic;

namespace hrvster.Models
{
	public class SweepWinner
	{
		public SweepWinner()
		{
			BackupWinners = new List<SweepBackupWinner>();
		}

		public int SweepWinnerId { get; set; }
		public Guid SweepWinnerUid { get; set; }
		public SweepEntry SweepEntry { get; set; }
		public Sweep Sweep { get; set; }
		public int AssignedId { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public bool FulfilledInd { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public List<SweepBackupWinner> BackupWinners { get; set; }
	}

	public class SweepBackupWinner
	{
		public int SweepWinnerBackupId { get; set; }
		public int SweepWinnerId { get; set; }
		public Sweep Sweep { get; set; }
		public SweepEntry SweepEntry { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public bool Selected { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

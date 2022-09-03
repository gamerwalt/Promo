using System.Collections.Generic;
using System;
using hrvster.Models;

namespace hrvster.Web.Models
{
	public class Winner
	{
		public int SweepWinnerId { get; set; }
		public Guid SweepWinnerUid { get; set; }
		public int AssignedId { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public bool FulfilledInd { get; set; }
		public List<BackupWinner> BackupWinners { get; set; }
	}

	public class BackupWinner
	{
		public int SweepWinnerBackupId { get; set; }
		public int SweepWinnerId { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
	}
}

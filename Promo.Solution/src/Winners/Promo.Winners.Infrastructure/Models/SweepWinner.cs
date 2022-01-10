using Promo.Core;
using System;
using System.Collections.Generic;

namespace Promo.Winners.Infrastructure.Models
{
	public class SweepWinner
	{
		public SweepWinner()
		{
			BackupWinners = new List<SweepBackupWinner>();
		}

		public int SweepWinnerId { get; set; }
		public Guid SweepWinnerUid { get; set; }
		public Guid SweepEntryUid { get; set; }

		public Guid SweepUid { get; set; }
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
		public Guid SweepUid { get; set; }
		public Guid SweepEntryUid { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public bool Selected { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

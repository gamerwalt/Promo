using System;
using System.Collections.Generic;

namespace Promo.Core.Models
{
	public class SweepWinner : Winner
	{
		public SweepWinner()
		{
			BackupWinners = new List<SweepWinner>();
		}

		public int SweepWinnerId { get; set; }
		public int SweepEntryId { get; set; }
		public List<SweepWinner> BackupWinners { get; set; }
		public Guid? SweepEntryUid { get; set; }
		public string UserIdentifier { get; set; }
		public string UserIdentifierType { get; set; }
	}
}

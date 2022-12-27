using hrvster.Models.Harvester;
using System;
using System.Collections.Generic;

namespace hrvster.Models.WebModels.Sweeps
{
	public class Sweepstake
	{
		public Guid SweepUid { get; set; }
		public Promotion Campaign { get; set; }
		public Account Account { get; set; }
		public string Name { get; set; }
		public string RulesUrl { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime StartDateUtc { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime EndDateUtc { get; set; }
		public DateTime? DrawDate { get; set; }
		public DateTime DrawDateUtc { get; set; }
		public bool Drawn { get; set; }
		public DateTime? DateDrawn { get; set; }
		public bool DrawAttempted { get; set; }
		public IEnumerable<SweepEntry> SweepEntries { get; set; }
		public IEnumerable<Harvested> Harvested { get; set; }

		public bool IsActive()
			=> DateTime.UtcNow >= StartDateUtc &&
				DateTime.UtcNow <= EndDateUtc;

		public bool WillBeInProgress()
			=> DateTime.UtcNow <= StartDateUtc;
	}
}

using System;

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
		public DateTime EndDate { get; set; }
		public DateTime? DrawDate { get; set; }
		public bool Drawn { get; set; }
		public DateTime? DateDrawn { get; set; }

		public bool IsActive()
			=> DateTime.UtcNow >= StartDate.ToUniversalTime() &&
				DateTime.UtcNow <= EndDate.ToUniversalTime();

		public bool WillBeInProgress()
			=> DateTime.UtcNow <= StartDate.ToUniversalTime();
	}
}

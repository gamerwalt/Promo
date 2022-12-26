using System;
using System.Collections.Generic;

namespace hrvster.Models
{
    public class Sweep
    {
		public Sweep()
		{
			Rules = new List<Rule>();
		}

        public int SweepId { get; set; }
        public Guid SweepUid { get; set; }
        public Campaign Campaign { get; set; }
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
		public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
		public bool DrawAttempted { get; set; }

		public List<Rule> Rules { get; set; }
	}
}

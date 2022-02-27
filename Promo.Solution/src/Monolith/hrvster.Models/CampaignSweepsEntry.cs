using System;

namespace hrvster.Models
{
    public class CampaignSweepsEntry
    {
        public Guid SweepEntryUid { get; set; }
        public Guid CampaignUid { get; set; }
		public Guid SweepUid { get; set; }
        public string UserIdentifier { get; set; }
        public string UserIdentifierType { get; set; }
        public bool AcceptedTerms { get; set; }
    }
}

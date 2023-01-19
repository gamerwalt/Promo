using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
    public class SweepEntryRuleBuilder
    {
        public Guid EntryRuleUid { get; set; }
        public Guid CampaignUid { get; set; }
        public Guid SweepUid { get; set; }
        public int EntryPerUser { get; set; }

        public SweepEntryRuleBuilder(Guid entryRuleUid, Guid campaignUid, Guid sweepUid, int entryPerUser)
        {
            EntryRuleUid = entryRuleUid;
            CampaignUid = campaignUid;
            SweepUid = sweepUid;
            EntryPerUser = entryPerUser;
        }

        public SweepEntryRuleBuilder Create(Guid entryRuleUid, Campaign campaign, Sweep sweep, int entryPerUser)
            => new SweepEntryRuleBuilder(entryRuleUid, campaign.CampaignUid, sweep.SweepUid, entryPerUser);

        public SweepEntryRule Build()
            => new SweepEntryRule
            {
                EntryRuleUid = EntryRuleUid,
                CampaignUid = CampaignUid,
                SweepUid = SweepUid,
                EntryPerUser = EntryPerUser
            };
    }
}

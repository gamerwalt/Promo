using Bogus;
using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
    public class RuleBuilder
    {
        public Guid RuleUid { get; set; }
        public Campaign Campaign { get; set; }
        public Sweep Sweep { get; set; }
        public WatchTypeCode WatchTypeCode { get; set; }
        public string WatchValue { get; set; }
        public string ExternalId { get; set; }
        public bool ActiveInd { get; set; }
        public bool MustBeFollowing { get; set; }
        public bool NotifyWinners { get; set; }

        public RuleBuilder(Guid ruleUid, Campaign campaign, Sweep sweep, string watchValue) 
        { 
            RuleUid = ruleUid;
            Campaign = campaign;
            Sweep = sweep;
            WatchValue = watchValue;
            ExternalId = new Randomizer().Long().ToString();
            WatchTypeCode = WatchTypeCode.Hashtag;
            ActiveInd = true;
            MustBeFollowing = true;
            NotifyWinners = true;
        }

        public RuleBuilder Create(Guid ruleUid, Campaign campaign, Sweep sweep, string watchValue)
            => new RuleBuilder(ruleUid, campaign, sweep, watchValue);

        public Rule Build()
            => new Rule
            {
                RuleUid = RuleUid,
                Campaign = Campaign,
                Sweep = Sweep,
                WatchValue = WatchValue,
                ExternalId = ExternalId,
                WatchTypeCode = WatchTypeCode,
                ActiveInd = ActiveInd,
                MustBeFollowing = MustBeFollowing,
                NotifyWinners = NotifyWinners,
                ScheduledInd = false
            };
    }
}

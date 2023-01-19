using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
    public class SweepDrawRuleBuilder
    {
        public Guid SweepDrawRuleUid { get; set; }
        public Guid SweepUid { get; set; }
        public int WinnerCount { get; set; }
        public int WinnerBackupCount { get; set; }

        public SweepDrawRuleBuilder(Guid sweepDrawRuleUid, Guid sweepUid, int winnerCount, int winnerBackupCount)
        { 
            SweepDrawRuleUid = sweepDrawRuleUid;
            SweepUid = sweepUid;
            WinnerCount = winnerCount;
            WinnerBackupCount = winnerBackupCount;
        }

        public SweepDrawRuleBuilder Create(Guid sweepDrawRuleUid, Sweep sweep, int winnerCount, int winnerBackupCount)
            => new SweepDrawRuleBuilder(sweepDrawRuleUid, sweep.SweepUid, winnerCount, winnerBackupCount);

        public DrawRule Build()
            => new DrawRule
            {
                SweepDrawRuleUid = SweepDrawRuleUid,
                SweepUid = SweepUid,
                WinnerCount = WinnerCount,
                WinnerBackupCount = WinnerBackupCount
            };
    }
}

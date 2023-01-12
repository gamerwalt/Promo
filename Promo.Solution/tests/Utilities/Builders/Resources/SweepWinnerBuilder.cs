using hrvster.Models;
using System;

namespace TestUtilities.Builders.Resources
{
    public class SweepWinnerBuilder
    {
        public int SweepWinnerId { get; set; }
        public Guid SweepWinnerUid { get; set; }
        public SweepEntry SweepEntry { get; set; }
        public Sweep Sweep { get; set; }
        public int AssignedId { get; set; }
        public string UserIdentifier { get; set; }
        public UserIdentifierType UserIdentifierType { get; set; }
        public bool FulfilledInd { get; set; }

        public SweepWinnerBuilder(Sweep sweep, Guid sweepWinnerUid, string userIdentifier, int assignedId)
        {
            Sweep = sweep;
            SweepWinnerUid = sweepWinnerUid;
            UserIdentifier = userIdentifier;
            AssignedId = assignedId;
        }

        public SweepWinnerBuilder Create(Sweep sweep, Guid sweepWinnerUid, string userIdentifier, int assignedId)
            => new SweepWinnerBuilder(sweep, sweepWinnerUid, userIdentifier, assignedId);

        public SweepWinner Build()
            => new SweepWinner
            {
                Sweep = Sweep,
                UserIdentifierType = UserIdentifierType.TwitterHandle,
                SweepEntry = new SweepEntry { SweepEntryUid = Guid.NewGuid() },
                AssignedId = AssignedId,
                UserIdentifier = UserIdentifier,
                SweepWinnerUid = SweepWinnerUid
            };
    }
}

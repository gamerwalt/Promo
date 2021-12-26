using Promo.Core;
using System;

namespace Promo.Sweeps.Infrastructure.Models
{
    public class SweepEntry
    {
        public int SweepEntryId { get; set; }
        public Guid SweepEntryUid { get; set; }
        public Sweep Sweep { get; set; }
        public string UserIdentifier { get; set; }
        public UserIdentifierType UserIdentifierType { get; set; }
        public bool AcceptedTerms { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

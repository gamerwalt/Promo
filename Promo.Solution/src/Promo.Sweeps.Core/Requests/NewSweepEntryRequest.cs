namespace Promo.Sweeps.Core.Requests
{
    public class NewSweepEntryRequest
    {
        public int EntryCount { get; set; }
        public string UserIdentifier { get; set; }
        public string UserIdentifierType { get; set; }
        public bool AcceptedTerms { get; set; }
    }
}

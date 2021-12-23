using System;

namespace Promo.Core.Exceptions
{
    public class InvalidCampaignException : Exception
    {
        public InvalidCampaignException() { }

        public InvalidCampaignException(string message)
            : base(message) { }
    }
}

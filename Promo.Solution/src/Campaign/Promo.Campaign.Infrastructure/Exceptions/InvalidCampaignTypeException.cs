using System;

namespace Promo.Campaign.Infrastructure.Exceptions
{
    public class InvalidCampaignTypeException : Exception
    {
        public InvalidCampaignTypeException() { }

        public InvalidCampaignTypeException(string message)
            : base(message) { }
    }
}

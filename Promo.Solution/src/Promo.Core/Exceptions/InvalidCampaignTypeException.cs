using System;

namespace Promo.Core.Exceptions
{
    public class InvalidCampaignTypeException : Exception
    {
        public InvalidCampaignTypeException() { }

        public InvalidCampaignTypeException(string message)
            : base(message) { }
    }
}

using System;

namespace Promo.Membership.Infrastructure.Exceptions
{
    public class InvalidCampaignTypeException : Exception
    {
        public InvalidCampaignTypeException() { }

        public InvalidCampaignTypeException(string message)
            : base(message) { }
    }
}

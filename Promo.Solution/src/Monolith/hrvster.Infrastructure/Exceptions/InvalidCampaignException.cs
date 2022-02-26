using System;

namespace hrvster.Infrastructure.Exceptions
{
    public class InvalidCampaignException : Exception
    {
        public InvalidCampaignException() { }

        public InvalidCampaignException(string message)
            : base(message) { }
    }
}

using System;

namespace Promo.Core.Models
{
    public class Account
    {
        public Guid AccountUid { get; set; }
        public string EmailAddress { get; internal set; }
        public string DisplayName { get; internal set; }
    }
}

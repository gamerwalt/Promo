using System;
using System.Collections.Generic;

namespace Promo.Models
{
    public class Email
    {
        public int Id { get; set; }
        public Guid EmailUid { get; set; }
        public string EmailAddress { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public EmailTemplate Template { get; set; }
        public EmailStatus Status { get; set; }
        public bool Sent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}

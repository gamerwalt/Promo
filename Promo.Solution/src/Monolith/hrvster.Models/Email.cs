using System;
using System.Collections.Generic;

namespace hrvster.Models
{
    public class Email
    {
        public Guid EmailUid { get; set; }
        public string EmailAddress { get; set; }
        public EmailStatus Status { get; set; }
        public EmailTemplate Template { get; set; }
        public bool Sent { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

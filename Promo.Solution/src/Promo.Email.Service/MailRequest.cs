using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Promo.Email.Service
{
    public class MailRequest
    {
        public string ToEmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}

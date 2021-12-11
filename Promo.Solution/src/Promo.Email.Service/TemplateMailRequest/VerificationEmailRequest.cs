using Promo.Models;

namespace Promo.Email.Service.TemplateMailRequest
{
    public class VerificationEmailRequest
    {
        public string EmailAddress { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public string VerificationLink { get; set; }
        public string DisplayName { get; set; }

    }
}

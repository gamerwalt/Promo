using Promo.Core;

namespace Promo.Email.Service.TemplateMailRequest
{
    public class WelcomeEmailRequest
    {
        public string EmailAddress { get; set; }
        public EmailTemplate EmailTemplate { get; set; }
        public string DisplayName { get; set; }
        public string DashboardLink { get; set; }
    }
}

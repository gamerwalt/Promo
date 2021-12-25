namespace Promo.Core.Models
{
    public class HrvsterServiceAccount
    {
        public const string SectionName = nameof(HrvsterServiceAccount);
        public string Uid { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}

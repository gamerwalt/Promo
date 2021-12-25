namespace Promo.Core.Authentication.ConfigurationOptions
{
    public class HrvsterServiceGoogleServicesOptions
    {
        public const string HrvsterServicesGoogleService = "HrvsterServicesGoogleService";
        public string ProjectId { get; set; }
        public string FirebaseAppId { get; set; }
        public string WebApiKey { get; set; }
        public string ProjectNumber { get; set; }
    }
}

namespace Promo.Membership.Core.ConfigurationOptions
{
    public class GoogleServicesOptions
    {
        public const string GoogleServices = "GoogleServices";
        public string ProjectId { get; set; }
        public string FirebaseAppId { get; set; }
        public string WebApiKey { get; set; }
        public string ProjectNumber { get; set; }
    }
}

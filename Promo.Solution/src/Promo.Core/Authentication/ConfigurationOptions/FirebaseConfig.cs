using System;

namespace Promo.Core.Authentication.ConfigurationOptions
{
    public class FirebaseConfig
    {
        public const string Firebase = "Firebase";
        public string Type { get; set; }
        public string Project_Id { get; set; }
        public string Private_Key_Id { get; set; }
        public string Private_Key { get; set; }
        public string Client_Email { get; set; }
        public string Client_Id { get; set; }
        public Uri Auth_Uri { get; set; }
        public Uri Token_Uri { get; set; }
        public Uri Auth_Provider_X509_Cert_Url { get; set; }
        public Uri Client_X509_Cert_Url { get; set; }
    }
}

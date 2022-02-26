namespace hrvster.Core.Requests.ApiRequests
{
    public class NewAccountRequest
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string DisplayName { get; set; }
        public string Pin { get; set; }
        public bool UseEmail { get; set; }
        public bool TermsAccepted { get; set; }
    }
}

namespace Promo.Membership.API.Requests
{
    public class AuthenticateRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ReturnSecureToken { get; set; }
    }
}

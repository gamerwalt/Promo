namespace Promo.Membership.API.Response
{
    public class SignInResponse
    {
        public string LocalId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string IdToken { get; set; }
        public bool Registered { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}

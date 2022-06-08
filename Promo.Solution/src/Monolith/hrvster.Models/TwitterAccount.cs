namespace hrvster.Models
{
    public class TwitterAccount
    {
        public const string SectionName = nameof(TwitterAccount);
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string AuthenticationToken { get; set; }
		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
    }
}

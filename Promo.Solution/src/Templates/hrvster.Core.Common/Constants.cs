namespace hrvster.Templates.Core.Common
{
	public class Constants
	{
		public const string ContextItemsSubdomain = "subdomain";
		public const string ContextItemsConnectionString = "connectionString";
	}

	public class TemplateSessionConstants
	{
		public const string GameUid = "Session_Game_Uid";
		public const string ExternalClient = "Session_External_Client";
		public const string RuleUid = "Session_Rule_Uid";
		public const string GameType = "Session_Game_Type";
		public const string UserDetails = "Session_User_Details";
	}

	public class HrvsterClaimTypes
	{
		public const string ConsumerKey = "ConsumerKey";
		public const string ConsumerSecret = "ConsumerSecret";
		public const string AccessToken = "AccessToken";
		public const string AccessTokenSecret = "AccessTokenSecret";
		public const string BearerToken = "BearerToken";
		public const string ExternalProfileId = "ExternalProfileId";
		public const string Description = "Description";
		public const string RefreshToken = "RefreshToken";
		public const string DisplayName = "DisplayName";
	}

	public class MessageConstants
	{
		public const string SweepstakeTweetReply = "I just got an entry into the [GameName] sweepstake.";
	}
}
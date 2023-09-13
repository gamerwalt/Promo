namespace hrvster.Templates.Core.Common
{
	public class Constants
	{
		public const string ContextItemsSubdomain = "subdomain";
		public const string ContextItemsConnectionString = "connectionString";
	}

	public class ContextConstants
	{
		public const string GameUid = "GameUid";
		public const string GameUidV1 = "gameUid";
		public const string GameUidV2 = "Uid";
		public const string GameUidV3 = "uid";
		public const string RuleUid = "ruleUid";
		public const string ExternalClient = "externalClient";
		public const string CampaignUid = "campaignUid";
		public const string AccountUid = "accountUid";
	}

	public class TemplateSessionConstants
	{
		public const string GameUid = "Session_Game_Uid";
		public const string ExternalClient = "Session_External_Client";
		public const string RuleUid = "Session_Rule_Uid";
		public const string GameType = "Session_Game_Type";
		public const string UserDetails = "Session_User_Details";
		public const string Subdomain = "Session_Subdomain";
		public const string AuthenticationKey = "Session_AuthKey";
		public const string RefreshKey = "Session_Auth_Refresh_Key";
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

	public class GameErrorReasons
	{
		public const string GameDoesNotExist = "The game does not exist.";
		public const string TwitterAuthenticationFailed = "Twitter authentication failed.";
		public const string BadRule = "Bad rule.";
		public const string BadExternalClient = "Bad external client.";
		public const string BadGameType = "Bad game type encountered";
		public const string GameCompleted = "The game has completed.";
		public const string GameNotStarted = "The game has not started.";
	}
}
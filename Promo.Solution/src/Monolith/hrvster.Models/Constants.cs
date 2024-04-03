namespace hrvster.Models
{
	public class Constants
	{
		//Providers
		public const string TwitterProvider = "TWITTER";
		public const string FaceBookProvider = "FB";
		public const string InstagramProvider = "INSTAGRAM";
		public const string ClientProvider = "CLIENT";

		//urls
		public const string ScheduledTweetUrl = "https://ads-api.twitter.com/11/accounts/18ce55gm9g0/scheduled_tweets?as_user_id={asUserId}&schduled_at={scheduledAt}";

		public static string GameRulesPath = "GameRules";
	}

	public class GameTypes
	{
		public const string Sweepstake = "Sweepstakes";
		public const string MatchGame = "MatchGames";
	}

	public class UrlTypes
	{
		public const string DashboardLink = "Dashboard";
		public const string MatchGamePlayLink = "PlayMatchGame/matchgame";
		public const string FacebookwebhookLink = "facebookhook/feedhook";
	}

	public class GameUrlTypes
	{
		public const string SweepGame = "Games/Sweeps/[GameUid]";
		public const string MatchGame = "Games/Pick3MatchGame/[GameUid]";
	}

	public class NotificationService
	{
		public const int DismissInSeconds = 10;
	}

	public class CacheKeys
	{
		public const string GameBoards = "game_boards_";
		public const string GameTiles = "game_tiles_";
		public const string BoardKeys = "board_keys";
	}

	public class GameUrlTypeReplaceInformation
	{
		public const string ReplaceableSubdomain = "[subdomain]";
		public const string ReplaceableGameUid = "[GameUid]";
	}
}

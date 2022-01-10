namespace Promo.Clients
{
    public static class ServiceUrls
    {
        //Sweeps
        public static string Sweeps = "v1/sweeps";

		public static string GetCampaignSweep = "v1/sweeps/campaign/{campaignUid}";

        //Campaign Sweeps
        public static string CreateCampaignSweepsEntry = "v1/campaign/{campaignUid}/sweeps";

		//Rules
		public static string Rules = "v1/rules";
        public static string GetRule = "v1/rule/{ruleUid}";
        public static string GetRulesForHashtags = "v1/rules/hashtags";
        public static string GetHashtagRules = "v1/rules/Hashtag";
        public static string UpdateRuleAsAdded = "v1/rule/{ruleUid}/added";
        public static string WatchTypeCampaignTypes = "v1/watchtypes";

		//EntryRule
		public static string GetEntryRule = "v1/campaign/{campaignUid}/sweeps/{sweepUid}/entryrule";

		//DrawRule
		public static string GetSweepDrawRules = "v1/sweeps/drawrules/list";

		//Campaigns
		public static string GetCampaign = "v1/campaigns/{campaignUid}";
		public static string GetCampaignsByUidList = "v1/campaigns/list";

		//Reporting
		public static string CreateReportSweepEntry = "v1/reporting/sweeps";
		public static string GetCampaignSweepUserEntryReport = "v1/reporting/campaign/{campaignUid}/sweeps/{sweepUid}";

		//Winners
		public static string SaveSweepWinners = "v1/winners/sweeps/{sweepUid}/list";

		//Harvester
		public static string GetHarvestedTwitterUser = "v1/users/{userIdentifier}";
	}
}

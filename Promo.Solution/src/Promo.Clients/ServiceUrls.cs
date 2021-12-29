namespace Promo.Clients
{
    public static class ServiceUrls
    {
        //Sweeps
        public static string Sweeps = "v1/sweeps";

        //Campaign Sweeps
        public static string CreateCampaignSweepsEntry = "v1/campaign/{campaignUid:Guid}/sweeps";


        //Rules
        public static string Rules = "v1/rules";
        public static string GetRule = "v1/rule/{ruleUid}";
        public static string GetRulesForHashtags = "v1/rules/hashtags";
        public static string GetHashtagRules = "v1/rules/Hashtag";
        public static string UpdateRuleAsAdded = "v1/rule/{ruleUid}/added";
        public static string WatchTypeCampaignTypes = "v1/watchtypes";

        //Campaigns
        public static string GetCampaign = "v1/campaigns/{campaignUid}";
    }
}

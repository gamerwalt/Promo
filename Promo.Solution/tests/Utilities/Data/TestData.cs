using System;

namespace TestUtilities.Data
{
    public static class TestData
    {
        public const string Fullname = "First Last";
        public const string UserName = "Username";
        public const string Address1 = "Some address 1";
        public const string Address2 = "Some address 2";
        public const string City = "City1";
        public const string State = "FL";
        public const string ZipCode = "95442";
        public const string PasswordHash = "lj0998jkj";
        public const int Role = 1;

        public const string Fullname2 = "First Last2";
        public const string UserName2 = "Username2";
        public const string Address1_2 = "Some address 12";
        public const string Address2_2 = "Some address 22";
        public const string City2 = "City2";
        public const string State2 = "TX";
        public const string ZipCode2 = "95446";
        public const string PasswordHash2 = "lj0998jkj2";

        public const int ClientId = 1;
        public const int FuelId = 100;

        public const string EmailAddress = "success@example.com";
        public const string Password = "Somepassword+2+8@";
        public const string DisplayName = "DisplayName";
        public const string FirstName = "Olawale";
        public const string LastName = "Adewoyin";
        public const string PhoneNumber = "+12048885522";

        public const string WrongEmailAddress = "failure@example.com";
        public const string WrongPassword = "somepassword";

        public const string FirebaseToken = "Token";
        public const int ExpiresIn = 3600;
        public const string RefreshToken = "RefreshToken";
        public const string VerifyToken = "VerifyToken";
        public const string UserUid = "7b681480-0b5f-49a4-ae50-58753e5fa6a8";

        public static Guid AccountUid = Guid.Parse(UserUid);
        public static int AccountId = 1;

		public const string AuthError = "{\r\n  \"error\": {\r\n    \"code\": 400,\r\n    \"message\": \"EMAIL_NOT_FOUND\",\r\n    \"errors\": [\r\n      {\r\n        \"message\": \"EMAIL_NOT_FOUND\",\r\n        \"domain\": \"global\",\r\n        \"reason\": \"invalid\"\r\n      }\r\n    ]\r\n  }\r\n}\r\n";

        public const string WindowsTimezone = "W. Central Africa Standard Time";
        public const string Locale = "Nigeria";

        //Sweeps
        public const int SweepId = 1;
        public static Guid SweepUid = Guid.Parse("938e2599-6fef-45ae-93c3-49bf446658a7");
        public const string SweepName = "Test Sweep 1";

        //Campaign
        public const int CampaignTypeId = 15;
		public static Guid CampaignTypeUid = Guid.Parse("3e5431d6-ba17-41f4-8f21-fd2d70245845");
        public const string CampaignTypeName = "Auto Enter Sweeps From Tweet Reply Hashtag";
        public const string CampaignTypeCode = "AutoEnterSweepsFromTweetReplyHashtag";

        public const int CampaignId = 100;
        public static Guid CampaignUid = Guid.Parse("3e6431d6-ba17-45f4-8f21-fd2d70245845");
		public const string CampaignName = "Test Promotion";
		public const string CampaignDescription = "Test Promotion";

        public const int RuleId = 1;
        public const int RuleId2 = 2;
        public const int RuleId3 = 3;
        public static Guid RuleUid = Guid.Parse("edc71f81-ddcf-4fdb-b615-1aa8c14e35f5");
		public static Guid RuleUid2 = Guid.Parse("edc71f81-ddcf-4fdb-b615-1aa8c14e35f6");
		public static Guid RuleUid3 = Guid.Parse("edc71f81-ddcf-4fdb-b615-1aa8c14e35f7");
        public const string WatchValue = "#WatchValue1";
		public const string WatchValue2 = "#WatchValue2";
		public const string WatchValue3 = "#WatchValue2";
	}
}

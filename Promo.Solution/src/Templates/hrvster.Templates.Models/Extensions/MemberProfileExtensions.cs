using hrvster.Core.Clients;
using hrvster.Templates.Models.Members;

namespace hrvster.Templates.Models.Extensions
{
	public static class MemberProfileExtensions
	{
		public static void UpdateFromUserDetails(this MemberProfile memberProfile, UserProviderDetails userDetails)
		{
			memberProfile.AccessToken = userDetails.AccessToken;
			memberProfile.RefreshToken = userDetails.RefreshToken;
			memberProfile.Name = userDetails.Name;
			memberProfile.ScreenName = userDetails.ScreenName;
			memberProfile.PictureUrl = userDetails.ProfileImage;
		}
	}
}

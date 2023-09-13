using hrvster.Models;
using hrvster.Templates.Core.Common;
using hrvster.Templates.Models.Members;
using System.Security.Claims;

namespace hrvster.Templates.Models.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static MemberProfile ToMemberProfile(this IEnumerable<Claim> claims)
		{
			// Convert claims to MemberProfile object
			var externalProfileId = claims.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalProfileId)?.Value;
			var screenName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
			var displayName = claims.FirstOrDefault(x => x.Type == HrvsterClaimTypes.DisplayName)?.Value;
			var accessToken = claims.FirstOrDefault(x => x.Type == HrvsterClaimTypes.AccessToken)?.Value;
			var refreshToken = claims.FirstOrDefault(x => x.Type == HrvsterClaimTypes.RefreshToken)?.Value;
			var description = claims.FirstOrDefault(x => x.Type == HrvsterClaimTypes.Description)?.Value;

			// Check for missing claims and handle accordingly
			if (string.IsNullOrWhiteSpace(externalProfileId) ||
				string.IsNullOrWhiteSpace(screenName) ||
				string.IsNullOrWhiteSpace(displayName) ||
				string.IsNullOrWhiteSpace(accessToken) ||
				string.IsNullOrWhiteSpace(refreshToken))
			{
				throw new InvalidOperationException("One or more required claims are missing.");
			}

			return new MemberProfile
			{
				ExternalProfileId = externalProfileId,
				ScreenName = screenName,
				Name = displayName,
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				Description = description ?? string.Empty,  // Description can be optional, so we provide a default value
				ProfileType = ProfileType.Twitter
			};
		}
	}
}

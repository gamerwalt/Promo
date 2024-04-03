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
			var externalClient = claims.FirstOrDefault(x => x.Type == HrvsterClaimTypes.ExternalClient)?.Value;
			externalProfileId = externalProfileId ?? claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

			// Check for missing claims and handle accordingly
			if (string.IsNullOrWhiteSpace(externalProfileId))
			{
				throw new InvalidOperationException("The External Profile Claim Is missing");
			}
			if(string.IsNullOrWhiteSpace(screenName))
			{
				throw new InvalidOperationException("The Screen Name Claim Is missing");
			}
			if(string.IsNullOrWhiteSpace(displayName))
			{
				throw new InvalidOperationException("The Display Name Claim Is missing");
			}
			if(string.IsNullOrWhiteSpace(accessToken))
			{
				throw new InvalidOperationException("The Access Token Claim Is missing");
			}
			if(string.IsNullOrWhiteSpace(refreshToken))
			{
				throw new InvalidOperationException("The Refresh Token Claim Is missing");
			}

			return new MemberProfile
			{
				ExternalProfileId = externalProfileId,
				ScreenName = screenName,
				Name = displayName,
				AccessToken = accessToken,
				RefreshToken = refreshToken,
				Description = description ?? string.Empty,  // Description can be optional, so we provide a default value
				ProfileType = ProfileType.Client
			};
		}
	}
}

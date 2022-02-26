using System;
using System.Linq;
using System.Security.Claims;

namespace Promo.Configuration.Extensions
{
	public static class UserClaimsExtensions
	{
		public static Guid GetAccountUidFromCliam(this ClaimsPrincipal claimsPrincipal, string matchinClaimTypeName)
		{
			if (claimsPrincipal == null)
			{
				throw new ArgumentNullException(nameof(claimsPrincipal));
			}

			var value = claimsPrincipal.Claims.FirstOrDefault(c => c.Type.Equals(matchinClaimTypeName, StringComparison.OrdinalIgnoreCase)).Value;

			if (value == null)
			{
				throw new Exception(string.Format("Value from {0} is empty for {1}", nameof(GetAccountUidFromCliam), nameof(matchinClaimTypeName)));
			}

			return Guid.Parse(value);
		}
	}
}

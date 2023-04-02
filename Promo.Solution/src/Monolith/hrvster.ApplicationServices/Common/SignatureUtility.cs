using hrvster.Models.Options;
using System;
using System.Security.Cryptography;
using System.Text;

namespace hrvster.ApplicationServices.Common
{

	public class SignatureUtility
	{
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public SignatureUtility(HarvesterSettingsOptions harvesterSettings)
		{
			_hrvsterSettings = harvesterSettings;
		}

		public string CreateSignature(long userId, string handle, Guid gameUid)
		{
			var message = $"{userId}:{handle}:{gameUid}";

			using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hrvsterSettings.SignatureKey));
			var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

			return Convert.ToBase64String(hash);
		}

		public bool VerifySignature(long userId, string handle, DateTime expiryDate, Guid gameUid, string signature)
		{
			var expectedSignature = CreateSignature(userId, handle, gameUid);

			return expectedSignature == signature;
		}
	}
}

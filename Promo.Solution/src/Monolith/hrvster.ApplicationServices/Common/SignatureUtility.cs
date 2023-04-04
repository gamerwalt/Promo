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

		public string CreateSignature(Guid gameplayUid, long userId, string handle)
		{
			var message = $"{gameplayUid}:{userId}:{handle}";

			using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hrvsterSettings.SignatureKey)))
			{
				var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
				return Base64UrlEncode(hash);
			}
		}

		public bool VerifySignature(Guid gameplayUid, long userId, string handle, string signature)
		{
			var expectedSignature = CreateSignature(gameplayUid, userId, handle);

			return string.Equals(expectedSignature, signature, StringComparison.Ordinal);
		}

		private static string Base64UrlEncode(byte[] input)
		{
			return Convert.ToBase64String(input).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

		private static byte[] Base64UrlDecode(string input)
		{
			string base64 = input.Replace('_', '/').Replace('-', '+');
			while (base64.Length % 4 != 0)
			{
				base64 += "=";
			}
			return Convert.FromBase64String(base64);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace hrvster.Core.Clients
{
	public class OAuthHelper
	{
		private static readonly Random RandomGenerator = new Random();

		public static long GetUnixTimestamp()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		public static string GenerateNonce()
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var nonce = new char[32];
			for (var i = 0; i < nonce.Length; i++)
			{
				nonce[i] = chars[RandomGenerator.Next(chars.Length)];
			}
			return new string(nonce);
		}

		public static string GenerateSignature(string baseString, string consumerSecret, string tokenSecret)
		{
			var key = $"{Uri.EscapeDataString(consumerSecret)}&{Uri.EscapeDataString(tokenSecret)}";
			var encoding = new ASCIIEncoding();
			var keyBytes = encoding.GetBytes(key);
			var baseStringBytes = encoding.GetBytes(baseString);

			using (var hmacsha1 = new HMACSHA1(keyBytes))
			{
				var hashBytes = hmacsha1.ComputeHash(baseStringBytes);
				return Convert.ToBase64String(hashBytes);
			}
		}

		public static string GetSignatureBaseString(string httpMethod, string uri, IDictionary<string, string> parameters)
		{
			var normalizedParameters = NormalizeParameters(parameters);
			var normalizedUri = Uri.EscapeDataString(uri);

			var baseString = new StringBuilder();
			baseString.Append(httpMethod.ToUpperInvariant());
			baseString.Append('&');
			baseString.Append(normalizedUri);
			baseString.Append('&');
			baseString.Append(Uri.EscapeDataString(normalizedParameters));

			return baseString.ToString();
		}

		private static string NormalizeParameters(IDictionary<string, string> parameters)
		{
			var normalizedParameters = new List<string>();

			foreach (var parameter in parameters.OrderBy(p => p.Key))
			{
				var encodedKey = Uri.EscapeDataString(parameter.Key);
				var encodedValue = Uri.EscapeDataString(parameter.Value);
				normalizedParameters.Add($"{encodedKey}={encodedValue}");
			}

			return string.Join("&", normalizedParameters);
		}

		public static string BuildOAuthHeader(IDictionary<string, string> parameters)
		{
			var header = new StringBuilder("OAuth ");

			foreach (var parameter in parameters)
			{
				if (parameter.Key.StartsWith("oauth_"))
				{
					header.Append($"{Uri.EscapeDataString(parameter.Key)}=\"{Uri.EscapeDataString(parameter.Value)}\",");
				}
			}

			// Remove the trailing comma
			if (header[header.Length - 1] == ',')
			{
				header.Length--;
			}

			return header.ToString();
		}
	}
}

using System.Security.Cryptography;

namespace hrvster.Templates.Core.Common
{
	public class CryptoHelper
	{
		public static string EncryptHandshakeKey(string userId, string userName, string base64Key, string base64Iv)
		{
			var plaintext = $"userId={userId}&userName={userName}";
			var key = Convert.FromBase64String(base64Key);
			var iv = Convert.FromBase64String(base64Iv);

			using var aesAlg = Aes.Create();
			aesAlg.Key = key;
			aesAlg.IV = iv;
			aesAlg.Mode = CipherMode.CBC;

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

			using var msEncrypt = new MemoryStream();
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			using (var swEncrypt = new StreamWriter(csEncrypt))
			{
				swEncrypt.Write(plaintext);
			}

			var encrypted = msEncrypt.ToArray();
			return Convert.ToBase64String(encrypted);
		}

		public static string DecryptHandshakeKey(string stringToEncrypt, string base64Key, string base64Iv)
		{
			// Decode the base64 key and IV
			var key = Convert.FromBase64String(base64Key);
			var iv = Convert.FromBase64String(base64Iv);

			using var aesAlg = Aes.Create();
			aesAlg.Key = key;
			aesAlg.IV = iv;
			aesAlg.Mode = CipherMode.CBC;

			var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
			var encryptedBytes = Base64UrlDecode(stringToEncrypt);

			using var msDecrypt = new MemoryStream(encryptedBytes);
			using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			using var srDecrypt = new StreamReader(csDecrypt);
			return srDecrypt.ReadToEnd();
		}

		private static byte[] Base64UrlDecode(string input)
		{
			string output = input;
			output = output.Replace('-', '+'); // 62nd char of encoding
			output = output.Replace('_', '/'); // 63rd char of encoding
			switch (output.Length % 4) // Pad with trailing '='s
			{
				case 0: break; // No pad chars in this case
				case 2: output += "=="; break; // Two pad chars
				case 3: output += "="; break; // One pad char
				default: throw new Exception("Illegal base64url string!");
			}
			var converted = Convert.FromBase64String(output); // Standard base64 decoder
			return converted;
		}
	}
}

using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Helper
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var base64Key = "cYPVqKhzs6tc7oOGOVHOwQEsbl0Ozf/DW2m41qkWwZQ=";
			var base64Iv = "QCe04vD3GLx6qKoL/zMNzA==";

			var userId = "123456789";
			var userName = "Walter";

			var encryptedValue = CryptoHelper.EncryptHandshakeKey(userId, userName, base64Key, base64Iv);
			Console.WriteLine(HttpUtility.UrlEncode(encryptedValue));
			Console.ReadLine();
		}
	}

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
			var encryptedBytes = Convert.FromBase64String(stringToEncrypt);

			using var msDecrypt = new MemoryStream(encryptedBytes);
			using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			using var srDecrypt = new StreamReader(csDecrypt);
			return srDecrypt.ReadToEnd();
		}
	}
}
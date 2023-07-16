using System.Text;

namespace Helper
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var clientId = "OHp1RmFrTjlaRXI4NGJrN0Q0NmY6MTpjaQ";
			var clientSecret = "iwMKDcCy0vbkKlHTPpWkwAIOndFb0yHkmLPoP4Shl8_6mteS8X";
			string clientCredentials = $"{clientId}:{clientSecret}";
			var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientCredentials));

			Console.WriteLine(base64);
		}
	}
}
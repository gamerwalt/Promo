using hrvster.Core.Clients.Twitter.Models;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Clients
{
	public interface ITwitterHttpImageUploaderService
	{
		Task<ImageResponse> UploadImageAsync(byte[] imageData, string externalProfileId);
	}

	public class TwitterHttpImageUploaderService : ITwitterHttpImageUploaderService
	{
		private readonly ILogger<TwitterHttpImageUploaderService> _logger;
		private readonly HttpClient _httpClient;
		private readonly TwitterAccount _twitterAccount;

		private const string ImageUploadEndpoint = "1.1/media/upload.json";

		public TwitterHttpImageUploaderService(
			ILoggerFactory loggerFactory, 
			HttpClient httpClient,
			TwitterAccount twitterAccount)
		{
			_logger = loggerFactory.CreateLogger<TwitterHttpImageUploaderService>();
			_httpClient = httpClient;
			_twitterAccount = twitterAccount;
		}


		public async Task<ImageResponse> UploadImageAsync(byte[] imageData, string externalProfileId)
		{
			var oauthNonce = Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
			TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			var oauthTimestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

			// generate signature base string
			var signatureBaseString = string.Format(
				"POST&{0}&{1}",
				Uri.EscapeDataString("https://upload.twitter.com/1.1/media/upload.json"),
				Uri.EscapeDataString("oauth_consumer_key=" + _twitterAccount.ConsumerKey + "&" +
									 "oauth_nonce=" + oauthNonce + "&" +
									 "oauth_signature_method=HMAC-SHA1" + "&" +
									 "oauth_timestamp=" + oauthTimestamp + "&" +
									 "oauth_token=" + _twitterAccount.AccessToken + "&" +
			"oauth_version=1.0")
			);
			var signingKey = Uri.EscapeDataString(_twitterAccount.ConsumerSecret) + "&" + Uri.EscapeDataString(_twitterAccount.AccessTokenSecret);

			var hasher = new HMACSHA1(new ASCIIEncoding().GetBytes(signingKey));
			var oauthSignature = Convert.ToBase64String(hasher.ComputeHash(new ASCIIEncoding().GetBytes(signatureBaseString)));

			var authorizationHeader = string.Format(
				"OAuth oauth_consumer_key=\"{0}\", oauth_nonce=\"{1}\", oauth_signature=\"{2}\", oauth_signature_method=\"{3}\", oauth_timestamp=\"{4}\", oauth_token=\"{5}\", oauth_version=\"{6}\"",
				Uri.EscapeDataString(_twitterAccount.ConsumerKey),
				Uri.EscapeDataString(oauthNonce),
				Uri.EscapeDataString(oauthSignature),
				Uri.EscapeDataString("HMAC-SHA1"),
				Uri.EscapeDataString(oauthTimestamp),
				Uri.EscapeDataString(_twitterAccount.AccessToken),
				Uri.EscapeDataString("1.0")
			);

			try
			{
				_httpClient.DefaultRequestHeaders.Clear();
				_httpClient.DefaultRequestHeaders.Add("Authorization", authorizationHeader);

				var content = new MultipartFormDataContent();
				content.Add(new ByteArrayContent(imageData), "media");
				var ownersString = string.Join(",", externalProfileId);
				content.Add(new StringContent(ownersString), "additional_owners");

				var responseMessage = await _httpClient.PostAsync(ImageUploadEndpoint, content);

				var response = await responseMessage.Content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<ImageResponse>(response);
			}
			catch (Exception ex)
			{
				_logger.LogWarning("An error occurred while trying to upload the image. Error message: {message}", ex.Message);

				return null;
			}
		}
	}
}

using Facebook;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Clients.Publishers
{
	public class FacebookPublisher : IExternalClient
	{
		private readonly ILogger<FacebookPublisher> _logger;
		private readonly FacebookClient _facebookClient;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public FacebookPublisher(
			ILoggerFactory loggerFactory,
			FacebookClient facebookClient,
			HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<FacebookPublisher>();
			_facebookClient = facebookClient;
			_hrvsterSettings = hrvsterSettings;
		}

		public async Task DeleteAsync(string externalId, string accessToken, string accessTokenSecret)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(accessToken))
					throw new ArgumentNullException(nameof(accessToken));

				_facebookClient.AccessToken = accessToken;

				await _facebookClient.DeleteTaskAsync($"/{externalId}");
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occurred while trying to delete the post with ID {id}.", externalId);
			}
		}

		public async Task<string> PublishAsync(Common.Message message)
		{
			var mediaIds = new List<string>();
			var albumId = string.Empty;

			_facebookClient.AccessToken = message.PageAccessToken;

			if (message.Media != null)
			{
				_logger.LogInformation("We are not publishing any images yet for Facebook.");
			}

			var messageParams = new Dictionary<string, object>()
			{
				{ "message", message.Text },
				{ "appsecret_proof", GenerateAppSecretProof(_facebookClient.AppSecret, message.PageAccessToken) }
			};

			dynamic result = await _facebookClient.PostTaskAsync($"{message.PageId}/feed", messageParams);

			return result["id"];
		}

		public async Task<string> PublishResponse(Common.Message message, string responseToId)
		{
			_facebookClient.AccessToken = message.Profile.AccessToken;

			var messageParams = new Dictionary<string, object>
			{
				["message"] = message.Text,
				["appsecret_proof"] = GenerateAppSecretProof(_facebookClient.AppSecret, message.Profile.AccessToken)
			};

			dynamic result = await _facebookClient.PostTaskAsync($"{responseToId}/comments", messageParams);

			return result.id;
		}

		private static string GenerateAppSecretProof(string appSecret, string accessToken)
		{
			using var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(appSecret));
			byte[] hash = hmac.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}
	}
}

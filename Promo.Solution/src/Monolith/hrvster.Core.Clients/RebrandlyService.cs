using hrvster.Models.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace hrvster.Core.Clients
{
	public interface IRebrandlyService
	{
		Task DeleteLinkAsync(string urlLink);
		Task<string> GenerateShortLinkAsync(string companyGameLink);
	}

	public class RebrandlyService : IRebrandlyService
	{
		private readonly HttpClient _httpClient;
		private readonly HarvesterSettingsOptions _harvesterSettingsOptions;

		public RebrandlyService(
			HttpClient httpClient,
			HarvesterSettingsOptions harvesterSettingsOptions)
		{
			_httpClient = httpClient;
			_harvesterSettingsOptions = harvesterSettingsOptions;

			_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apiKey", _harvesterSettingsOptions.RebrandlyApiKey);
			_httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
		}

		public async Task DeleteLinkAsync(string urlLink)
		{
			var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://api.rebrandly.com/v1/links");

			var responseMessage = await _httpClient.SendAsync(requestMessage);
			responseMessage.EnsureSuccessStatusCode();

			var allLinksData = await responseMessage.Content.ReadAsStringAsync();
			var allLinks = JsonConvert.DeserializeObject<List<RebrandlyLink>>(allLinksData);
			urlLink = urlLink.Replace("https://", "");
			var link = allLinks.FirstOrDefault(l => l.ShortUrl == urlLink);

			if(link != null)
			{
				var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"https://api.rebrandly.com/v1/links/{link.Id}");
				var deleteResponse = await _httpClient.SendAsync(deleteRequest);
				deleteResponse.EnsureSuccessStatusCode();
			}
		}

		public async Task<string> GenerateShortLinkAsync(string companyGameLink)
		{
			var requestContent = new 
			{  
				destination = companyGameLink,
				domain = new
				{
					fullName = "games.hrvster.com"
				}
			};

			var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.rebrandly.com/v1/links");
			requestMessage.Content = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");

			var responseMessage = await _httpClient.SendAsync(requestMessage);
			responseMessage.EnsureSuccessStatusCode();

			var responseContent = await responseMessage.Content.ReadAsStringAsync();
			var response = JsonConvert.DeserializeObject<RebrandlyResponse>(responseContent);

			return response.ShortUrl;
		}

		private class RebrandlyResponse
		{
			[JsonProperty("shortUrl")]
			public string ShortUrl { get; set; }
		}

		private class RebrandlyLink
		{
			public string Id { get; set; }
			public string ShortUrl { get; set; }
		}
	}
}

using Newtonsoft.Json;
using Promo.Core.Authentication;
using Promo.Core.Exceptions;
using Promo.Core.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
	public interface IHarvesterClient
	{
		Task<HarvestedUser> GetUserAsync(string userIdentifier);
		Task CreateNewSweepPrizeAsync(string sweepPrize);
	}

	public class HarvesterClient : IHarvesterClient
	{
		private readonly HttpClient _client;
		private readonly IServiceAuthenticate _serviceAuthenticate;

		public HarvesterClient(HttpClient client, IServiceAuthenticate serviceAuthenticate)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_serviceAuthenticate = serviceAuthenticate ?? throw new ArgumentNullException(nameof(serviceAuthenticate));
		}

		public async Task CreateNewSweepPrizeAsync(string sweepPrize)
		{
			var authModel = await AuthenticateService();

			var sweepPrizeObject = JsonConvert.DeserializeObject<SweepPrize>(sweepPrize);
			
			var result = new HttpResponseMessage();
			var url = ServiceUrls.HarvesterCreateNewSweepPrize.Replace("{sweepUid}", sweepPrizeObject.SweepUid.ToString());
			string content = sweepPrize;

			using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
			{
				requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

				result = await _client.SendAsync(requestMessage);
				if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
			}
		}

		public async Task<HarvestedUser> GetUserAsync(string userIdentifier)
		{
			var authModel = await AuthenticateService();

			var result = new HttpResponseMessage();
			var url = ServiceUrls.GetHarvestedTwitterUser.Replace("{userIdentifier}", userIdentifier.Replace("@", ""));

			using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

			result = await _client.SendAsync(requestMessage);
			if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);

			if (result.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<HarvestedUser>(await result.Content.ReadAsStringAsync());

			return null;
		}

		private async Task<AuthenticationModel> AuthenticateService()
		{
			var authModel = await _serviceAuthenticate.Authenticate();
			if (authModel == null)
				throw new ServiceAuthenticateException();

			return authModel;
		}
	}
}

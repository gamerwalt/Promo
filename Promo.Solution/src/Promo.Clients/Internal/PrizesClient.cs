using Newtonsoft.Json;
using Promo.Core.Authentication;
using Promo.Core.Exceptions;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
	public interface IPrizesClient
	{
		Task<IEnumerable<SweepPrize>> GetAllSweepPrizesAsync(Guid sweepUid);
	}

	public class PrizesClient : IPrizesClient
	{
		private readonly HttpClient _client;
		private readonly IServiceAuthenticate _serviceAuthenticate;

		public PrizesClient(HttpClient client, IServiceAuthenticate serviceAuthenticate)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_serviceAuthenticate = serviceAuthenticate ?? throw new ArgumentNullException(nameof(serviceAuthenticate));
		}

		public async Task<IEnumerable<SweepPrize>> GetAllSweepPrizesAsync(Guid sweepUid)
		{
			var authModel = await AuthenticateService();

			var result = new HttpResponseMessage();
			var url = ServiceUrls.GetSweepPrizes.Replace("{sweepUid}", sweepUid.ToString());

			using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

			result = await _client.SendAsync(requestMessage);
			if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);

			if (result.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<IEnumerable<SweepPrize>>(await result.Content.ReadAsStringAsync());

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

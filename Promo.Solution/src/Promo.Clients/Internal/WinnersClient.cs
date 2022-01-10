using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Core.Authentication;
using Promo.Core.Exceptions;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
	public interface IWinnersClient
	{
		Task<bool> SaveSweepWinnersAsync(IEnumerable<SweepWinner> winners, Guid sweepUid);
	}

	public class WinnersClient : IWinnersClient
	{
		private readonly HttpClient _client;
		private readonly IServiceAuthenticate _serviceAuthenticate;

		public WinnersClient(HttpClient client, IServiceAuthenticate serviceAuthenticate)
		{
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_serviceAuthenticate = serviceAuthenticate ?? throw new ArgumentNullException(nameof(serviceAuthenticate));
		}

		public async Task<bool> SaveSweepWinnersAsync(IEnumerable<SweepWinner> winners, Guid sweepUid)
		{
			var authModel = await AuthenticateService();

			var result = new HttpResponseMessage();
			var content = BuildSweepWinnersContent(winners);
			var url = ServiceUrls.SaveSweepWinners.Replace("{sweepUid}", sweepUid.ToString());

			using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, url))
			{
				requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
				requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

				result = await _client.SendAsync(requestMessage);
				if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
					throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
			}

			return result.IsSuccessStatusCode;
		}

		private string BuildSweepWinnersContent(IEnumerable<SweepWinner> winners)
		{
			return JsonConvert.SerializeObject(
				new
				{
					SweepWinners = winners
				},
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}
			);
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

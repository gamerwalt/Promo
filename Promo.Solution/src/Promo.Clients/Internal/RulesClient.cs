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
    public interface IRulesClient
    {
        Task<IEnumerable<WatchTypeCampaignType>> GetWatchTypeCampaignTypesAsync();
    }

    public class RulesClient : IRulesClient
    {
        private readonly HttpClient _client;
        private readonly IServiceAuthenticate _serviceAuthenticate;

        public RulesClient(HttpClient client, IServiceAuthenticate serviceAuthenticate)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _serviceAuthenticate = serviceAuthenticate ?? throw new ArgumentNullException(nameof(serviceAuthenticate));
        }

        public async Task<IEnumerable<WatchTypeCampaignType>> GetWatchTypeCampaignTypesAsync()
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ServiceUrls.WatchTypeCampaignTypes}"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<WatchTypeCampaignType>>(await result.Content.ReadAsStringAsync());
                }
            }

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

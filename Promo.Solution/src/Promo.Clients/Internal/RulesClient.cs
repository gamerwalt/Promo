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
    public interface IRulesClient
    {
        Task<IEnumerable<WatchTypeCampaignType>> GetWatchTypeCampaignTypesAsync();
        Task CreateCampaignRulesAsync(List<Rule> rules);
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

        public async Task CreateCampaignRulesAsync(List<Rule> rules)
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();
            string content = GetContent(rules);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ServiceUrls.Rules}"))
            {
                requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
            }
        }

        private string GetContent(List<Rule> rules)
        {
            return JsonConvert.SerializeObject(rules, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
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

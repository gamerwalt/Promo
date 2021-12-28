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
        Task CreateCampaignRulesAsync(IEnumerable<Rule> rules);
        Task<IEnumerable<Rule>> GetRulesNeededToBeWatchedAsync();
        Task UpdateRuleAsAddedAsync(Guid ruleUid);
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

        public async Task CreateCampaignRulesAsync(IEnumerable<Rule> rules)
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();
            string content = GetContentFromRules(rules);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ServiceUrls.Rules}"))
            {
                requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
            }
        }

        private string GetContentFromRules(IEnumerable<Rule> rules)
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

        public async Task<IEnumerable<Rule>> GetRulesNeededToBeWatchedAsync()
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"{ServiceUrls.UnaddedRules}"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<Rule>>(await result.Content.ReadAsStringAsync());
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

        public async Task UpdateRuleAsAddedAsync(Guid ruleUid)
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();
            string content = JsonConvert.SerializeObject(
                new { RuleUid = ruleUid }, 
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            );

            var url = ServiceUrls.UpdateRuleAsAdded.Replace("{ruleUid}", ruleUid.ToString());

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Patch, $"{url}"))
            {
                requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
            }
        }
    }
}

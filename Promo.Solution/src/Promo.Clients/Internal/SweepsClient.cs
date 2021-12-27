using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Core.Authentication;
using Promo.Core.Exceptions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface ISweepsClient
    {
        Task<bool> CreateSweepsForCampaignAsync(string campaignName, Guid campaignUid, DateTime startDate, DateTime endDate);
    }

    public class SweepsClient : ISweepsClient
    {
        private readonly HttpClient _client;
        private readonly IServiceAuthenticate _serviceAuthenticate;

        public SweepsClient(HttpClient client, IServiceAuthenticate serviceAuthenticate)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _serviceAuthenticate = serviceAuthenticate ?? throw new ArgumentNullException(nameof(serviceAuthenticate));
        }

        public async Task<bool> CreateSweepsForCampaignAsync(string campaignName, Guid campaignUid, DateTime startDate, DateTime endDate)
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();
            string content = GetContent(campaignName, campaignUid, startDate, endDate);

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{ServiceUrls.Sweeps}"))
            {
                requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

                result = await _client.SendAsync(requestMessage);
                if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
            }

            return result.IsSuccessStatusCode;
        }

        private async Task<AuthenticationModel> AuthenticateService()
        {
            var authModel = await _serviceAuthenticate.Authenticate();
            if (authModel == null)
                throw new ServiceAuthenticateException();
            return authModel;
        }

        private static string GetContent(string campaignName, Guid campaignUid, DateTime startDate, DateTime endDate)
        {
            return JsonConvert.SerializeObject(new
            {
                Name = campaignName,
                CampaignUid = campaignUid,
                StartDate = startDate,
                EndDate = endDate,
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}

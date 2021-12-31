using Microsoft.Extensions.Configuration;
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
    public interface ISweepsClient
    {
        Task<bool> CreateSweepsForCampaignAsync(string campaignName, Guid campaignUid, DateTime startDate, DateTime endDate);
        Task CreateSweepsEntryAsync(int entryCount, Guid campaignUid, string screenName, string userIdentifierType, bool acceptedTerms);
		Task<IEnumerable<Sweep>> GetCampaignSweepsAsync(Guid campaignUid);
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

        public async Task CreateSweepsEntryAsync(int entryCount, Guid campaignUid, string screenName, string userIdentifierType, bool acceptedTerms)
        {
            var authModel = await AuthenticateService();

            var result = new HttpResponseMessage();
            var content = GetSweepEntryContent(entryCount, campaignUid, screenName, userIdentifierType, acceptedTerms);
            var url = ServiceUrls.CreateCampaignSweepsEntry.Replace("{campaignUid}", campaignUid.ToString());

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

            result = await _client.SendAsync(requestMessage);
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);
        }

		public async Task<IEnumerable<Sweep>> GetCampaignSweepsAsync(Guid campaignUid)
		{
			var authModel = await AuthenticateService();

			var result = new HttpResponseMessage();
			var url = ServiceUrls.GetCampaignSweep.Replace("{campaignUid}", campaignUid.ToString());

			using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
			requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authModel.IdToken);

			result = await _client.SendAsync(requestMessage);
			if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
				throw new Exception(ExceptionMessages.UnauthroizedAccessToAPI);

			if(result.IsSuccessStatusCode)
				return JsonConvert.DeserializeObject<IEnumerable<Sweep>>(await result.Content.ReadAsStringAsync());

			return null;
		}

		private static string GetSweepEntryContent(int entryCount, Guid campaignUid, string userIdentifier, string userIdentifierType, bool acceptedTerms)
        {
            return JsonConvert.SerializeObject(
                new
                {
                    entryCount,
                    campaignUid,
                    userIdentifier,
                    userIdentifierType,
                    acceptedTerms
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

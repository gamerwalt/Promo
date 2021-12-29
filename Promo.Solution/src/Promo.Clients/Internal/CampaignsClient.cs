using Newtonsoft.Json;
using Promo.Core.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface ICampaignsClient
    {
        Task<Campaign> GetCampaignAsync(Guid campaignUid);
    }

    public class CampaignsClient : ICampaignsClient
    {
        private readonly HttpClient _client;

        public CampaignsClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<Campaign> GetCampaignAsync(Guid campaignUid)
        {
            var result = new HttpResponseMessage();

            var url = ServiceUrls.GetCampaign.Replace("{campaignUid}", campaignUid.ToString());

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                result = await _client.SendAsync(requestMessage);

                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<Campaign>(await result.Content.ReadAsStringAsync());
                }
            }

            return null;
        }
    }
}

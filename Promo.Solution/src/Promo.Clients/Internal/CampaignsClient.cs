using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface ICampaignsClient
    {
        Task<Campaign> GetCampaignAsync(Guid campaignUid);
		Task<IEnumerable<Campaign>> GetCampaignsAsync(IEnumerable<Guid> campaignGuidList);
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

		public async Task<IEnumerable<Campaign>> GetCampaignsAsync(IEnumerable<Guid> campaignGuidList)
		{
			var result = new HttpResponseMessage();

			var content = JsonConvert.SerializeObject(new { campaignUids = campaignGuidList.ToList() }, 
				new JsonSerializerSettings
				{
					ContractResolver = new CamelCasePropertyNamesContractResolver()
				}
			);
			var url = ServiceUrls.GetCampaignsByUidList;

			using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
			{
				requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");
				result = await _client.SendAsync(requestMessage);

				if (result.IsSuccessStatusCode)
				{
					return JsonConvert.DeserializeObject<IEnumerable<Campaign>>(await result.Content.ReadAsStringAsync());
				}
			}

			return null;
		}
	}
}

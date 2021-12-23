using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Models.Campaigns;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Clients.Internal
{
    public interface ISweepsClient
    {
        Task CreateSweepsForCampaignAsync(Campaign campaign);
    }

    public class SweepsClient : ISweepsClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public SweepsClient(HttpClient client, IConfiguration config)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task CreateSweepsForCampaignAsync(Campaign campaign)
        {
            var content = JsonConvert.SerializeObject(campaign, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            var result = await _client.PostAsync("v1/sweeps/rules", new StringContent(content, Encoding.UTF8, "application/json"));
        }
    }
}

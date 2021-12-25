using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Promo.Core.Authentication.ConfigurationOptions;
using Promo.Core.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Core.Authentication
{
    public interface IServiceAuthenticate
    {
        Task<AuthenticationModel> Authenticate();
    }

    public class ServiceAuthenticate : IServiceAuthenticate
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private readonly HrvsterServiceAccount _hrvsterServiceAccount;

        public ServiceAuthenticate(HttpClient client, IConfiguration config, HrvsterServiceAccount hrvsterServiceAccount)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _hrvsterServiceAccount = hrvsterServiceAccount ?? throw new ArgumentNullException(nameof(hrvsterServiceAccount));
        }

        public async Task<AuthenticationModel> Authenticate()
        {
            var content = JsonConvert.SerializeObject(new AuthenticateRequest
            {
                Email = _hrvsterServiceAccount.Email,
                Password =_hrvsterServiceAccount.Password,
                ReturnSecureToken = true
            }, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });;
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            var url = $"/v1/accounts:signInWithPassword?key={_config["HrvsterServicesGoogleService:WebApiKey"]}";

            var result = await _client.PostAsync(url, stringContent);
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<AuthenticationModel>(await result.Content.ReadAsStringAsync());
            }

            return null;
        }
    }
}

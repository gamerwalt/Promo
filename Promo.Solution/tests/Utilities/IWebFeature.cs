using hrvster.Infrastructure;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace TestUtilities
{
    public interface IWebFeature<TFeature> where TFeature : class
    {
        public HttpResponseMessage Response { get; set; }
        public HttpClient Client { get; set; }
        public IConfiguration Configuration {get; set;}
        public IAccountsRepository AccountsRepository { get; set; }
        public ICampaignRepository CampaignRepository { get; set; }

        Guid UserUid { get; set; }

        void DeleteAllTableData();
    }
}

using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using TestUtilities;
using TestUtilities.Framework;

namespace hrvster.Web.Mvc.Common
{
    public class Feature : FeatureBase<Feature>, IWebFeature<Feature>
    {
        public TestServer TestServer { get; set; }
        public HttpClient Client { get; set; }
        public HttpResponseMessage Response { get; set; }

        //Add Mock Repositories and Services that will be used per Feature here

        public Feature()
            => Initialize();

        public override Feature Initialize(
            Action<IConfiguration> additionalConfiguration = null, 
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            base.Initialize(
                additionalConfiguration,
                services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

            try
            {
                TestServer = TestServerFactory.CreateTestServer(additionalConfiguration, services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

                //Client.BaseAddress = new Uri("https://localhost:5001");
                Client = TestServer.CreateClient();
            }
            catch (Exception ex)
            {
                throw;
            }

            RegisterMockedTestServices();

            return this;
        }

        public virtual void RegisterMockedTestServices()
        {

        }

        protected virtual void ApplyAdditionalServiceConfiguration(
            IServiceCollection services,
            Action<IServiceCollection> additionalServiceConfiguration)
        {
            AdditionalServices?.Invoke(services);
            additionalServiceConfiguration?.Invoke(services);
        }

        protected override Action<IServiceCollection> AdditionalServices
            => services =>
            {
                //services.AddSingleton<>();
            };

        public override void Dispose()
        {
            TestServer?.Dispose();

            DeleteAllTableData();

            base.Dispose();
        }

        public void DeleteAllTableData()
        {
            //Delete from all Test table
        }
    }
}
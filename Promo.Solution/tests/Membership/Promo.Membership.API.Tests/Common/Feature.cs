using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promo.TestUtilities;
using Promo.TestUtilities.Common;
using Promo.TestUtilities.Database;
using Promo.TestUtilities.Framework;
using Promo.TestUtilities.Logging;
using System;
using System.Net.Http;

namespace Promo.Membership.API.Tests.Common
{
    public class Feature : StorageTestFixtureBase<Feature>, IApiFeature<Feature>
    {
        public TestServer TestServer { get; set; }
        public HttpClient Client { get; set; }
        public HttpResponseMessage Response { get; set; }

        public Feature()
        {
            Initialize();
        }

        public override Feature Initialize(
            string testingApi = null,
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            base.Initialize(testingApi, additionalConfiguration, services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

            if (testingApi == null) testingApi = TestingAPI.Membership;

            switch(testingApi)
            {
                case TestingAPI.Membership:
                    TestServer = TestServerFactory.CreateMembershipTestServer(Logger, additionalConfiguration, services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));
                    break;
            }
            
            Client = TestServer.CreateClient();

            SetServices();

            return this;
        }

        protected virtual void ApplyAdditionalServiceConfiguration(IServiceCollection services, Action<IServiceCollection> additionalServiceConfiguration)
        {
            AdditionalServices?.Invoke(services);

            additionalServiceConfiguration?.Invoke(services);
        }

        public override void Dispose()
        {
            Client?.Dispose();
            TestServer?.Dispose();

            base.Dispose();
        }
    }
}

using Firebase.Auth;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using TestUtilities;
using TestUtilities.Framework;
using TestUtilities.Mocks.Authentication;
using TestUtilities.Mocks.Repositories;

namespace hrvster.Web.Mvc.Common
{
    public class Feature : StorageFeatureBase<Feature>, IWebFeature<Feature>
    {
        public TestServer TestServer { get; set; }
        public HttpClient Client { get; set; }
        public HttpResponseMessage Response { get; set; }
        public string AntiForgeryFieldValue { get; set; }
        public string AntiForgeryToken { get; set; }
        public string AntiForgeryCookie { get; set; }

        //Add Mock Repositories and Services that will be used per Feature here

        public Mock<IFirebaseAuthProvider> MockedFirebaseAuthProvider { get; private set; }

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
                TestServer = TestServerFactory.CreateTestServer(
                    additionalConfiguration, 
                    services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

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
            MockedFirebaseAuthProvider = TestServer.Services.GetService<Mock<IFirebaseAuthProvider>>();
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
                services.MockFirebaseAuthProvider()
                    .MockProfileRepository();
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
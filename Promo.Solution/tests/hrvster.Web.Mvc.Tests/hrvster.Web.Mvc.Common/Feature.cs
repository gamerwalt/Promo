using Firebase.Auth;
using hrvster.Infrastructure;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MySqlConnector;
using NToastNotify;
using System.Data;
using TestUtilities;
using TestUtilities.Framework;
using TestUtilities.Mocks.Authentication;
using TestUtilities.Mocks.Notifications;
using TestUtilities.Mocks.Repositories;

namespace hrvster.Web.Mvc.Common
{
    public class Feature : StorageFeatureBase<Feature>, IWebFeature<Feature>
    {
        public TestServer TestServer { get; set; }
        public HttpClient Client { get; set; }
        public HttpResponseMessage Response { get; set; }
        public IConfiguration Configuration { get; set; }
        public IAccountsRepository AccountsRepository { get; set; }
        public ICampaignRepository CampaignRepository { get; set; }
        public Guid UserUid { get; set; }

        public string AntiForgeryFieldValue { get; set; }
        public string AntiForgeryToken { get; set; }
        public string AntiForgeryCookie { get; set; }

        //Add Mock Repositories and Services that will be used per Feature here

        public Mock<IFirebaseAuthProvider> MockedFirebaseAuthProvider { get; private set; }
        public Mock<IToastNotification> MockedToastNotification { get; private set; }

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
            SetServices();

            return this;
        }

        private void SetServices()
        {
            Configuration = TestServer.Services.GetRequiredService<IConfiguration>();
            AccountsRepository = TestServer.Services.GetRequiredService<IAccountsRepository>();
            CampaignRepository = TestServer.Services.GetRequiredService<ICampaignRepository>();
        }

        public virtual void RegisterMockedTestServices()
        {
            MockedFirebaseAuthProvider = TestServer.Services.GetRequiredService<Mock<IFirebaseAuthProvider>>();
            MockedToastNotification = TestServer.Services.GetRequiredService<Mock<IToastNotification>>();
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
                    .MockFirebaseAuthManager()
                    .MockSessionManager()
                    .MockToastNotification()
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
            base.DeleteAllTestData();
        }

        public Feature AndUpdateTheAccountUid(string userUid)
        {
            UserUid = Guid.Parse(userUid);

            var hrvsterConnectionString = Configuration["ConnectionStrings:hrvster"];
            using var conn = new MySqlConnection(hrvsterConnectionString);
            var command = conn.CreateCommand();
            command.CommandText = $"UPDATE accounts SET account_uid = '{UserUid}'";
            command.CommandType = CommandType.Text;
            conn.Open();
            command.ExecuteNonQuery();

            return this;
        }
    }
}
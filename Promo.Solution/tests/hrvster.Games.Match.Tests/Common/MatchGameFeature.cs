using hrvster.Core.Clients.Publishers;
using hrvster.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using TestUtilities;
using TestUtilities.Mocks.Publishers;

namespace hrvster.Games.Match.Tests.Common
{
    public class MatchGameFeature : StorageFeatureBase<MatchGameFeature>, IFeature<MatchGameFeature>
    {
        public IConfiguration Configuration { get; private set; }
        public MatchEngine MatchEngine { get; private set; }

        public MatchGameFeature()
        {
            Initialize();
        }

        public override MatchGameFeature Initialize(
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            base.Initialize(
                additionalConfiguration,
                services => ApplyAdditionalServiceConfiguration(services, additionalServiceConfiguration));

            Configuration = base.Configuration;

            RegisterMockedTestServices();
            SetServices();

            return this;
        }

        private void RegisterMockedTestServices()
        {

        }

        public void SetServices()
        {
            MatchEngine = ServiceProvider.GetService<MatchEngine>();
        }

        protected virtual void ApplyAdditionalServiceConfiguration(
            IServiceCollection services,
            Action<IServiceCollection> additionalServiceConfiguration)
        {
            if (ServiceProvider != null)
            {
                var loggerFactory = ServiceProvider.GetRequiredService<ILoggerFactory>();
                var twitterAccount = ServiceProvider.GetRequiredService<TwitterAccount>();

                services.MockTwitterClient(loggerFactory, twitterAccount);

                var m = new Mock<Func<ExternalClientType, IExternalClient>>();
                m.Setup(c => c.Invoke(It.IsAny<ExternalClientType>()))
                    .Returns(ServiceProvider.GetRequiredService<TwitterClient>());
            }

            AdditionalServices?.Invoke(services);
            additionalServiceConfiguration?.Invoke(services);
        }

        protected override Action<IServiceCollection> AdditionalServices
            => services =>
            {
                //services.AddSingletons();
            };

        public override void Dispose()
        {
            DeleteAllTableData();

            base.Dispose();
        }

        public void DeleteAllTableData()
        {
            base.DeleteAllTestData();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Promo.Core;
using Promo.TestUtilities.Extensions;
using Promo.TestUtilities.Logging;
using System;

namespace Promo.TestUtilities.Common
{
    public abstract class FeatureBase<TFeature> : IDisposable where TFeature : class
    {
        public TestLogger Logger { get; } = new TestLogger();
        public IConfiguration Configuration { get; private set; }
        public IServiceProvider ServiceProvider { get; private set; }

        protected IDatabase Database { get; private set; }

        protected virtual Action<IServiceCollection> AdditionalServices { get; } = null;
        public virtual TFeature Initialize(
            string testingApi = TestingAPI.Membership,
            Action<IConfiguration> additionalConfiguration = null,
            Action<IServiceCollection> additionalServiceConfiguration = null)
        {
            Configuration = TestConfig.GetConfiguration();
            additionalConfiguration?.Invoke(Configuration);
            var serviceCollection = new ServiceCollection()
                .AddTestLogging(Configuration, testLogger: Logger);

            ConfigureServices?.Invoke(serviceCollection);

            additionalServiceConfiguration?.Invoke(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            return this as TFeature;
        }

        protected virtual Action<IServiceCollection> ConfigureServices
            => services =>
            {
                services.AddDatabaseForTesting(Configuration);

                AdditionalServices?.Invoke(services);
            };

        protected virtual void SetServices()
            => Database = ServiceProvider.GetService<IDatabase>();

        public virtual void Dispose() => Database.DeleteTestData();
    }
}

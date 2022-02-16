using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Promo.Configuration.Extensions;
using Promo.Core;
using Promo.TestUtilities.Common;
using Promo.TestUtilities.Logging;
using System;

namespace Promo.TestUtilities.Database
{
    public abstract class StorageTestFixtureBase<TFeature> : FeatureBase<TFeature>, IStorageTestFixtureBase<TFeature> where TFeature : class
    {
        protected IDatabase Database { get; private set; }

        public StorageTestFixtureBase()
        {
            Initialize();
        }

        protected override Action<IServiceCollection> ConfigureServices =>
            services =>
            {
                services.AddSingleton<ILogger, TestLogger>();
                services.AddAccountRepositories();
                //services.AddReportingRepositories();
                //services.AddNotificationRepositories();
                //services.AddSweepsRepositories();
                //services.AddFulfillmentRepositories();
                //services.AddTwitterHarvesterRepositories();

                AdditionalServices?.Invoke(services);
            };

        protected override void SetServices()
        {
            Database = ServiceProvider.GetService<IDatabase>();
        }

        protected virtual Action<IServiceCollection> AdditionalServices { get; } = null;

        public void DeleteTestTableData()
        {
            Database.DeleteTestData();
        }

        public override void Dispose()
        {
            Database.DeleteTestData();
        }
    }
}

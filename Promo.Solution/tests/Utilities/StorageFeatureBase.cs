using hrvster.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TestUtilities
{
    public abstract class StorageFeatureBase<TFeature> : FeatureBase<TFeature> where TFeature : class
    {
        public StorageFeatureBase() { Initialize(); }

        protected override Action<IServiceCollection> ConfigureServices =>
            services =>
            {
                //add the services here
                services.AddFirebaseServices(Configuration);
            };

        protected virtual Action<IServiceCollection> AdditionalServices { get; } = null;

    }
}

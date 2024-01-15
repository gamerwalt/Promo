using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Web.TestUtilities
{
    public abstract class StorageFeatureBase<TFeature> : FeatureBase<TFeature> where TFeature : class
    {
        protected StorageFeatureBase() { Initialize(); }
        protected override Action<IServiceCollection> ConfigureServices =>
            services =>
            {
                //Do mocks here
                //services.MockFirebaseAuthManager()
                //    .MockExternalClient()
                //    .MockPaymentAdaptor()
                //    .MockToastNotification()
                //    .MockAuthenticiation();
            };
        protected virtual Action<IServiceCollection> AdditionalServices { get; } = null;

        public void DeleteAllTableData()
        {

        }

        public override void Dispose()
        {
            DeleteAllTableData();

            base.Dispose();
        }
    }
}

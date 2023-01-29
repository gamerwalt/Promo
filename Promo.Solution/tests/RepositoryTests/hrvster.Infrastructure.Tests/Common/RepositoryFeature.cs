using hrvster.ApplicationServices.Subscription;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Subscriptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities;

namespace hrvster.Infrastructure.Tests.Common
{
	public class RepositoryFeature : StorageFeatureBase<RepositoryFeature>, IFeature<RepositoryFeature>
	{
		public IConfiguration Configuration { get; private set; }
		public ISweepsRepository SweepsRepository { get; private set; }
		public ISweepNotificationRepository SweepNotificationRepository { get; private set; }
		public IAccountsRepository AccountsRepository { get; private set; }
		public ICampaignRepository CampaignRepository { get; private set; }
		public IPlansRepository PlansRepository { get; private set; }
		public IMatchGamesRepository MatchGameRepository { get; private set; }



        public ISubscriptionService SubscriptionService { get; private set; }


        public RepositoryFeature()
		{
			Initialize();
		}

		public override RepositoryFeature Initialize(
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
			SweepsRepository = ServiceProvider.GetService<ISweepsRepository>();
			AccountsRepository = ServiceProvider.GetService<IAccountsRepository>();
			CampaignRepository = ServiceProvider.GetService<ICampaignRepository>();
			SweepNotificationRepository = ServiceProvider.GetService<ISweepNotificationRepository>();
			PlansRepository = ServiceProvider.GetService<IPlansRepository>();
			AccountsRepository = ServiceProvider.GetService<IAccountsRepository>();
			MatchGameRepository = ServiceProvider.GetService<IMatchGamesRepository>();

            SubscriptionService = ServiceProvider.GetService<ISubscriptionService>();
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

using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Subscription;
using hrvster.Infrastructure;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.ApplicationServices.Tests.Common
{
    public class MatchGameFeature : StorageFeatureBase<MatchGameFeature>, IFeature<MatchGameFeature>
    {
        public IMediator Mediator { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IMatchGamesRepository MatchGamesRepository { get; private set; }
        public IAccountsRepository AccountsRepository { get; private set; }
        public ICampaignRepository CampaignRepository { get; private set; }
        public IPlansRepository PlansRepository { get; private set; }
        public IMatchImagesRepository MatchImageRepository { get; private set; }
        public IMatchPrizesRepository MatchPrizesRepository { get; private set; }
        public IMatchBoardsRepository MatchBoardsRepository { get; private set; }


		public IMatchGamesService MatchGamesService { get; private set; }
        public IMatchGameReadinessService MatchGameReadinessService { get; private set; }
		public ISubscriptionService SubscriptionService { get; private set; }

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
            Mediator = ServiceProvider.GetService<IMediator>();
            MatchGamesRepository = ServiceProvider.GetService<IMatchGamesRepository>();
            AccountsRepository = ServiceProvider.GetService<IAccountsRepository>();
            CampaignRepository = ServiceProvider.GetService<ICampaignRepository>();
            PlansRepository = ServiceProvider.GetService<IPlansRepository>();
            MatchImageRepository = ServiceProvider.GetService<IMatchImagesRepository>();
            MatchPrizesRepository = ServiceProvider.GetService<IMatchPrizesRepository>();
            MatchBoardsRepository = ServiceProvider.GetService<IMatchBoardsRepository>();

            MatchGamesService = ServiceProvider.GetService<IMatchGamesService>();
            MatchGameReadinessService = ServiceProvider.GetService<IMatchGameReadinessService>();
            SubscriptionService = ServiceProvider.GetService<ISubscriptionService>();

            var hrvsterUser = ServiceProvider.GetService<HrvsterUser>();
            hrvsterUser.EmailAddress = TestData.EmailAddress;
            hrvsterUser.UserUid = TestData.AccountUid;
            hrvsterUser.DisplayName = TestData.DisplayName;
            hrvsterUser.Locale = TestData.Locale;
            hrvsterUser.WindowsTimeZone = TestData.WindowsTimezone;
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

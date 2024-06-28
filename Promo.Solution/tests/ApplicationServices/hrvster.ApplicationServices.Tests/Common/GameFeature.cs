using hrvster.ApplicationServices.MatchGames;
using hrvster.ApplicationServices.Payments;
using hrvster.ApplicationServices.Subscription;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Clients.Publishers;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Core;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.MatchGames;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NToastNotify;
using TestUtilities;
using TestUtilities.Data;
using TestUtilities.Mocks.Clients;
using TestUtilities.Mocks.Publishers;

namespace hrvster.ApplicationServices.Tests.Common
{
    public class GameFeature : StorageFeatureBase<GameFeature>, IFeature<GameFeature>
    {
        public IMediator Mediator { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public ISweepsRepository SweepsRepository { get; private set; }
        public ISweepsEntriesRepository SweepEntryRepository { get; private set; }
        public ISweepRulesRepository SweepRulesRepository { get; private set; }

        public ISweepstakesService SweepstakesService { get; private set; }
        public IAccountsRepository AccountsRepository { get; private set; }
        public IProfilesRepository ProfilesRepository { get; private set; }
        public ICampaignRepository CampaignRepository { get; private set; }
        public IPlansRepository PlansRepository { get; private set; }
        public IMatchGameRulesRepository MatchGameRulesRepository { get; private set; }
        public IBlacklistedUserRepository BlacklistedUsersRepository { get; private set; }
        public IMatchPrizesRepository MatchPrizeRepository { get; private set; }
        public IMatchImagesRepository MatchImagesRepository { get; private set; }

        public IAccountSubscriptionService AccountSubscriptionService { get; private set; }
        public ISubscriptionService SubscriptionService { get; private set; }
        public IMatchGamesService MatchGamesService { get; private set; }
        public IGameService GameService { get; private set; }
        public IMatchPrizeService MatchPrizeService { get; private set; }
        public IMatchImageService MatchImageService { get; private set; }
        public IMatchGameBoardService MatchGameBoardService { get; private set; }

        public Mock<IExternalClient> MockedExternalClient { get; private set; }
        public Mock<TwitterClient> MockedTwitterClient { get; private set; }

        public HrvsterUser hrvsterUser { get; private set; }

        public GameFeature()
        {
            Initialize();
        }

        public override GameFeature Initialize(
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
            SweepsRepository = ServiceProvider.GetService<ISweepsRepository>();
            SweepEntryRepository = ServiceProvider.GetService<ISweepsEntriesRepository>();
            SweepRulesRepository = ServiceProvider.GetService<ISweepRulesRepository>();
            AccountsRepository = ServiceProvider.GetService<IAccountsRepository>();
            ProfilesRepository = ServiceProvider.GetService<IProfilesRepository>();
            CampaignRepository = ServiceProvider.GetService<ICampaignRepository>();
            PlansRepository = ServiceProvider.GetService<IPlansRepository>();
            MatchGameRulesRepository = ServiceProvider.GetService<IMatchGameRulesRepository>();
            BlacklistedUsersRepository = ServiceProvider.GetService<IBlacklistedUserRepository>();
            MatchPrizeRepository = ServiceProvider.GetService<IMatchPrizesRepository>();
            MatchImagesRepository = ServiceProvider.GetService<IMatchImagesRepository>();

            SweepstakesService = ServiceProvider.GetService<ISweepstakesService>();
            AccountSubscriptionService = ServiceProvider.GetService<IAccountSubscriptionService>();
            SubscriptionService = ServiceProvider.GetService<ISubscriptionService>();
            MatchGamesService = ServiceProvider.GetService<IMatchGamesService>();
            GameService = ServiceProvider.GetService<IGameService>();
            MatchPrizeService = ServiceProvider.GetService<IMatchPrizeService>();
            MatchImageService = ServiceProvider.GetService<IMatchImageService>();
            MatchGameBoardService = ServiceProvider.GetService<IMatchGameBoardService>();

            hrvsterUser = ServiceProvider.GetService<HrvsterUser>();
            hrvsterUser.EmailAddress = TestData.EmailAddress;
            hrvsterUser.UserUid = TestData.AccountUid;
            hrvsterUser.DisplayName = TestData.DisplayName;
            hrvsterUser.Locale = TestData.Locale;
            hrvsterUser.WindowsTimeZone = TestData.WindowsTimezone;

            MockedExternalClient = ServiceProvider.GetService<Mock<IExternalClient>>();
            MockedTwitterClient = ServiceProvider.GetService<Mock<TwitterClient>>();
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

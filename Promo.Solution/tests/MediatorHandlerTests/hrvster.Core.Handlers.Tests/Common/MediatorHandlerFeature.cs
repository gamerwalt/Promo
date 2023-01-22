using hrvster.ApplicationServices.Subscription;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Core.Clients.Publishers;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
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
using TestUtilities.Mocks.Publishers;

namespace hrvster.Core.Handlers.Tests.Common
{
	public class MediatorHandlerFeature : StorageFeatureBase<MediatorHandlerFeature>, IFeature<MediatorHandlerFeature>
	{
		public IMediator Mediator { get; private set; }
		public IConfiguration Configuration { get; private set; }
		public IAccountsRepository AccountRepository { get; private set; }
		public IProfilesRepository ProfileRespository { get; private set; }

        public ICampaignRepository CampaignRepository { get; private set; }
		public ISweepsRepository SweepsRepository { get; private set; }
		public IMailRepository MailRepository { get; private set; }
		public ISweepWinnersRepository SweepWinnersRepository { get; private set; }
		public ISweepPrizesRepository SweepsPrizesRepository { get; private set; }
		public ISweepEntryRulesRepository SweepEntryRulesRepository { get; private set; }
		public ISweepDrawRuleRepository SweepDrawRuleRepository { get; private set; }
		public ISweepsEntriesRepository SweepEntryRepository { get; private set; }
		public ISweepRulesRepository SweepRulesRepository { get; private set; }
		public IPlansRepository PlansRepository { get; private set; }

		public IDrawService DrawService { get; private set; }
		public IAccountSubscriptionService AccountSubscriptionService { get; private set; }
		public ISubscriptionService SubscriptionService { get; private set; }
        public SweepstakeUtility SweepstakeUtility { get; private set; }

        public Mock<IExternalClient> MockedExternalClient { get; private set; }
		public Mock<TwitterClient> MockedTwitterClient { get; private set; }
		public Mock<IToastNotification> MockedToastNotification { get; private set; }
        public MediatorHandlerFeature()
		{
			Initialize();
		}

		public override MediatorHandlerFeature Initialize(
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
			AccountRepository = ServiceProvider.GetService<IAccountsRepository>();
			ProfileRespository = ServiceProvider.GetService<IProfilesRepository>();
			CampaignRepository = ServiceProvider.GetService<ICampaignRepository>();
			SweepsRepository = ServiceProvider.GetService<ISweepsRepository>();
			MailRepository = ServiceProvider.GetService<IMailRepository>();
			SweepWinnersRepository = ServiceProvider.GetService<ISweepWinnersRepository>();
			SweepsPrizesRepository = ServiceProvider.GetService<ISweepPrizesRepository>();
			SweepEntryRulesRepository = ServiceProvider.GetService<ISweepEntryRulesRepository>();
			SweepDrawRuleRepository = ServiceProvider.GetService<ISweepDrawRuleRepository>();
			SweepEntryRepository = ServiceProvider.GetService<ISweepsEntriesRepository>();
			SweepRulesRepository = ServiceProvider.GetService<ISweepRulesRepository>();
			SweepstakeUtility = ServiceProvider.GetService<SweepstakeUtility>();
			DrawService = ServiceProvider.GetService<IDrawService>();
			AccountSubscriptionService = ServiceProvider.GetService<IAccountSubscriptionService>();
            SubscriptionService = ServiceProvider.GetService<ISubscriptionService>();
            PlansRepository = ServiceProvider.GetService<IPlansRepository>();

            MockedExternalClient = ServiceProvider.GetService<Mock<IExternalClient>>();
			MockedTwitterClient = ServiceProvider.GetService<Mock<TwitterClient>>();
			MockedToastNotification = ServiceProvider.GetService<Mock<IToastNotification>>();

            SetupHrvsterUser();
		}

		private void SetupHrvsterUser()
		{
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
			if(ServiceProvider != null)
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

using hrvster.ApplicationServices.Subscription;
using hrvster.ApplicationServices.Sweepstakes;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.ApplicationServices.Tests.Common
{
	public class SweepsFeature : StorageFeatureBase<SweepsFeature>, IFeature<SweepsFeature>
	{
		public IMediator Mediator { get; private set; }
		public IConfiguration Configuration { get; private set; }
		public ISweepsRepository SweepsRepository { get; private set; }
		public ISweepsEntriesRepository SweepEntryRepository { get; private set; }
		public ISweepRulesRepository SweepRulesRepository { get; private set; }

		public ISweepstakesService SweepstakesService { get; private set; }
		public IAccountsRepository AccountRepository { get; private set; }
		public ICampaignRepository CampaignRepository { get; private set; }
		public IPlansRepository PlansRepository { get; private set; }

		public IAccountSubscriptionService AccountSubscriptionService { get; private set; }
		public ISubscriptionService SubscriptionService { get; private set; }

		public SweepsFeature()
		{
			Initialize();
		}

		public override SweepsFeature Initialize(
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

			AccountRepository = ServiceProvider.GetService<IAccountsRepository>();
			CampaignRepository = ServiceProvider.GetService<ICampaignRepository>();
			PlansRepository = ServiceProvider.GetService<IPlansRepository>();

			SweepstakesService = ServiceProvider.GetService<ISweepstakesService>();
			AccountSubscriptionService = ServiceProvider.GetService<IAccountSubscriptionService>();
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

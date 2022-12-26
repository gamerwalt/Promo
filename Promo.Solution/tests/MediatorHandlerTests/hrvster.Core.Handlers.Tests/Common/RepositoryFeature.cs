using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities;
using TestUtilities.Data;

namespace hrvster.Core.Handlers.Tests.Common
{
	public class RepositoryFeature : StorageFeatureBase<RepositoryFeature>, IFeature<RepositoryFeature>
	{
		public IMediator Mediator { get; private set; }
		public IConfiguration Configuration { get; private set; }
		public IAccountsRepository AccountRepository { get; private set; }
		public ICampaignRepository CampaignRepository { get; private set; }
		public ISweepsRepository SweepsRepository { get; private set; }

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
			Mediator = ServiceProvider.GetService<IMediator>();
			AccountRepository = ServiceProvider.GetService<IAccountsRepository>();
			CampaignRepository = ServiceProvider.GetService<ICampaignRepository>();
			SweepsRepository = ServiceProvider.GetService<ISweepsRepository>();

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

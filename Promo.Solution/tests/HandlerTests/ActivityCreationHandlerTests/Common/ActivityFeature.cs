using FluentAssertions.Common;
using MediatR;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestUtilities;

namespace ActivityCreationHandlerTests.Common
{
	public class ActivityFeature : StorageFeatureBase<ActivityFeature>, IFeature<ActivityFeature>
	{
		public IMediator Mediator { get; private set; }
		public IConfiguration Configuration { get; private set; }

		public ActivityFeature()
		{
			Initialize();
		}

		public override ActivityFeature Initialize(
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
				//services.MockFirebaseAuthProvider()
				//	.MockFirebaseAuthManager()
				//	.MockSessionManager()
				//	.MockToastNotification()
				//	.MockProfileRepository();
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

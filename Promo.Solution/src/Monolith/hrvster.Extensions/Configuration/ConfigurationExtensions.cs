using hrvster.ApplicationServices.Common;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Infrastructure.Harvester;
using hrvster.Infrastructure.Scheduling;
using hrvster.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace hrvster.Extensions.Configuration
{
	public static class ConfigurationExtensions
	{
		public static IServiceCollection AddServiceContainer(this IServiceCollection services, ServiceContainer serviceContainer)
			=> services.AddSingleton(c => BuildServiceContainer(c, serviceContainer));
		

		private static ServiceContainer BuildServiceContainer(IServiceProvider serviceProvider, ServiceContainer serviceContainer)
			=> serviceContainer;
	}
}

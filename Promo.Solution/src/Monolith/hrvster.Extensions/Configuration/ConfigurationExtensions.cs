using hrvster.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace hrvster.Extensions.Configuration
{
	public static class ConfigurationExtensions
	{
		public static void AddServiceContainer(this IServiceCollection services, ServiceContainer serviceContainer)
		{
			services.AddSingleton(c => BuildServiceContainer(c, serviceContainer));
		}

		private static ServiceContainer BuildServiceContainer(IServiceProvider serviceProvider, ServiceContainer serviceContainer)
		{
			return serviceContainer;
		}
	}
}

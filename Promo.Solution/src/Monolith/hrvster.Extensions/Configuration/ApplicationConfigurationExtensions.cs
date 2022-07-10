using hrvster.ApplicationServices.Harvesters;
using hrvster.ApplicationServices.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ApplicationConfigurationExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
			=> services
				.AddTransient<ITwitterHarvesterService, TwitterHarvesterService>()
				.AddTransient<ITweetProcessor, TweetProcessor>();
		
	}
}

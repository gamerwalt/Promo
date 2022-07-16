using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.ApplicationServices.Processors;
using hrvster.ApplicationServices.Sweepstakes;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ApplicationConfigurationExtensions
	{
		public static IServiceCollection AddApplicationCronServices(this IServiceCollection services)
			=> services
				.AddTransient<ITwitterHarvesterService, TwitterHarvesterService>()
				.AddTransient<ITweetProcessor, TweetProcessor>()
				.AddTransient<IDrawService, DrawService>();
		
	}
}

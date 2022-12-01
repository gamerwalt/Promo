using hrvster.ApplicationServices.Processors;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ProcessorConfigurationExtensions
	{
		public static IServiceCollection AddProcessors(this IServiceCollection services)
			=> services.AddTweetProcessors();

		public static IServiceCollection AddTweetProcessors(this IServiceCollection services)
			=> services.AddTransient<ITweetProcessor, TweetProcessor>();
	}
}

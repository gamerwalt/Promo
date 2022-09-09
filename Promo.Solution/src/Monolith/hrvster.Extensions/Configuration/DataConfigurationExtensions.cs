using hrvster.Infrastructure.Harvester;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class DataConfigurationExtensions
	{
		public static IServiceCollection AddHarvesterRepositories(this IServiceCollection services)
			=> services
				.AddTransient<IIncomingTweetsRepository, IncomingTweetsRepository>();
	}
}

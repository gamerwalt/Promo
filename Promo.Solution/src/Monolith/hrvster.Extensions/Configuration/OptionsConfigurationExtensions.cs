using hrvster.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace hrvster.Extensions.Configuration
{
	public static class OptionsConfigurationExtensions
	{
		public static IServiceCollection AddServicesOptions(this IServiceCollection services, IConfiguration configuration)
			=> services.Configure<Services>(configuration)
				.Configure<Services>(configuration.GetSection(Services.SectionName))
				.AddTransient(p => p.GetService<IOptions<Services>>().Value);

		public static IServiceCollection AddSweeepstakesOptions(this IServiceCollection services, IConfiguration configuration)
			=> services.Configure<SweepstakeConfiguration>(configuration)
				.Configure<SweepstakeConfiguration>(configuration.GetSection(SweepstakeConfiguration.SectionName))
				.AddTransient(p => p.GetService<IOptions<SweepstakeConfiguration>>().Value);
	}
}

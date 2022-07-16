using hrvster.Models.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace hrvster.Extensions.OptionsConfiguration
{
	public static class OptionConfigurationExtensions
	{
		public static IServiceCollection AddDrawSettingConfiguration(this IServiceCollection services, IConfiguration configuration)
			=> services
				.Configure<DrawSettingOptions>(configuration)
				.Configure<DrawSettingOptions>(configuration.GetSection(DrawSettingOptions.SectionName))
				.AddTransient(s => s.GetService<IOptions<DrawSettingOptions>>().Value);

	}
}

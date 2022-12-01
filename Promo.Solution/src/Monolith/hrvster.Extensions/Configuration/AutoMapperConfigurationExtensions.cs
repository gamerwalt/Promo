using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class AutoMapperConfigurationExtensions
	{
		public static IServiceCollection AddAutomapperConfiguration(this IServiceCollection services, System.Type type)
			=> services.AddAutoMapper(type);
	}
}

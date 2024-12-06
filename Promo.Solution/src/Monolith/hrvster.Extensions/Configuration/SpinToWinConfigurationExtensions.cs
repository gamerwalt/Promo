using hrvster.ApplicationServices.SpinToWin;
using hrvster.Infrastructure.SpinToWinRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class SpinToWinConfigurationExtensions
	{
		public static IServiceCollection AddSpinToWin(this IServiceCollection services, IConfiguration config)
		{
			return services.AddTransient<ISpinToWinService, SpinToWinService>()
				.AddTransient<ISpinToWinRepository, SpinToWinRepository>();
		}
	}
}

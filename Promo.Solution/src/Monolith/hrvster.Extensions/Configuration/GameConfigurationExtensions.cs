using hrvster.Games.Match;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class GameConfigurationExtensions
	{
		public static IServiceCollection AddMatchEngineServices(this IServiceCollection services)
			=> services.AddTransient<MatchEngine>();
	}
}

using hrvster.ApplicationServices.MatchGames;
using hrvster.Games.Match;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class GameConfigurationExtensions
	{
		public static IServiceCollection AddMatchEngineServices(this IServiceCollection services)
			=> services.AddTransient<MatchEngine>()
				.AddMatchGameService();

		public static IServiceCollection AddMatchGameService(this IServiceCollection services)
			=> services.AddTransient<IMatchGamesService, MatchGamesService>()
				.AddTransient<IMatchGameReadinessService, MatchGameReadinessService>()
				.AddTransient<IMatchRulesService, MatchRulesService>()
				.AddTransient<IMatchImageService, MatchImagesService>();
	}
}

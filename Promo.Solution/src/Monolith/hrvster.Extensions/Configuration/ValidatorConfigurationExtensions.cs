using hrvster.Core.Validators.Sweepstakes;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class ValidatorConfigurationExtensions
	{
		public static IServiceCollection AddUpdateValidators(this IServiceCollection services)
		{
			services.AddSingleton<ISweepRuleUpdateValidator, SweepRuleUpdateValidator>();

			return services;
		}
	}
}

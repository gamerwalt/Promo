using hrvster.Core;
using Microsoft.Extensions.DependencyInjection;

namespace hrvster.Extensions.Configuration
{
	public static class AntiforgeryConfigurationExtensions
	{
		public static IServiceCollection AddAntiforgeryConfig(this IServiceCollection services)
		{
			services.AddAntiforgery(t =>
			{
				t.Cookie.Name = AntiforgeryTokenExtractor.AntiForgeryCookieName;
				t.FormFieldName = AntiforgeryTokenExtractor.AntiForgeryFieldName;
			});

			return services;
		}
	}
}

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Promo.Core.Authentication.ConfigurationOptions;

namespace Promo.Configuration.Extensions
{
	public static class GoogleServicesConfiguration
	{
		public static IServiceCollection AddGoogleServices(this IServiceCollection services, IConfiguration configuration)
		{
			var googleServicesOptions = configuration.GetSection(GoogleServicesOptions.GoogleServices).Get<GoogleServicesOptions>();

			services.Configure<GoogleServicesOptions>(configuration);
			services.Configure<GoogleServicesOptions>(configuration.GetSection(GoogleServicesOptions.GoogleServices));
			services.AddTransient(p => p.GetService<IOptions<GoogleServicesOptions>>().Value);

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.Authority = googleServicesOptions.FirebaseAppId;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = googleServicesOptions.FirebaseAppId,
						ValidateAudience = true,
						ValidAudience = googleServicesOptions.ProjectId,
						ValidateLifetime = true
					};
				});

			return services;
		}

		public static IServiceCollection AddFirebaseServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<FirebaseApp>(c => BuildFirebaseApp(configuration));

			return services;
		}

		private static FirebaseApp BuildFirebaseApp(IConfiguration configuration)
		{
			var firebaseConfig = new FirebaseConfig();
			configuration.Bind(FirebaseConfig.Firebase, firebaseConfig);

			var firebaseConfigSerialized = JsonConvert.SerializeObject(firebaseConfig);

			return FirebaseApp.Create(new AppOptions()
			{
				Credential = GoogleCredential.FromJson(firebaseConfigSerialized),
			});
		}
	}
}

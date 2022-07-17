using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Promo.Core.Authentication.ConfigurationOptions;
using System;

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
			services.Configure<FirebaseConfig>(configuration);
			services.Configure<FirebaseConfig>(configuration.GetSection(FirebaseConfig.Firebase));
			services.AddTransient(p => p.GetService<IOptions<FirebaseConfig>>().Value);

			services.AddSingleton<FirebaseApp>(c => BuildFirebaseApp(c, configuration));
			services.AddSingleton<FirebaseAuth>(c => BuildFirebaseAuth(c, configuration));

			return services;
		}

		private static FirebaseAuth BuildFirebaseAuth(IServiceProvider c, IConfiguration configuration)
		{
			return FirebaseAuth.GetAuth(c.GetService<FirebaseApp>());
		}

		private static FirebaseApp BuildFirebaseApp(IServiceProvider sp, IConfiguration configuration)
		{
			return FirebaseApp.Create(new AppOptions()
			{
				Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(sp.GetService<FirebaseConfig>()))
			});
		}
	}
}

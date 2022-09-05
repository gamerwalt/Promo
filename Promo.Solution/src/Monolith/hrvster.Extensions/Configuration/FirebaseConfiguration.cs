using Firebase.Auth;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using hrvster.Infrastructure.Authentication.ConfigurationOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
using FirebaseConfig = hrvster.Infrastructure.Authentication.ConfigurationOptions.FirebaseConfig;

namespace hrvster.Extensions.Configuration
{
	public static class FirebaseConfiguration
	{
		public static IServiceCollection AddFirebaseServices(
			this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<FirebaseConfig>(configuration);
			services.Configure<FirebaseConfig>(configuration.GetSection(FirebaseConfig.Firebase));
			services.AddTransient(p => p.GetService<IOptions<FirebaseConfig>>().Value);

			services.AddSingleton<FirebaseApp>(c => BuildFirebaseApp(c, configuration));
			services.AddSingleton<FirebaseAuth>(c => BuildFirebaseAuth(c, configuration));
			services.AddSingleton<FirebaseAuthProvider>(c => BuildFirebaseAuthProvider(c, configuration));
			return services;
		}

		private static FirebaseAuthProvider BuildFirebaseAuthProvider(IServiceProvider sp, IConfiguration configuration)
		{
			var googleServicesOption = sp.GetService<GoogleServicesOptions>();
			return new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(googleServicesOption.WebApiKey));
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

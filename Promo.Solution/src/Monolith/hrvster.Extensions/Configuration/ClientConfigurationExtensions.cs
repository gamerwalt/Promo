using hrvster.ApplicationServices.Harvesters.Twitter;
using hrvster.Core.Authentication;
using hrvster.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using System.Text;

namespace hrvster.Extensions.Configuration
{
	public static class ClientConfigurationExtensions
	{
		public static IServiceCollection AddHttpClients(this IServiceCollection services)
			=> services.AddIdentityClient();

		public static IServiceCollection AddIdentityClient(this IServiceCollection services)
		{
			services.AddHttpClient<IIdentityClient, IdentityClient>();

			return services;
		}

		public static IServiceCollection AddHarvesterService(this IServiceCollection services)
		{
			var sp = services.BuildServiceProvider();
			var harvesterServiceOptions = sp.GetRequiredService<HarvesterSettingsOptions>();

			services.AddHttpClient<IHarvesterService, HarvesterService>(c =>
			{
				c.BaseAddress = new Uri(harvesterServiceOptions.ServiceUri);
				c.DefaultRequestHeaders.Clear();
				c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
					"Basic",
					EncodeCredentials(harvesterServiceOptions.ServiceUsername, harvesterServiceOptions.ServicePassword));
			});

			return services;
		}

		private static string EncodeCredentials(string serviceUsername, string servicePassword)
		{
			if (string.IsNullOrWhiteSpace(serviceUsername)) throw new ArgumentNullException(nameof(serviceUsername));
			if (servicePassword == null) servicePassword = "";

			var encoding = Encoding.UTF8;
			var credential = string.Format("{0}:{1}", serviceUsername, servicePassword);

			return Convert.ToBase64String(encoding.GetBytes(credential));
		}
	}
}

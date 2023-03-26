using hrvster.ApplicationServices.Publishing;
using hrvster.Core.Clients.Publishers;
using hrvster.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace hrvster.Extensions.Configuration
{
	public static class PublisherConfigurationExtensions
	{
		public static IServiceCollection AddPublishers(this IServiceCollection services)
			=> services.AddTransient<TwitterClient>()
				.AddTransient<FacebookClient>()
				.AddTransient<InstagramClient>()
				.AddTransient<Func<ExternalClientType, IExternalClient>>(sp => serviceTypeName =>
				{
					switch (serviceTypeName)
					{
						case ExternalClientType.Twitter:
							return sp.GetService<TwitterClient>();
						case ExternalClientType.Facebook:
							return sp.GetService<FacebookClient>();
						case ExternalClientType.Instagram:
							return sp.GetService<InstagramClient>();
						default:
							return sp.GetService<TwitterClient>();
					}
				})
				.AddTransient<IPublishService, PublishService>();
	}
}

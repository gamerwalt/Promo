using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Promo.Configuration.Extensions
{
	public static class MediatorConfiguration
	{
		public static IServiceCollection AddMediator(this IServiceCollection services, Assembly assembly)
		{
			services.AddMediatR(assembly);

			return services;
		}
	}
}

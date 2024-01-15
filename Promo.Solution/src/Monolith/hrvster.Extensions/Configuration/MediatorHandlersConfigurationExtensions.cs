using hrvster.Core.Handlers;
using hrvster.Core.Handlers.ServiceHandlers;
using hrvster.Core.Notifications.Stripe;
using hrvster.Core.Requests;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.Notifications.Handlers;
using hrvster.Core.Requests.Notifications.Harvester;
using hrvster.Core.Requests.WebRequests;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace hrvster.Extensions.Configuration
{
	public static class MediatorHandlersConfigurationExtensions
	{
		public static IServiceCollection AddMediatorAndFluentValidation(this IServiceCollection services)
		{
			var handlerAssembly = typeof(GetAccountRequest).GetTypeInfo().Assembly;
			var notificationAssembly = typeof(AccountCreatedViaFirebase).GetTypeInfo().Assembly;
			var notificationHandlerAssembly = typeof(StoreAccountInDatabase).GetTypeInfo().Assembly;
			var harvsterHandlerAssembly = typeof(NewHashTagTweetReceived).GetTypeInfo().Assembly;
			var webRequestAssembly = typeof(SigninForm).GetTypeInfo().Assembly;
			var handlers = typeof(SubscribeUserToPlan).GetTypeInfo().Assembly;
			var serviceHandlers = typeof(ProcessNewTweet).GetTypeInfo().Assembly;
			var notificationAssemblyV2 = typeof(UserSubscribedThroughStripe).GetTypeInfo().Assembly;

			services.AddMediatR(new[] { handlerAssembly, notificationAssembly, notificationHandlerAssembly, harvsterHandlerAssembly, webRequestAssembly, handlers, serviceHandlers, notificationAssemblyV2 });

			return services;
		}
	}
}

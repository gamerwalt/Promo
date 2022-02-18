using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using MediatR;
using Microsoft.Extensions.Logging;
using Promo.Core.MessageContracts;
using Promo.Core.Notifications;
using System.Threading.Tasks;

namespace Promo.Core.MessageComponents.Consumers
{
	public class AccountCreatedConsumer : IConsumer<IAccountCreated>
	{
		private readonly ILogger<AccountCreatedConsumer> _logger;
		private readonly IMediator _mediator;

		public AccountCreatedConsumer() { }

		public AccountCreatedConsumer(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<AccountCreatedConsumer>();
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<IAccountCreated> context)
		{
			_logger.LogInformation("$Consuming message for account created.");
			_logger.LogInformation("Account Id : {accountUid} message for account created.", context.Message.AccountUid);

			await context.RespondAsync<IAccountCreatedAccepted>(new
			{
				AccountUid = context.Message.AccountUid
			});

			await _mediator.Publish(new AccountCreated(context.Message));


			//Save in Notification database
			//Save in Games Database
			//Save in Harvster database
			//save in fulfillment database


			//verification link sending
			//if (FirebaseApp.DefaultInstance == null)
			//{
			//	var firebaseConfig = new FirebaseConfig();
			//	_config.Bind(FirebaseConfig.Firebase, firebaseConfig);

			//	var firebaseConfigSerialized = JsonConvert.SerializeObject(firebaseConfig);
			//	FirebaseApp.Create(new AppOptions()
			//	{
			//		Credential = GoogleCredential.FromJson(firebaseConfigSerialized),
			//	});
			//}

			//var emailLink = await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(notification.CreatedUser.Email);

			//var email = new Promo.Core.Models.Email
			//{
			//	EmailUid = Guid.NewGuid(),
			//	EmailAddress = notification.CreatedUser.Email,
			//	Status = EmailStatus.New,
			//	Template = EmailTemplate.VerificationEmail,
			//	Sent = false,
			//	Parameters = new Dictionary<string, string>
			//	{
			//		{ EmailParameters.VerificationLink, emailLink },
			//		{ EmailParameters.Email, notification.CreatedUser.Email },
			//		{ EmailParameters.DisplayName, notification.CreatedUser.DisplayName }
			//	}
			//};

			//var authModel = await _serviceAuthenticate.Authenticate();

			//await _mailClient.CreateAsync(email, authModel.IdToken);

			//FirebaseApp.DefaultInstance.Delete();
		}
	}

	public class AccountCreatedConsumerDefinition : ConsumerDefinition<AccountCreatedConsumer>
	{
		public AccountCreatedConsumerDefinition()
		{
			ConcurrentMessageLimit = 8;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AccountCreatedConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
		}
	}
}

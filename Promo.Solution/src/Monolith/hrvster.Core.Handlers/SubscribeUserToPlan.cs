using hrvster.Core.Authentication;
using hrvster.Core.Requests;
using hrvster.Infrastructure.Subscriptions;
using hrvster.Models;
using hrvster.Models.Core;
using hrvster.Models.Options;
using hrvster.Models.Subscriptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers
{
	public class SubscribeUserToPlan : IRequestHandler<SubscribeUserToPlanRequest, ServiceOperation<AccountSubscription>>
	{
		private readonly ILogger<SubscribeUserToPlan> _logger;
		private readonly IAccountSubscriptionsRepository _repository;
		private readonly ISessionManager _sessionManager;
		private readonly HarvesterSettingsOptions _harvsterSettings;
		private readonly PaymentProviderOptions _paymentProviderOption;

		private readonly PaymentClient _paymentClient;

		public SubscribeUserToPlan(
			ILoggerFactory loggerFactory,
			IAccountSubscriptionsRepository repository,
			ISessionManager sessionManager,
			HarvesterSettingsOptions harvesterSettings,
			PaymentProviderOptions paymentProviderOption)
		{
			_logger = loggerFactory.CreateLogger<SubscribeUserToPlan>();
			_repository = repository;
			_sessionManager = sessionManager;
			_harvsterSettings = harvesterSettings;

			_paymentProviderOption = paymentProviderOption;
			_paymentClient = Enum.Parse<PaymentClient>(_paymentProviderOption.PaymentProvider);
		}

		public async Task<ServiceOperation<AccountSubscription>> Handle(
			SubscribeUserToPlanRequest request, CancellationToken cancellationToken)
		{
			var hrvsterUser = _sessionManager.GetUserInformation();
			var startDate = DateTime.Now;
			var endDate = request.IsTrial 
				? startDate.AddDays(_harvsterSettings.TrialDays) 
				: startDate.AddDays(30);

			var startDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(startDate, TimeZoneInfo.Local.Id,
				hrvsterUser.WindowsTimeZone);
			var endDateUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(endDate, TimeZoneInfo.Local.Id,
				hrvsterUser.WindowsTimeZone);

			if (!hrvsterUser.IsDaylight)
			{
				startDateUtc = startDateUtc.AddHours(-2);
				endDateUtc = endDateUtc.AddHours(-2);
			}

			var subscriptionUid = Guid.NewGuid();

			_logger.LogInformation("Account with Uid {accountUid} will be subscribed to the plan {planName}",
				request.UserUId,
				request.Plan.PlanName);

			//Todo: check if we have one already, if we do, add to history, then update with most recent information

			var accountSubscription = await _repository.GetAccountSubscriptionAsync(request.UserUId);

			if(accountSubscription != null)
			{
				if(accountSubscription.IsTrial && request.IsTrial)
				{
					return ServiceOperation<AccountSubscription>.CreateFailedOperation(null, "Account was not able to be subscribed. User is trying to re-trial.", nameof(SubscribeUserToPlan));
				}
				//add to history
				await _repository.CreateHistoryAsync(accountSubscription);
				//update current
				await _repository.DeleteSubscriptionAsync(accountSubscription);
			}
			
			await _repository.CreateAsync(new AccountSubscription
			{
				Account = new Models.Account
				{
					AccountUid = request.UserUId
				},
				Plan = request.Plan,
				StartDate = startDate,
				StartDateUtc = startDateUtc,
				EndDate = endDate,
				EndDateUtc = endDateUtc,
				IsDiscounted = false,
				SubscriptionStatus = Models.SubscriptionStatus.PendingPayment,
				SubscriptionUid = subscriptionUid,
				PaymentClient = Models.PaymentClient.Flutterwave,
				IsTrial = request.IsTrial
			});
						

			_logger.LogInformation("Account subscription created for account with uid {accountUid}",
				request.UserUId);

			var model = await _repository.GetAsync(subscriptionUid);

			if(model == null)
			{
				return ServiceOperation<AccountSubscription>.CreateFailedOperation(null, "Account was not able to be subscribed. Our technicians will be on this.", nameof(SubscribeUserToPlan));
			}

			return ServiceOperation<AccountSubscription>.CreateSuccessOperation(model, "Account has been subscribed. It is currently pending payment.");
		}
	}
}

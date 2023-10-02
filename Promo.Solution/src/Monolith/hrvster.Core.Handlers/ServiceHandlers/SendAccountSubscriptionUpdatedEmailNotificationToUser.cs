using hrvster.Core.Requests.ServiceRequests;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.ServiceHandlers
{
	public class SendAccountSubscriptionUpdatedEmailNotificationToUser : AsyncRequestHandler<SendAccountSubscriptionUpdatedEmailNotificationToUserRequest>
	{
		private readonly IMailRepository _repository;

		public SendAccountSubscriptionUpdatedEmailNotificationToUser(IMailRepository repository)
		{
			_repository = repository;
		}

		protected override async Task Handle(
			SendAccountSubscriptionUpdatedEmailNotificationToUserRequest request, CancellationToken cancellationToken)
		{
			await _repository.CreateAsync(new Models.Email
			{
				Template = Models.EmailTemplate.AccountSubscriptionUpdateEmail,
				EmailAddress = request.Account.EmailAddress,
				EmailUid = Guid.NewGuid(),
				Status = Models.EmailStatus.New,
				Parameters = new Dictionary<string, string>()
				{
					{ EmailParameters.EmailAddress, request.Account.EmailAddress },
					{ EmailParameters.CurrentAccountSubscriptionStatus, request.Status.ToString() },
					{ EmailParameters.NewAccountSubscriptionStatus, request.NewStatus.ToString() },
					{ EmailParameters.PendingHours, request.PendingHours.ToString() },
				}
			});
		}
	}
}

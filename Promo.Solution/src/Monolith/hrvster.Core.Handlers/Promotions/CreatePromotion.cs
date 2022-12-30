using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Promotions
{
	public class CreatePromotion : AsyncRequestHandler<CreatePromotionRequest>
	{
		private readonly ILogger<CreatePromotion> _logger;
		private readonly ICampaignRepository _repository;
		private readonly IMediator _mediator;

		public CreatePromotion(
			ILoggerFactory loggerFactory,
			ICampaignRepository repository,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<CreatePromotion>();
			_repository = repository;
			_mediator = mediator;
		}

		protected override async Task Handle(CreatePromotionRequest request, CancellationToken cancellationToken)
		{
			var model = CreateCampaignModel(request);

			await _repository.CreateCampaignAsync(model, request.TimeZone, request.IsDaylight);

			await _mediator.Publish(new PromotionCreated(model));
		}

		private static Campaign CreateCampaignModel(CreatePromotionRequest request)
		{
			return new Models.Campaign
			{
				CampaignUid = request.PromotionUid,
				ActiveInd = true,
				CampaignName = request.Name,
				Description = request.Description,
				StartDate = request.StartDate,
				EndDate = request.EndDate,
				Account = new Models.Account
				{
					AccountUid = request.AccountUid,
				},
				CampaignType = new Models.CampaignType
				{
					CampaignTypeUid = request.CampaignTypeUid
				}
			};
		}
	}
}

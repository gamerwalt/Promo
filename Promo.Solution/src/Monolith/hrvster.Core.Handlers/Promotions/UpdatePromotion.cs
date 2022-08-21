using hrvster.Core.Requests.WebRequests.Promotions;
using hrvster.Infrastructure;
using hrvster.Models.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Promotions
{
	public class UpdatePromotion : AsyncRequestHandler<UpdatePromotionRequest>
	{
		private readonly ILogger<UpdatePromotion> _logger;
		private readonly ICampaignRepository _repository;

		public UpdatePromotion(ILoggerFactory loggerFactory, ICampaignRepository repository)
		{
			_logger = loggerFactory.CreateLogger<UpdatePromotion>();
			_repository = repository;
		}

		protected override async Task Handle(UpdatePromotionRequest request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Handling {className} for Request {request}",
				nameof(UpdatePromotion),
				request.ToJsonString());

			await _repository.UpdateCampaignAsync(request.CampaignUid, request.CampaignName,
				request.Description, request.CampaignTypeUid, request.StartDate, request.EndDate);
		}
	}
}

using hrvster.Infrastructure;
using hrvster.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Common
{
	public interface ICampaignService
	{
		Task<Campaign> GetCampaignAsync(Guid promotionUid, CancellationToken cancellationToken);
		Task<Campaign> GetCampaignFromGameUidAsync(Guid gameUid, string gameType);
	}

	public class CampaignService : ICampaignService
	{
		private readonly ILogger<CampaignService> _logger;
		private readonly ICampaignRepository _repository;

		public CampaignService(ILoggerFactory loggerFactory, ICampaignRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CampaignService>();
			_repository = repository;
		}

		public Task<Campaign> GetCampaignAsync(Guid promotionUid, CancellationToken cancellationToken)
			=> _repository.GetCampaignByUidAsync(promotionUid);

		public async Task<Campaign> GetCampaignFromGameUidAsync(Guid gameUid, string gameType)
		{
			if (gameType == GameTypes.Sweepstake)
				return await _repository.GetCampaignBySweepsGameAsync(gameUid);

			return await _repository.GetCampaignByMatchGameAsync(gameUid);
		}
	}
}

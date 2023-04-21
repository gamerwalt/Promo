using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Common
{
	public interface IUserRateLimiterService
	{
		Task UpsertAsync(string hashtag, string externalUserId, UserIdentifierType userIdentifierType);
		Task<bool> IsAllowedAsync(string externalUserId, List<string> hashtags, UserIdentifierType userIdentifierType);
	}

	public class UserRateLimiterService : IUserRateLimiterService
	{
		private readonly ILogger<UserRateLimiterService> _logger;
		private readonly IUserLimiterRepository _repository;
		private readonly HarvesterSettingsOptions _hrvsterSettings;

		public UserRateLimiterService(
			ILoggerFactory loggerFactory,
			IUserLimiterRepository repository,
			HarvesterSettingsOptions hrvsterSettings)
		{
			_logger = loggerFactory.CreateLogger<UserRateLimiterService>();
			_repository = repository;
			_hrvsterSettings = hrvsterSettings;
		}

		public async Task UpsertAsync(string hashtag, string externalUserId, UserIdentifierType userIdentifierType)
		{
			var records = await _repository.GetByWatchValue(hashtag, userIdentifierType);
			if (records.Any(r => r.ExternalUserId == externalUserId))
			{
				var userLimiter = records.FirstOrDefault(r => r.ExternalUserId == externalUserId);

				await _repository.UpdatedAsync(userLimiter);

				return;
			}

			await _repository.CreateAsync(Guid.NewGuid(), externalUserId, userIdentifierType, hashtag);
		}


		public async Task<bool> IsAllowedAsync(
			string externalUserId,
			List<string> hashtags,
			UserIdentifierType userIdentifierType)
		{
			foreach (var hashtag in hashtags)
			{
				var records = await _repository.GetByWatchValue(hashtag, userIdentifierType);

				//Todo: RateLimiter
				if (records.Any(
					r =>
						r.ExternalUserId == externalUserId
						&& IsNotAllowed(r)))
					return false;
			}

			return true;
		}

		private bool IsNotAllowed(UserLimiter userLimiter)
		{
			var timeSpanInMinutes = (TimeSinceLastUpdate(userLimiter) - DateTime.Now).TotalMinutes;

			var absoluteMinutes = Math.Abs(timeSpanInMinutes);

			return absoluteMinutes < _hrvsterSettings.UserTweetRateLimiterInMinutes;
		}

		private DateTime TimeSinceLastUpdate(UserLimiter userLimiter)
		{
			return userLimiter.UpdatedDate ?? userLimiter.CreatedDate;
		}
	}
}

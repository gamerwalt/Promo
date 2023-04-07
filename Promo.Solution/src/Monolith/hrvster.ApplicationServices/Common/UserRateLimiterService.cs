using hrvster.Infrastructure.Harvester;
using hrvster.Models;
using hrvster.Models.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models.Entities;

namespace hrvster.ApplicationServices.Common
{
	public interface IUserRateLimiterService
	{
		Task UpsertAsync(IHashtagEntity hashtag, string externalUserId, UserIdentifierType userIdentifierType);
		Task<bool> IsAllowedAsync(string externalUserId, List<IHashtagEntity> hashtags, UserIdentifierType userIdentifierType);
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

		public async Task UpsertAsync(IHashtagEntity hashtag, string externalUserId, UserIdentifierType userIdentifierType)
		{
			var records = await _repository.GetByWatchValue(hashtag.Text, userIdentifierType);
			if(records.Any(r => r.ExternalUserId == externalUserId))
			{
				var userLimiter = records.FirstOrDefault(r => r.ExternalUserId == externalUserId);

				await _repository.UpdatedAsync(userLimiter);

				return;
			}

			await _repository.CreateAsync(Guid.NewGuid(), externalUserId, userIdentifierType, hashtag.Text);
		}
		

		public async Task<bool> IsAllowedAsync(
			string externalUserId,
			List<IHashtagEntity> hashtags,
			UserIdentifierType userIdentifierType)
		{
			foreach (var hashtag in hashtags)
			{
				var records = await _repository.GetByWatchValue(hashtag.Text, userIdentifierType);

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

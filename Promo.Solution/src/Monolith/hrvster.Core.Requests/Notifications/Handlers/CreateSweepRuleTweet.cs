using hrvster.Core.Requests.Notifications.Publishing;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Core;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Notifications.Handlers
{
	public class CreateSweepRuleTweet : INotificationHandler<SweepRuleTweetPublished>
	{
		private readonly ILogger<CreateSweepRuleTweet> _logger;
		private readonly IWatchTypesRepository _watchTypesRepository;
		private readonly ISweepRulesRepository _repository;

		public CreateSweepRuleTweet(
			ILoggerFactory loggerFactory, 
			IWatchTypesRepository watchTypesRepository,
			ISweepRulesRepository repository)
		{
			_logger = loggerFactory.CreateLogger<CreateSweepRuleTweet>();
			_watchTypesRepository = watchTypesRepository;
			_repository = repository;
		}

		public async Task Handle(SweepRuleTweetPublished notification, CancellationToken cancellationToken)
		{
			var ruleUid = Guid.NewGuid();

			var watchTypes = await _watchTypesRepository.GetWatchTypesAsync();
			var watchTypeCodeSelected = watchTypes.SingleOrDefault(c => c.WatchTypeId.Equals(notification.Message.WatchTypeId)).WatchTypeCode;

			await _repository.CreateRuleAsync(new Rule
			{
				RuleUid = ruleUid,
				Sweep = new Sweep
				{
					SweepUid = notification.Message.SweepUid,
				},
				WatchTypeCode = (WatchTypeCode)Enum.Parse(typeof(WatchTypeCode), watchTypeCodeSelected),
				WatchValue = notification.Message.Hashtag,
				ExternalId = long.Parse(notification.TweetId),
				ActiveInd = true,
				MustBeFollowing = notification.MustBeFollowing
			});
		}
	}
}

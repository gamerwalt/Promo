using hrvster.Core.Requests;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.Sweepstakes;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Sweepstakes
{
	public interface IDrawService
	{
		Task ProcessDrawsAsync(CancellationToken token);
	}

	public class DrawService : IDrawService
	{
		private readonly ILogger<DrawService> _logger;
		private readonly IMediator _mediator;

		public DrawService(
			ILoggerFactory loggerFactory,
			IMediator mediator)
		{
			_logger = loggerFactory.CreateLogger<DrawService>();
			_mediator = mediator;
		}

		public async Task ProcessDrawsAsync(CancellationToken token)
		{
			var sweepsDrawPropertiesRules = await _mediator.Send(new GetSweepstakesRequiringADrawRequest(), token);

			var sweepsAndRules = AggregateSweepAndRules(sweepsDrawPropertiesRules);

			foreach (var rule in sweepsAndRules)
			{
				_logger.LogInformation("Performing the draw for the sweepstakes {sweepName} under the campaign {campaignName}",
					rule.Sweep.Name,
					rule.Campaign.CampaignName);

				var sweepNotifications = await _mediator.Send(new GetSweepNotificationRequest(rule.Sweep));

				var entries = await PreDrawAsync(rule);
				var winners = await _mediator.Send(new GetSweepWinners(rule.Sweep.SweepUid));

				if(sweepNotifications != null && sweepNotifications.DrawAttemptHappened)
				{
					_logger.LogWarning("The sweepstake {sweepName}'s draw has been attempted previously. Will not draw a winner.",
						rule.Sweep.Name);

					continue;
				}

				if (entries == null || entries.Count() == 0)
				{
					_logger.LogWarning("The sweepstake {sweepName} does not have any entries.", rule.Sweep.Name);

					await _mediator.Send(new UpsertSweepNotificationRequest(rule.Sweep, SweepNotificationType.DrawAttempted));

					_logger.LogInformation("Sending an email to {emailAddress} that the sweepstake {sweepName} has no entries.",
						rule.Account.EmailAddress,
						rule.Sweep.Name);

					await _mediator.Send(new SweepEntriesNotEnoughMailRequest(rule.Account.EmailAddress, rule.Sweep.Name));

					continue;
				}

				if (winners.Count() > 0)
				{
					_logger.LogWarning("The sweepstake {sweepName} already has winners. Exiting.", rule.Sweep.Name);

					await _mediator.Send(new UpsertSweepNotificationRequest(rule.Sweep, SweepNotificationType.DrawAttempted));

					continue;
				}

				var drawProperties = await _mediator.Send(new GetDrawPropertiesRequest(rule, entries));

				await _mediator.Send(new PerformDrawRequest(drawProperties));
				
				await _mediator.Send(new UpsertSweepNotificationRequest(rule.Sweep, SweepNotificationType.DrawAttempted));
			}
		}

		private List<SweepDrawPropertiesAndRule> AggregateSweepAndRules(IEnumerable<SweepDrawPropertiesAndRule> sweepsDrawPropertiesRules)
		{
			var newSweepDrawPropertiesAndRule = new List<SweepDrawPropertiesAndRule>();
			var rules = new List<Rule>();

			foreach (var rule in sweepsDrawPropertiesRules)
			{
				var sweepSameNameCount = sweepsDrawPropertiesRules.Where(s => s.Sweep.Name.Equals(rule.Sweep.Name)).Count();

				if (sweepSameNameCount == 1)
				{
					rule.Sweep.Rules.Add(rule.SweepRule);
					newSweepDrawPropertiesAndRule.Add(rule);
					continue;
				}

				var properties = sweepsDrawPropertiesRules.Where(c => c.Sweep.Name.Equals(rule.Sweep.Name));
				foreach (var property in properties)
				{
					rule.Sweep.Rules.Add(property.SweepRule);
				}

				if (!newSweepDrawPropertiesAndRule.Any(s => s.Sweep.Name.Equals(rule.Sweep.Name)))
					newSweepDrawPropertiesAndRule.Add(rule);
			}

			return newSweepDrawPropertiesAndRule;
		}

		private async Task<IEnumerable<SweepEntry>> PreDrawAsync(SweepDrawPropertiesAndRule rule)
		{
			var entries = await _mediator.Send(new GetSweepstakeEntriesRequest(rule.Sweep));

			_logger.LogInformation("Retrieved a total of {entriesCount} entries for the sweepstake {sweepName}",
				entries.Count(),
				rule.Sweep.Name);

			if (entries.Count() == 0)
			{
				_logger.LogWarning("Cannot continue with performing draw for the sweepstakes {sweepName} and campaign {campaignName}.",
					rule.Sweep.Name,
					rule.Campaign.CampaignName);

				return null;
			}

			return entries;
		}
	}
}

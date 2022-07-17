using hrvster.Core.Requests.ServiceRequests;
using hrvster.Models;
using hrvster.Models.Sweepstakes;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
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

			var drawTasks = new List<Task>();

			foreach(var rule in sweepsDrawPropertiesRules)
			{
				_logger.LogInformation("Performing the draw for the sweepstakes {sweepName} under the campaign {campaignName}",
					rule.Sweep.Name,
					rule.Campaign.CampaignName);

				var entries = await PreDrawAsync(rule);
				if (entries == null || entries.Count() == 0)
					continue;

				var drawProperties = await _mediator.Send(new GetDrawPropertiesRequest(rule, entries));

				await _mediator.Send(new PerformDrawRequest(drawProperties));
			}
		}

		private async Task<IEnumerable<SweepEntry>> PreDrawAsync(SweepDrawPropertiesAndRule rule)
		{
			var entries = await _mediator.Send(new GetSweepstakeEntriesRequest(rule.Sweep));

			_logger.LogInformation("Retrieved a total of {entriesCount} for the sweepstake {sweepName}",
				entries.Count(),
				rule.Sweep.Name);

			if(entries.Count() == 0)
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

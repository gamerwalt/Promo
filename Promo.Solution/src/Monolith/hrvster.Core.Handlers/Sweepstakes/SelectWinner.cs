using hrvster.Core.Clients.Common;
using hrvster.Core.Clients.Publishers;
using hrvster.Core.Handlers.ServiceHandlers;
using hrvster.Core.Requests.Notifications;
using hrvster.Core.Requests.ServiceRequests;
using hrvster.Core.Requests.WebRequests.Sweeps;
using hrvster.Infrastructure;
using hrvster.Infrastructure.Accounts;
using hrvster.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Handlers.Sweepstakes
{
	public class SelectWinner : AsyncRequestHandler<SelectWinnerRequest>
	{
		private readonly ILogger<SelectWinner> _logger;
		private readonly IRandomizer _randomizer;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepWinnersRepository _sweepWinnersRepository;
		private readonly ISweepsEntriesRepository _sweepEntriesRepository;
		private readonly ISweepDrawRuleRepository _drawRuleRepository;
		private readonly IMediator _mediator;
		private readonly Func<ExternalClientType, IExternalClient> _publisherResolver;
		private readonly IProfilesRepository _profilesRepository;

		public SelectWinner(
			ILoggerFactory loggerFactory, 
			IRandomizer randomizer, 
			ISweepsRepository sweepsRepository,
			ISweepWinnersRepository sweepWinnersRepository,
			ISweepsEntriesRepository sweepsEntriesRepository,
			ISweepDrawRuleRepository drawRuleRepository,
			IMediator mediator,
			Func<ExternalClientType, IExternalClient> publisherResolver,
			IProfilesRepository profilesRepository)
		{
			_logger = loggerFactory.CreateLogger<SelectWinner>();
			_randomizer = randomizer;
			_sweepWinnersRepository = sweepWinnersRepository;
			_sweepEntriesRepository = sweepsEntriesRepository;
			_sweepsRepository= sweepsRepository;
			_drawRuleRepository = drawRuleRepository;
			_mediator = mediator;
			_publisherResolver = publisherResolver;
			_profilesRepository = profilesRepository;
		}

		protected override async Task Handle(SelectWinnerRequest request, CancellationToken cancellationToken)
		{
			var totalEntrants = request.Entrants.Count;
			var entrantIndex = 1;
			var winner = default(SweepEntry);

			var sweep = await _sweepsRepository.GetAsync(request.SweepUid);
			var winners = await _sweepWinnersRepository.GetSweepWinnersAsync(request.SweepUid);
			var drawRules = await _drawRuleRepository.GetSweepDrawRuleAsync(request.SweepUid);
			_logger.LogInformation("There is a total of {totalEntrants} remaining in the entries for sweepstake {sweepName} and a total of {winnersCount} winners already selected.",
				totalEntrants,
				sweep.Name,
				winners.Count());

			if (request.SelectAllPossibleWinners)
			{
				while (totalEntrants > 0 && winners.Count() < drawRules.WinnerCount)
				{
					if (totalEntrants > 1)
					{
						entrantIndex = _randomizer.GetRandomNumber(1, totalEntrants);
					}

					var allEntrants = await _sweepEntriesRepository.GetSweepEntriesAsync(request.SweepUid);
					var distinctEntrants = allEntrants.GroupBy(c => c.UserIdentifier)
						.Select(g => g.First())
						.ToList();
					winners = await _sweepWinnersRepository.GetSweepWinnersAsync(request.SweepUid);
					foreach (var win in winners)
					{
						var toRemove = distinctEntrants.FirstOrDefault(c => c.SweepEntryUid == win.SweepEntry.SweepEntryUid);
						if(toRemove != null)
						{
							distinctEntrants.Remove(toRemove);
						}
					}

					winner = await PickndWinnerAsync(distinctEntrants, entrantIndex, sweep, winners);
					winners = await _sweepWinnersRepository.GetSweepWinnersAsync(request.SweepUid);

					await NotifyWinnerAsync(winner, sweep);

					var entryToRemove = distinctEntrants.FirstOrDefault(c => c.SweepEntryUid == winner.SweepEntryUid);

					distinctEntrants.Remove(entryToRemove);

					totalEntrants = distinctEntrants.Count;
				}
			}
			else
			{
				var allEntrants = await _sweepEntriesRepository.GetSweepEntriesAsync(request.SweepUid);
				var distinctEntrants = allEntrants.GroupBy(c => c.UserIdentifier)
					.Select(g => g.First())
					.ToList();

				if (totalEntrants > 1)
				{
					entrantIndex = _randomizer.GetRandomNumber(1, totalEntrants);
				}

				winner = await PickndWinnerAsync(distinctEntrants, entrantIndex, sweep, winners);
				await NotifyWinnerAsync(winner, sweep);
			}
		}

		private async Task NotifyWinnerAsync(SweepEntry winner, Sweep sweep)
		{
			var twitterPublisher = _publisherResolver(ExternalClientType.Twitter);
			var profile = await _profilesRepository.GetProfileByCampaignUid(sweep.Campaign.CampaignUid, ProfileType.Twitter);
			var message = $"{winner.UserIdentifier} - {DateTime.UtcNow.ToString("MMM dd, yyyy hh:mm:ss")} - You are a potential winner for the sweepstakes {sweep.Name} ran by @hrvsterHQ for @{profile.ScreenName}";
			var tweetId = await twitterPublisher.PublishAsync(new Message
			{
				Profile = profile,
				Text = message
			});

			var contactMessage = $"Please contact @{profile.ScreenName} about possible prizes and how they can be claimed.";
			tweetId = await twitterPublisher.PublishResponse(new Message
			{
				Profile = profile,
				Text = contactMessage
			}, tweetId);
		}

		private async Task<SweepEntry> PickndWinnerAsync(
			List<SweepEntry> entrants, 
			int entrantIndex, 
			Sweep sweep, 
			IEnumerable<SweepWinner> winners)
		{
			var winner = entrants[entrantIndex - 1];

			await SaveWinnerInformation(winner, sweep, winners);

			return winner;
		}

		private async Task SaveWinnerInformation(SweepEntry winner, Sweep sweep, IEnumerable<SweepWinner> winners)
		{
			await _sweepWinnersRepository.SaveSweepWinnerAsync(new Models.SweepWinner
			{
				AssignedId = winners.Count() + 1,
				SweepEntry = winner,
				SweepWinnerUid = Guid.NewGuid(),
				UserIdentifier = winner.UserIdentifier,
				UserIdentifierType = winner.UserIdentifierType,
				Sweep = new Models.Sweep
				{
					SweepId = sweep.SweepId
				}
			});
		}
	}
}

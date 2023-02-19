using Bogus;
using hrvster.Infrastructure;
using hrvster.Models;
using MediatR;

namespace SweepEntrySeeder.Requests
{
	public class SeedSweepstakeEntriesRequest : IRequest { }

	public class SeedSweepstakeEntries : AsyncRequestHandler<SeedSweepstakeEntriesRequest>
	{
		private readonly ILogger<SeedSweepstakeEntries> _logger;
		private readonly ISweepsEntriesRepository _sweepsEntriesRepository;
		private readonly ISweepsRepository _sweepsRepository;
		private readonly ISweepRulesRepository _sweepRulesRepository;
		private readonly SweepstakeSeederOptions _settings;

		public SeedSweepstakeEntries(
			ILoggerFactory loggerFactory,
			ISweepsEntriesRepository sweepsEntriesRepository,
			ISweepsRepository sweepsRepository,
			ISweepRulesRepository sweepRulesRepository,
			SweepstakeSeederOptions settings)
		{
			_logger = loggerFactory.CreateLogger<SeedSweepstakeEntries>();
			_sweepsEntriesRepository = sweepsEntriesRepository;
			_sweepsRepository = sweepsRepository;
			_sweepRulesRepository = sweepRulesRepository;
			_settings = settings;
		}

		protected async override Task Handle(SeedSweepstakeEntriesRequest request, CancellationToken cancellationToken)
		{
			var sweepUids = _settings.SweepstakeUids.Split(',').Select(Guid.Parse).ToList();

			foreach (var sweepUid in sweepUids)
			{
				var sweep = await _sweepsRepository.GetAsync(sweepUid);
				var rules = await _sweepRulesRepository.GetSweepRuleAsync(sweepUid);

				var rule = rules.FirstOrDefault();

				if (rule != null)
				{
					Randomizer.Seed = new Random(7552);

					var entryRules = new Faker<SweepEntry>()
						.RuleFor(s => s.Sweep, f => sweep)
						.RuleFor(s => s.Rule, f => rule)
						.RuleFor(s => s.AcceptedTerms, f => true)
						.RuleFor(s => s.SweepEntryUid, f => Guid.NewGuid())
						.RuleFor(s => s.UserIdentifier, f => f.Internet.UserName())
						.RuleFor(s => s.UserIdentifierType, f => UserIdentifierType.TwitterHandle);

					var entries = entryRules.Generate(_settings.TotalEntries);

					var tasks = new List<Task>();

					foreach (var entry in entries)
					{
						tasks.Add(_sweepsEntriesRepository.CreateAsync(entry));

						if(tasks.Count == 50)
						{
							await Task.WhenAll(tasks);
							tasks = new List<Task>();
						}
					}
				}
			}


		}
	}
}

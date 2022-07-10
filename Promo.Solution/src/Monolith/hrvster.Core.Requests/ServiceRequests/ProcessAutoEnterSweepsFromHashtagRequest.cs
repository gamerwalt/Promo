using hrvster.Models;
using hrvster.Models.Harvester;
using MediatR;
using System;
using System.Collections.Generic;
using Tweetinvi.Models;

namespace hrvster.Core.Requests.ServiceRequests
{
	public class ProcessAutoEnterSweepsFromHashtagRequest : IRequest
	{
		private IEnumerable<BlacklistedSweepUser> _blacklisted;
		private SweepEntryRule _entryRules;
		private ITweet _tweet;
		private Rule _rule;
		private Guid _userUid;
		private Guid _harvestedUid;
		private SweepEntryReport _sweepEntryReport;

		public ProcessAutoEnterSweepsFromHashtagRequest(IEnumerable<BlacklistedSweepUser> blacklisted, SweepEntryRule entryRules, ITweet tweet, Rule rule, Guid userUid, Guid harvestedUid, SweepEntryReport userReport)
		{
			_blacklisted = blacklisted;
			_entryRules = entryRules;
			_tweet = tweet;
			_rule = rule;
			_userUid = userUid;
			_harvestedUid = harvestedUid;
			_sweepEntryReport = userReport;
		}

		public IEnumerable<BlacklistedSweepUser> BlackListed { get { return _blacklisted; } }
		public SweepEntryRule EntryRules { get { return _entryRules; } }
		public ITweet Tweet { get { return _tweet; } }
		public Rule Rule { get { return _rule; } }
		public Guid UserUid { get { return _userUid; } }
		public Guid HarvestedUid { get { return _harvestedUid; } }
		public SweepEntryReport SweepEntryReport { get { return _sweepEntryReport; } }

	}
}

using hrvster.Infrastructure.Harvester;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Core.Requests.Harvester
{
	public class GetSweepsBlacklistedUsers : IRequest<IEnumerable<long>>
	{
		private Guid sweepUid;

		public GetSweepsBlacklistedUsers(Guid sweepUid)
		{
			this.sweepUid = sweepUid;
		}

		public Guid SweepUid
		{
			get { return sweepUid; }
		}
	}

	public class GetSweepBlacklistedUsersRequestHandler : IRequestHandler<GetSweepsBlacklistedUsers, IEnumerable<long>>
	{
		private readonly IBlacklistedSweepsUsersRepository _repository;
		private readonly ILogger<GetSweepBlacklistedUsersRequestHandler> _logger;

		public GetSweepBlacklistedUsersRequestHandler(ILoggerFactory loggerFactory, IBlacklistedSweepsUsersRepository repository)
		{
			_logger = loggerFactory.CreateLogger<GetSweepBlacklistedUsersRequestHandler>();
			_repository = repository;
		}

		public async Task<IEnumerable<long>> Handle(GetSweepsBlacklistedUsers request, CancellationToken cancellationToken)
		{
			_logger.LogInformation("Retrieving sweeps blacklisted users for sweep with uid: {sweepUid}.", request.SweepUid);

			var results = await _repository.GetBlacklistedSweepUsersAsync(request.SweepUid);

			_logger.LogInformation("Retrieved {blacklistCount} number of blacklisted users for sweep with uid: {sweepUid}", results.Count(), request.SweepUid);

			return results.ToList().Select(c => c.TweetUserId);
		}
	}
}

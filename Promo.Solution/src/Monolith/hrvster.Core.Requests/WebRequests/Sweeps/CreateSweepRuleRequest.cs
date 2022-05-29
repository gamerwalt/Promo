using MediatR;
using System;

namespace hrvster.Core.Requests.WebRequests.Sweeps
{
	public class CreateSweepRuleRequest : IRequest
	{
		private Guid sweepUid;
		private int sweepstakeId;
		private int watchTypeId;
		private string watchValue;
		private string tweetId;

		public CreateSweepRuleRequest(Guid sweepUid, int sweepstakeId, int watchTypeId, string watchValue, string tweetId)
		{
			this.sweepUid = sweepUid;
			this.sweepstakeId = sweepstakeId;
			this.watchTypeId = watchTypeId;
			this.watchValue = watchValue;
			this.tweetId = tweetId;
		}

		public Guid SweepstakeUid
		{
			get { return sweepUid; }
		}

		public int SweepstakeId
		{
			get { return sweepstakeId; }
		}

		public int WatchTypeId
		{
			get { return watchTypeId; }
		}

		public string WatchValue
		{
			get { return watchValue; }
		}

		public string TweetId
		{
			get { return tweetId; }
		}
	}
}

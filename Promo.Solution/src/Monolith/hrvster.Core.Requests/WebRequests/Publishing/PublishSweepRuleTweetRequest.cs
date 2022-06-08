using hrvster.Models;
using hrvster.Models.Profiles;
using MediatR;
using System;
using System.Collections.Generic;

namespace hrvster.Core.Requests.WebRequests.Publishing
{
	public class PublishSweepRuleTweetRequest : IRequest<bool>
	{
		private Profile _profile;
		private int _watchTypeId;
		private string _watchValue;
		private string _tweetText;
		private bool _postNow;
		private DateTime _scheduledDate;
		private Sweep _sweep;
		private IEnumerable<string> _mediaLocation;
		private bool _mustBeFollowing;

		public PublishSweepRuleTweetRequest(
			Sweep sweep,
			Profile profile,
			int watchTypeId,
			string watchValue,
			string tweetText,
			bool postNow,
			DateTime scheduledDate,
			bool mustBeFollowing,
			IEnumerable<string> mediaLocation)
		{
			_profile = profile;
			_watchTypeId = watchTypeId;
			_watchValue = watchValue;
			_tweetText = tweetText;
			_postNow = postNow;
			_scheduledDate = scheduledDate;
			_sweep = sweep;
			_mediaLocation = mediaLocation;
			_mustBeFollowing = mustBeFollowing;
		}

		public Profile Profile
		{
			get { return _profile; }
		}

		public int WatchTypeId
		{
			get { return _watchTypeId; }
		}

		public string WatchValue
		{
			get { return _watchValue; }
		}

		public string TweetText
		{
			get { return _tweetText; }
		}

		public bool PostNow
		{
			get { return _postNow; }
		}

		public DateTime ScheduledDate
		{
			get { return _scheduledDate; }
		}

		public Sweep Sweep
		{
			get { return _sweep; }
		}

		public IEnumerable<string> MediaLocation
		{
			get { return _mediaLocation; }
		}

		public bool MustBeFollowing
		{
			get { return _mustBeFollowing; }
		}
	}
}

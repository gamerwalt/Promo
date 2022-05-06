using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Notifications.Harvester
{
	public class UnableToEnterSweeps : INotification
	{
		private NewHashTagTweetReceived notification;
		private Campaign campaign;
		private Sweep sweep;

		public UnableToEnterSweeps(NewHashTagTweetReceived notification, Campaign campaign, Sweep sweep)
		{
			this.notification = notification;
			this.campaign = campaign;
			this.sweep = sweep;
		}

		public NewHashTagTweetReceived Notification
		{
			get { return notification; }
		}

		public Campaign Campaign
		{
			get { return campaign; }
		}

		public Sweep Sweep
		{
			get { return sweep; }
		}
	}
}

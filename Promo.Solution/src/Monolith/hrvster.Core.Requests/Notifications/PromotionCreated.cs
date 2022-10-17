using hrvster.Models;
using MediatR;

namespace hrvster.Core.Requests.Notifications
{
	public class PromotionCreated : INotification
	{
		private Campaign _model;

		public PromotionCreated(Campaign model)
		{
			_model = model;
		}

		public Campaign Campaign { get { return _model; } }
	}
}

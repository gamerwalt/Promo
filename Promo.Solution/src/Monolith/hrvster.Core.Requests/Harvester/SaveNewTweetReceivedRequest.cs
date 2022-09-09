using hrvster.Models.Harvester;
using MediatR;

namespace hrvster.Core.Requests.Harvester
{
	public class SaveNewTweetReceivedRequest : IRequest<IncomingTweet>
	{
		private string _json;

		public SaveNewTweetReceivedRequest(string json)
		{
			_json = json;
		}

		public string JsonTweet { get { return _json; } }
	}
}

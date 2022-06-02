using System;

namespace hrvster.Models.Core
{
	public class WatchTypeProvider
	{
		public int WatchTypeProviderId { get; set; }
		public Guid WatchTypeProviderUid { get; set; }
		public string WatchTypeProviderCode { get; set; }
		public string WatchTypeProviderName { get; set; }
		public bool ActiveInd { get; set; }
		public string ImageUrl { get; set; }
	}
}

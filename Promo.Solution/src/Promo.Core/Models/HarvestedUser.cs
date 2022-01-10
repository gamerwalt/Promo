using System;

namespace Promo.Core.Models
{
	public class HarvestedUser
	{
		public int UserId { get; set; }
		public Guid UserUid { get; set; }
		public string ExternalId { get; set; }
		public Uri ProfileImageUri { get; set; }
		public bool Verified { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }
		public string ScreenName { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}

using System;

namespace Promo.Games.Infrastructure.Models
{
	public class Account
	{
		public int AccountId { get; set; }
		public Guid AccountUid { get; set; }
		public string EmailAddress { get; set; }
		public string PhoneNumber { get; set; }
		public string DisplayName { get; set; }
	}
}

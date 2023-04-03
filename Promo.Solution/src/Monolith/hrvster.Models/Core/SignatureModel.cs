using System;

namespace hrvster.Models.Core
{
	public class SignatureModel
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public DateTime ExpiryDate { get; set; }
		public int GamePlayCount { get; set; }
		public string ExternalUserHandle { get; set; }
		public string ExternalUserId { get; set; }
		public Guid GameUid { get; set; }
		public string Link { get; set; }
		public string Signature { get; set; }
	}
}

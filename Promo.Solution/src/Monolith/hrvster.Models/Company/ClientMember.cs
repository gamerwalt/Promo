using System;

namespace hrvster.Models.Company
{
	public class ClientMember
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public Account Account { get; set; }
		public string ExternalId { get; set; }
		public string ExternalUserName { get; set; }
		public string ExternalAttributes { get; set; }
	}
}

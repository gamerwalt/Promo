using System;

namespace hrvster.Models.Company
{
	public class ClientMemberPoint
	{
		public Guid Uid { get; set; }
		public ClientMember ClientMember { get; set; }
		public int Points { get; set; }
		public string Activity { get; set; }
	}
}

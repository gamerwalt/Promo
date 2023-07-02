using System;

namespace hrvster.Models.Company
{
	public class AccountCompany
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public Account Account { get; set; }
		public string Name { get; set; }
		public string Subdomain { get; set; }
		public bool IsActive { get; set; }
	}
}

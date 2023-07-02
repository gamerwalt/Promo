using System;

namespace hrvster.Models.Company
{
	public class ClientImage
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public AccountCompany Company { get; set; }
		public string LogoLocation { get; set; }
		public string HeaderLocation { get; set; }
		public string FooterLocation { get; set; }
	}
}

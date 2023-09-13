using Microsoft.AspNetCore.Http;
using System;

namespace hrvster.Web.Models
{
	public class ViewClientPages
	{
		public Guid Uid { get; set; }
		public ClientImages Images { get; set; }
	}

	public class ClientImages
	{
		public string LogoLocation { get; set; }
		public string HeaderLocation { get; set; }
		public string FooterLocation { get; set; }
	}
}

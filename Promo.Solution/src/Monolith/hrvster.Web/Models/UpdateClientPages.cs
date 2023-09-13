using Microsoft.AspNetCore.Http;
using System;

namespace hrvster.Web.Models
{
	public class UpdateClientPages
	{
		public Guid Uid { get; set; }
		public IFormFile LogoLocation { get; set; }
		public IFormFile HeaderLocation { get; set; }
		public IFormFile FooterLocation { get; set; }
		public string PreviousLogoLocation { get; set; }
		public string PreviousHeaderLocation { get; set; }
		public string PreviousFooterLocation { get; set; }
	}
}

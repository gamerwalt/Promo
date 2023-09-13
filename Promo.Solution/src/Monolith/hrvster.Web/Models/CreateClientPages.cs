using Microsoft.AspNetCore.Http;

namespace hrvster.Web.Models
{
	public class CreateClientPages
	{
		public IFormFile LogoLocation { get; set; }
		public IFormFile HeaderLocation { get; set; }
		public IFormFile FooterLocation { get; set; }
	}
}

using Microsoft.AspNetCore.Http;

namespace hrvster.Web.Models
{
	public class UpdateMatchImage
	{
		public MatchImage MatchImage { get; set; }
		public MatchGame MatchGame { get; set; }
		public IFormFile Image { get; set; }
	}
}

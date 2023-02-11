using hrvster.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class CreateImage
	{
		public MatchGame MatchGame { get; set; }
		public Campaign Campaign { get; set; }
		public ICollection<IFormFile> Images { get; set; }
	}
}

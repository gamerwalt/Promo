using hrvster.Models;
using System.Collections.Generic;

namespace hrvster.Web.Models
{
	public class MatchGameImages
	{
		public IEnumerable<MatchImage> MatchImages { get; set; }
		public Campaign Campaign { get; internal set; }
		public MatchGame MatchGame { get; set; }
	}
}

using hrvster.Models.WebModels;
using System.Collections.Generic;
using Account = hrvster.Models.WebModels.Account;

namespace hrvster.Web.Models
{
	public class Dashboard
	{
		public Account Account { get; set; }
		public IEnumerable<Promotion> Promotions { get; set; }
	}
}

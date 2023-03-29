using Org.BouncyCastle.Bcpg.OpenPgp;
using System;

namespace hrvster.Web.Models
{
	public class MatchPrize
	{
		public Guid Uid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Value { get; set; }
		public string ImageUrl { get; set; }
		public int Quantity { get; set; }
	}
}

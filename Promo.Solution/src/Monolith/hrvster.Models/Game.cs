using System;

namespace hrvster.Models
{
	public class Game
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime StartDateUtc { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime EndDateUtc { get; set; }
	}
}

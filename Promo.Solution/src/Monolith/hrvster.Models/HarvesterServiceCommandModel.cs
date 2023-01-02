using System;

namespace hrvster.Models
{
	public class HarvesterServiceCommandModel
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string Command { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

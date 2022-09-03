using hrvster.Models;
using System;

namespace hrvster.Web.Models
{
	public class SweepEntry
	{
		public Guid SweepEntryUid { get; set; }
		public string UserIdentifier { get; set; }
		public UserIdentifierType UserIdentifierType { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

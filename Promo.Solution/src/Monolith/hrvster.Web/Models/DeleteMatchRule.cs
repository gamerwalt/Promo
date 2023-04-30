using System;

namespace hrvster.Web.Models
{
	public class DeleteMatchRule
	{
		public Guid RuleUid { get; set; }
		public Guid GameUid { get; set; }
		public string ExternalId { get; set; }
		public string ExternalClientType { get; set; }
	}
}

using hrvster.Models;

namespace hrvster.Template1.Web.Models
{
	public class CreateMember
	{
		public string EmailAddress { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string PhoneNumber { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string ExternalClient { get; set; }
		public Guid GameUid { get; set; }
		public Guid RuleUid { get; set; }
		public string GameName { get; set; }
		public DateTime GameStartDate { get; set; }
		public DateTime GameEndDate { get; set; }
	}
}

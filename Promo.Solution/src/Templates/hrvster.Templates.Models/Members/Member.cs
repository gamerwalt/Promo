namespace hrvster.Templates.Models.Members
{
	public class Member
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string EmailAddress { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string PhoneNumber { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public List<MemberProfile> Profiles { get; set; }
	}
}

namespace hrvster.Templates.Models.Members
{
	public class Member
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public string EmailAddress { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public List<MemberProfile> Profiles { get; set; }
	}
}

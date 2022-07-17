namespace hrvster.Web.Models
{
	public class Register
	{
		public string EmailAddress { get; set; }
		public string UserName { get; set; }
		public string PhoneNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Password { get; set; }
		public bool AcceptedTerms { get; set; }
	}
}

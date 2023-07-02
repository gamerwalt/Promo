namespace hrvster.Web.Models
{
	public class ViewClientPages
	{
		public ClientImages Images { get; set; }
	}

	public class ClientImages
	{
		public string LogoLocation { get; set; }
		public string HeaderLocation { get; set; }
		public string FooterLocation { get; set; }
	}
}

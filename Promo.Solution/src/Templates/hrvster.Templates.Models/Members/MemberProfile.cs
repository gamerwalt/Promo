using hrvster.Models;

namespace hrvster.Templates.Models.Members
{
	public class MemberProfile
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public Member Member { get; set; }
		public string ExternalProfileId { get; set; }
		public ProfileType ProfileType { get; set; }
		public string ScreenName { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string PictureUrl { get; set; }
		public string AccessToken { get; set; }
		public string? RefreshToken { get; set; }
	}
}

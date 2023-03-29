namespace hrvster.Games.Match.Models
{
	public class MatchPrize
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public MatchGame MatchGame { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Value { get; set; }
		public MatchImage Image { get; set; }
		public int Quantity { get; set; }
	}
}

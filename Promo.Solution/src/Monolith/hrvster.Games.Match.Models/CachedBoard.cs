using Newtonsoft.Json;

namespace hrvster.Games.Match.Models
{
	public class CachedBoard
	{ 
		[JsonProperty("Id")]
		public int Id { get; set; }

		[JsonProperty("Uid")]
		public Guid Uid { get; set; }

		[JsonProperty("IsWinnable")]
		public bool IsWinnable { get; set; }

		[JsonProperty("IsReserved")]
		public bool IsReserved { get; set; }

		[JsonProperty("PrizeId")]
		public int PrizeId { get; set; }

		[JsonProperty("PrizeUid")]
		public Guid PrizeUid { get; set; }

		[JsonProperty("MatchGameId")]
		public int MatchGameId { get; set; }

		[JsonProperty("MatchGameUid")]
		public Guid MatchGameUid { get; set; }

		[JsonProperty("MatchGameName")]
		public string MatchGameName { get; set; }
	}

	public class CachedTile
	{
		[JsonProperty("BoardUid")]
		public Guid BoardUid { get; set; }
		[JsonProperty("TileUid")]
		public Guid TileUid { get; set; }
		[JsonProperty("ImageId")]
		public int ImageId { get; set; }
		[JsonProperty("ImageUid")]
		public Guid ImageUid { get; set; }
		[JsonProperty("ImageUrl")]
		public string ImageUrl { get; set; }
		[JsonProperty("PrizeId")]
		public int PrizeId { get; set; }
		[JsonProperty("PrizeUid")]
		public Guid PrizeUid { get; set; }
		[JsonProperty("WinIndicator")]
		public bool WinIndicator { get; set; }
	}
}

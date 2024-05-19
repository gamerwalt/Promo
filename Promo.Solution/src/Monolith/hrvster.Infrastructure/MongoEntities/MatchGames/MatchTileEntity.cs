using hrvster.Games.Match.Models;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace hrvster.Infrastructure.MongoEntities.MatchGames
{
	public class MatchTileEntity
	{
		[BsonElement("uid")]
		public Guid Uid { get; set; }

		[BsonIgnoreIfDefault] // Optional, ignores the default value of the type
		public bool WinIndicator { get; set; }

		// Nested objects can also have Bson attributes if needed
		[BsonElement("image")]
		public MatchImage Image { get; set; }

		[BsonElement("board")]
		public MatchBoard Board { get; set; }

		[BsonElement("prize")]
		public MatchPrize Prize { get; set; }

		[BsonElement("random")]
		public Double Random { get; set; }
	}
}

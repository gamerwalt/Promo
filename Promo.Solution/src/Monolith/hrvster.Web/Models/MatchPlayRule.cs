using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using System;

namespace hrvster.Web.Models
{
	public class MatchPlayRule
	{
		public int Id { get; set; }
		public Guid Uid { get; set; }
		public int PlayPerGame { get; set; }
		public int PlayPerTweet { get; set; }
	}
}

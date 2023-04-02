using hrvster.Infrastructure.Extensions;
using MySqlConnector;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Core
{
	public interface ISignatureRepository
	{
		Task CreateAsync(Guid Uid, string signature, string link, DateTime expiryDate, Guid gameUid, string externalUserId, string externalUserHandle, int gamePlayCount, CancellationToken cancellationToken);
	}

	public class SignatureRepository : ISignatureRepository
	{
		private readonly IDatabase _database;
		public SignatureRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(
			Guid Uid,
			string signature,
			string link,
			DateTime expiryDate,
			Guid gameUid,
			string externalUserId,
			string externalUserHandle,
			int gamePlayCount,
			CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "signature_add";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("Uid", Uid)
				.AddParameter("signature", signature)
				.AddParameter("link", link)
				.AddParameter("expiryDate", expiryDate)
				.AddParameter("gameUid", gameUid)
				.AddParameter("externalUserId", externalUserId)
				.AddParameter("externalUserHandle", externalUserHandle)
				.AddParameter("gamePlayCount", gamePlayCount);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}
	}
}

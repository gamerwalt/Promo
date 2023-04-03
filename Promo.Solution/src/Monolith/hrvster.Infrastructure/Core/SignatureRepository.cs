using hrvster.Infrastructure.Extensions;
using hrvster.Models.Core;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Core
{
	public interface ISignatureRepository
	{
		Task CreateAsync(Guid Uid, string signature, string link, DateTime expiryDate, Guid gameUid, string externalUserId, string externalUserHandle, int gamePlayCount, CancellationToken cancellationToken);
		Task<SignatureModel> GetAsync(string gameUidSession, string signatureSession, string screenName);
		Task<SignatureModel> GetByUidAsync(Guid uid);
		Task UpdateSignatureAsync(SignatureModel signatureToUpdate, Guid uidToUpdate, int idToUpdate);
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

		public async Task UpdateSignatureAsync(SignatureModel signatureToUpdate, Guid uidToUpdate, int idToUpdate)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "signature_update";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("Uid", uidToUpdate)
				.AddParameter("id", idToUpdate)
				.AddParameter("newUid", signatureToUpdate.Uid)
				.AddParameter("signature", signatureToUpdate.Signature)
				.AddParameter("link", signatureToUpdate.Link)
				.AddParameter("expiryDate", signatureToUpdate.ExpiryDate)
				.AddParameter("gameUid", signatureToUpdate.GameUid)
				.AddParameter("externalUserId", signatureToUpdate.ExternalUserId)
				.AddParameter("externalUserHandle", signatureToUpdate.ExternalUserHandle)
				.AddParameter("gamePlayCount", signatureToUpdate.GamePlayCount);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<SignatureModel> GetAsync(string gameUidSession, string signatureSession, string screenName)
		{
			//signature_get
			var signature = default(SignatureModel);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "signature_get";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("gameUid", gameUidSession)
					.AddParameter("signature", signatureSession)
					.AddParameter("screenName", screenName);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					signature = CreateRecordFrom(reader);
				}
			}

			return signature;
		}

		public async Task<SignatureModel> GetByUidAsync(Guid uid)
		{
			var signature = default(SignatureModel);

			using (var conn = new MySqlConnection(_database.GetConnectionString()))
			{
				var command = conn.CreateCommand();
				command.CommandText = "signature_get_by_uid";
				command.CommandType = CommandType.StoredProcedure;
				command.AddParameter("uid", uid);

				await conn.OpenAsync();
				using var reader = await command.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					signature = CreateRecordFrom(reader);
				}
			}

			return signature;
		}

		private SignatureModel CreateRecordFrom(MySqlDataReader reader)
		{
			return new SignatureModel
			{
				Id = reader.GetInt32("id"),
				Uid = reader.GetGuid("uid"),
				ExpiryDate = reader.GetDateTime("expiry_date"),
				GamePlayCount = reader.GetInt32("game_play_count"),
				ExternalUserHandle = reader.GetString("external_user_handle"),
				ExternalUserId = reader.GetString("external_user_id"),
				GameUid = reader.GetGuid("game_uid"),
				Link = reader.GetString("link"),
				Signature = reader.GetString("signature")
			};
		}
	}
}

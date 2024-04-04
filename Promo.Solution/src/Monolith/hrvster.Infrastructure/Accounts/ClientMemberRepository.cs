using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using hrvster.Models.Company;
using MySqlConnector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.Infrastructure.Accounts
{
	public interface IClientMemberRepository
	{
		Task CreateAsync(ClientMember clientMember, CancellationToken cancellationToken);
		Task<bool> ClientMemberExistsAsync(ClientMember clientMember, CancellationToken cancellationToken);
		Task CreateClientMemberPoints(ClientMemberPoint clientMemberPoint);
		Task<IEnumerable<ClientMemberPoint>> GetClientMemberPointsAsync(string userId, Account account);
	}

	public class ClientMemberRepository : IClientMemberRepository
	{
		private readonly IDatabase _database;

		public ClientMemberRepository(IDatabase database)
		{
			_database = database;
		}

		public async Task CreateAsync(ClientMember clientMember, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "client_member_create";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("uid", clientMember.Uid)
				.AddParameter("accountId", clientMember.Account.AccountId)
				.AddParameter("externalId", clientMember.ExternalId)
				.AddParameter("externalUserName", clientMember.ExternalUserName)
				.AddParameter("externalAttributes", clientMember.ExternalAttributes);

			await conn.OpenAsync(cancellationToken);
			await command.ExecuteNonQueryAsync(cancellationToken);
		}

		public async Task<bool> ClientMemberExistsAsync(ClientMember clientMember, CancellationToken cancellationToken)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "client_member_exists";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("accountId", clientMember.Account.AccountId)
				.AddParameter("externalId", clientMember.ExternalId);

			conn.Open();
			using var reader = command.ExecuteReader();

			return reader.HasRows;
		}

		public async Task CreateClientMemberPoints(ClientMemberPoint clientMemberPoint)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "client_member_points_create";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("uid", clientMemberPoint.Uid)
				.AddParameter("accountId", clientMemberPoint.ClientMember.Account.AccountId)
				.AddParameter("externalId", clientMemberPoint.ClientMember.ExternalId)
				.AddParameter("points", clientMemberPoint.Points)
				.AddParameter("activity", clientMemberPoint.Activity);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<ClientMemberPoint>> GetClientMemberPointsAsync(string userId, Account account)
		{
			var clientMemberPoints = new List<ClientMemberPoint>();

			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "client_member_points_get";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("accountId", account.AccountId)
				.AddParameter("externalId", userId);

			await conn.OpenAsync();
			using var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				clientMemberPoints.Add(CreateRecordFrom(reader));
			}

			return clientMemberPoints;
		}

		private ClientMemberPoint CreateRecordFrom(MySqlDataReader reader)
		{
			return new ClientMemberPoint
			{
				Uid = reader.GetGuid("client_member_point_uid"),
				Activity = reader.GetStringValue("activity"),
				Points = reader.GetInt32("points")
			};
		}
	}
}

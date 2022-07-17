using hrvster.Infrastructure.Extensions;
using hrvster.Models;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace hrvster.Infrastructure
{
	public interface IMailRepository
	{
		Task CreateAsync(Email email);
		Task<IEnumerable<Email>> GetUnsentAsync();
		Task UpdateAsSentAsync(Email email);
	}

	public class MailRepository : IMailRepository
	{
		private readonly IDatabase _database;
		public MailRepository(IDatabase database)
		{
			_database = database;
		}
		public async Task CreateAsync(Email email)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "mail_create";
			command.CommandType = CommandType.StoredProcedure;
			command
				.AddParameter("email_uid", email.EmailUid)
				.AddParameter("email_address_to", email.EmailAddress)
				.AddParameter("parameters", JsonConvert.SerializeObject(email.Parameters))
				.AddParameter("template_name", email.Template.ToString())
				.AddParameter("status", email.Status.ToString())
				.AddParameter("sent", email.Sent);

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task UpdateAsSentAsync(Email email)
		{
			using var conn = new MySqlConnection(_database.GetConnectionString());
			var command = conn.CreateCommand();
			command.CommandText = "mail_update_as_sent";
			command.CommandType = CommandType.StoredProcedure;
			command.AddParameter("email_uid", email.EmailUid)
				.AddParameter("status", EmailStatus.Sent.ToString());

			await conn.OpenAsync();
			await command.ExecuteNonQueryAsync();
		}

		public async Task<IEnumerable<Email>> GetUnsentAsync()
		{
			var emails = new List<Email>();

			using var conn = new MySqlConnection(_database.GetConnectionString());
			{
				var command = conn.CreateCommand();
				command.CommandText = "mail_get_not_sent";
				command.CommandType = CommandType.StoredProcedure;

				await conn.OpenAsync();
				using (var reader = await command.ExecuteReaderAsync())
				{
					while (await reader.ReadAsync())
					{
						emails.Add(CreateEmailFrom(reader));
					}
				}
			}

			return emails;
		}

		private Email CreateEmailFrom(MySqlDataReader reader)
		{
			return new Email
			{
				Id = reader.GetInt32("email_id"),
				EmailUid = reader.GetGuid("email_uid"),
				EmailAddress = reader.GetStringValue("email_address_to"),
				Parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.GetStringValue("parameters")),
				Template = (EmailTemplate)Enum.Parse(typeof(EmailTemplate), reader.GetStringValue("template_name")),
				Status = (EmailStatus)Enum.Parse(typeof(EmailStatus), reader.GetStringValue("status")),
				Sent = reader.GetBoolean("sent"),
				CreatedDate = reader.GetNullableDateTimeValue("created_date").GetValueOrDefault()
			};
		}
	}
}

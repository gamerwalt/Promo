using MySqlConnector;
using Newtonsoft.Json;
using Promo.Core.Extensions;
using Promo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Promo.Core.Repositories.MySqlRepositories
{
    public interface IMailRepository
    {
        Task CreateAsync(Email email);
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
    }
}

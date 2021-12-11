using Promo.Core.Repositories.MySqlRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Email.Service
{
    public interface IMailService
    {
        Task ProcessAnyUnsentEmails(CancellationToken token);
    }

    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;

        public MailService(IMailRepository mailRepository)
        {
            _mailRepository = mailRepository;
        }

        public Task ProcessAnyUnsentEmails(CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}

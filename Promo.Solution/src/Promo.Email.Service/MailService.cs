using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Promo.Core.Repositories.MySqlRepositories;
using Promo.Email.Service.TemplateMailRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private readonly MailSettings _mailSettings;

        public MailService(IMailRepository mailRepository, IOptions<MailSettings> mailSettingsOptions)
        {
            _mailRepository = mailRepository;
            _mailSettings = mailSettingsOptions.Value;
        }

        public async Task ProcessAnyUnsentEmails(CancellationToken token)
        {
            var emailsToGoOut = await _mailRepository.GetUnsentAsync();
            if(emailsToGoOut != null && emailsToGoOut.Count() != 0)
            {
                foreach(var email in emailsToGoOut)
                {
                    switch(email.Template)
                    {
                        case Models.EmailTemplate.VerificationEmail:
                            await SendVerificationEmailAsync(new VerificationEmailRequest
                            {
                                EmailAddress = email.EmailAddress,
                                EmailTemplate = Models.EmailTemplate.VerificationEmail,
                                DisplayName = email.Parameters["DisplayName"],
                                VerificationLink = email.Parameters["VerificationLink"]
                            });

                            await _mailRepository.UpdateAsSentAsync(email);

                            break;
                    }
                }
            }
        }

        private async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmailAddress));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        private async Task SendVerificationEmailAsync(VerificationEmailRequest request)
        {
            var filePath = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{request.EmailTemplate.ToString()}.html";
            var streamReader = new StreamReader(filePath);
            var mailText = streamReader.ReadToEnd();
            streamReader.Close();

            Type type = request.GetType();

            foreach (var f in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                mailText = mailText.Replace($"[{f.Name}]", f.GetValue(request).ToString());
            }

            await SendEmailAsync(
                new MailRequest
                {
                    ToEmailAddress = request.EmailAddress,
                    Body = mailText,
                    Subject = "Please verify your email address."
                }
            );
        }
    }
}

using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Promo.Core;
using Promo.Email.Service.TemplateMailRequest;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Promo.Email.Service.Services
{
    public interface IMailService
    {
        Task ProcessAnyUnsentEmails(CancellationToken token);
    }

    public class MailService : IMailService
    {
        private ILogger<MailService> _logger;
        private readonly IMailRepository _mailRepository;
        private readonly MailSettings _mailSettings;

        public MailService(
            ILogger<MailService> logger,
            IMailRepository mailRepository,
            IOptions<MailSettings> mailSettingsOptions
            )
        {
            _logger = logger;
            _mailRepository = mailRepository;
            _mailSettings = mailSettingsOptions.Value;
        }

        public async Task ProcessAnyUnsentEmails(CancellationToken token)
        {
            _logger.LogInformation($"{nameof(ProcessAnyUnsentEmails)} processing started.");

            var emailsToGoOut = await _mailRepository.GetUnsentAsync();
            if (emailsToGoOut != null && emailsToGoOut.Count() != 0)
            {
                _logger.LogInformation($"There are {emailsToGoOut.Count()} emails that needs to be sent.");
                foreach (var email in emailsToGoOut)
                {
                    switch (email.Template)
                    {
                        case EmailTemplate.VerificationEmail:
                            await SendVerificationEmailAsync(new VerificationEmailRequest
                            {
                                EmailAddress = email.EmailAddress,
                                EmailTemplate = EmailTemplate.VerificationEmail,
                                DisplayName = email.Parameters["DisplayName"],
                                VerificationLink = email.Parameters["VerificationLink"]
                            });

                            await _mailRepository.UpdateAsSentAsync(email);

                            break;

                        case EmailTemplate.WelcomeEmail:
                            await SendWelcomeEmailAsync(new WelcomeEmailRequest
                            {
                                EmailAddress = email.EmailAddress,
                                EmailTemplate = EmailTemplate.WelcomeEmail,
                                DisplayName = email.Parameters["DisplayName"],
                                DashboardLink = email.Parameters["DashboardLink"]
                            });

                            await _mailRepository.UpdateAsSentAsync(email);

                            break;
                    }
                }
            }

            _logger.LogInformation($"{nameof(ProcessAnyUnsentEmails)} processing completed.");
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
            var filePath = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{request.EmailTemplate}.html";
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

        private async Task SendWelcomeEmailAsync(WelcomeEmailRequest request)
        {
            var filePath = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{request.EmailTemplate}.html";
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
                    Subject = "Let's get you started!"
                }
            );
        }
    }
}

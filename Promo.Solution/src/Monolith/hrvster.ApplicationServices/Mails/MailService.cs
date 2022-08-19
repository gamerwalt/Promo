using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Models.TemplateMailRequest;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace hrvster.ApplicationServices.Mails
{
	public interface IMailService
	{
		Task ProcessAnyUnsentEmails(CancellationToken token);
	}

	public class MailService : IMailService
	{
		private ILogger<MailService> _logger;
		private readonly IMailRepository _mailRepository;
		private readonly SendGridOptions _sendGrid;

		public MailService(
			ILogger<MailService> logger,
			IMailRepository mailRepository,
			SendGridOptions sendGrid)
		{
			_logger = logger;
			_mailRepository = mailRepository;
			_sendGrid = sendGrid;
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
							var sendVerificationEmailResult = await SendVerificationEmailAsync(new VerificationEmailRequest
							{
								EmailAddress = email.EmailAddress,
								EmailTemplate = EmailTemplate.VerificationEmail,
								DisplayName = email.Parameters.ContainsKey(EmailParameters.DisplayName) 
									? email.Parameters[EmailParameters.DisplayName] 
									: String.Empty,
								VerificationLink = email.Parameters[EmailParameters.VerificationLink]
							});

							if(sendVerificationEmailResult)
								await _mailRepository.UpdateAsSentAsync(email);

							break;

						case EmailTemplate.WelcomeEmail:
							var sendWelcomeEmailResult = await SendWelcomeEmailAsync(new WelcomeEmailRequest
							{
								EmailAddress = email.EmailAddress,
								EmailTemplate = EmailTemplate.WelcomeEmail,
								DisplayName = email.Parameters[EmailParameters.DisplayName],
								DashboardLink = email.Parameters[EmailParameters.DashboardLink]
							});

							if(sendWelcomeEmailResult)
								await _mailRepository.UpdateAsSentAsync(email);

							break;

						case EmailTemplate.PasswordResetEmail:
							var sendPasswordResetResult = await SendPasswordResetEmailLinkAsync(new PasswordResetRequest
							{
								EmailAddress = email.EmailAddress,
								PasswordResetLink = email.Parameters[EmailParameters.PasswordResetLink],
								EmailTemplate = EmailTemplate.PasswordResetEmail
							});

							if(sendPasswordResetResult)
								await _mailRepository.UpdateAsSentAsync(email);

							break;
					}
				}
			}

			_logger.LogInformation($"{nameof(ProcessAnyUnsentEmails)} processing completed.");
		}

		

		private async Task<bool> SendEmailAsync(MailRequest mailRequest)
		{
			var client = new SendGridClient(_sendGrid.ApiKey);
			var msg = MailHelper.CreateSingleEmail(
				new EmailAddress(_sendGrid.Mail, _sendGrid.DisplayName), 
				new EmailAddress(mailRequest.ToEmailAddress),
				mailRequest.Subject, 
				mailRequest.Body, 
				mailRequest.Body);
			
			var response = await client.SendEmailAsync(msg);

			return response.IsSuccessStatusCode;
		}

		private async Task<bool> SendPasswordResetEmailLinkAsync(PasswordResetRequest request)
		{
			var filePath = GetFilePath(request);

			var streamReader = new StreamReader(filePath);
			var mailText = streamReader.ReadToEnd();
			streamReader.Close();

			var type = request.GetType();

			foreach(var f in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
			{
				mailText = mailText.Replace($"[{f.Name}]", f.GetValue(request).ToString());
			}

			return await SendEmailAsync(
				new MailRequest
				{
					ToEmailAddress = request.EmailAddress,
					Body = mailText,
					Subject = "Reset your password!"
				}
			);
		}

		private static string GetFilePath(PasswordResetRequest request)
		{
			return Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{request.EmailTemplate}.html";
		}

		private async Task<bool> SendVerificationEmailAsync(VerificationEmailRequest request)
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

			return await SendEmailAsync(
				new MailRequest
				{
					ToEmailAddress = request.EmailAddress,
					Body = mailText,
					Subject = "Please verify your email address."
				}
			);
		}

		private async Task<bool> SendWelcomeEmailAsync(WelcomeEmailRequest request)
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

			return await SendEmailAsync(
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

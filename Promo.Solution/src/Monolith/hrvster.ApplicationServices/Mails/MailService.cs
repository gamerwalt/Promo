using hrvster.Infrastructure;
using hrvster.Models;
using hrvster.Models.Options;
using hrvster.Models.TemplateMailRequest;
using Microsoft.Extensions.Logging;
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
						case EmailTemplate.NotEnoughSweepstakeDistinctUsers:
							var sendDistinctUsersNotEnoughResult = await SendSweepsDistinctUsersNotEnoughEmailAsync(new SendSweepsDistinctUsersNotEnoughEmailRequest
							{
								EmailTemplate = EmailTemplate.NotEnoughSweepstakeDistinctUsers,
								EmailAddress = email.Parameters[EmailParameters.EmailAddress],
								SweepstakeName = email.Parameters[EmailParameters.SweepstakeName]
							});

							if (sendDistinctUsersNotEnoughResult)
								await _mailRepository.UpdateAsSentAsync(email);

							break;
						case EmailTemplate.SweepEntriesNotEnoughEmail:
							var sendEntriesNotEnough = await SendEntriesNotEnoughEmailAsync(
								new SweepEntriesNotEnoughRequest
								{
									EmailTemplate = EmailTemplate.SweepEntriesNotEnoughEmail,
									EmailAddress = email.Parameters[EmailParameters.EmailAddress],
									SweepstakeName = email.Parameters[EmailParameters.SweepstakeName]
								}
							);

							if (sendEntriesNotEnough)
								await _mailRepository.UpdateAsSentAsync(email);

							break;
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

							if (sendVerificationEmailResult)
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

							if (sendWelcomeEmailResult)
								await _mailRepository.UpdateAsSentAsync(email);

							break;

						case EmailTemplate.PasswordResetEmail:
							var sendPasswordResetResult = await SendPasswordResetEmailLinkAsync(new PasswordResetRequest
							{
								EmailAddress = email.EmailAddress,
								PasswordResetLink = email.Parameters[EmailParameters.PasswordResetLink],
								EmailTemplate = EmailTemplate.PasswordResetEmail
							});

							if (sendPasswordResetResult)
								await _mailRepository.UpdateAsSentAsync(email);

							break;

						case EmailTemplate.EmailAddressUpdateRequest:

							var result = await SendEmailAddressEmailAsync(new EmailAddressUpdateRequest
							{
								EmailTemplate = EmailTemplate.EmailAddressUpdateRequest,
								EmailAddress = email.EmailAddress,
								DisplayName = email.Parameters[EmailParameters.DisplayName],
								SupportLink = email.Parameters[EmailParameters.SupportLink],
								OldEmailAddress = email.Parameters[EmailParameters.OldEmailAddress],
								NewEmailAddress = email.Parameters[EmailParameters.NewEmailAddress]
							});

							if (result)
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

		private async Task<bool> SendEmailAddressEmailAsync(EmailAddressUpdateRequest request)
		{
			var mailText = ExtractMailText(request);

			return await SendEmailAsync(
				new MailRequest
				{
					ToEmailAddress = request.EmailAddress,
					Body = mailText,
					Subject = "Email address update requested"
				}
			);
		}

		private static string ExtractMailText(EmailRequest request)
		{
			var mailText = GetMailText(request.EmailTemplate);

			var type = request.GetType();

			foreach (var f in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
			{
				mailText = mailText.Replace($"[{f.Name}]", f.GetValue(request).ToString());
			}

			return mailText;
		}

		private async Task<bool> SendSweepsDistinctUsersNotEnoughEmailAsync(
			SendSweepsDistinctUsersNotEnoughEmailRequest request)
		{
			var mailText = GetMailText(request.EmailTemplate);

			var type = request.GetType();

			foreach (var f in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
			{
				mailText = mailText.Replace($"[{f.Name}]", f.GetValue(request).ToString());
			}

			return await SendEmailAsync(
				new MailRequest
				{
					ToEmailAddress = request.EmailAddress,
					Body = mailText,
					Subject = $"Unable to draw a winner for {request.SweepstakeName}"
				}
			);
		}

		private async Task<bool> SendPasswordResetEmailLinkAsync(PasswordResetRequest request)
		{
			var mailText = GetMailText(request.EmailTemplate);

			var type = request.GetType();

			foreach (var f in type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
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

		private async Task<bool> SendEntriesNotEnoughEmailAsync(SweepEntriesNotEnoughRequest request)
		{
			var mailText = GetMailText(request.EmailTemplate);

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
					Subject = "Sweepstakes ready for draw - No Entries Available."
				});
		}

		private async Task<bool> SendVerificationEmailAsync(VerificationEmailRequest request)
		{
			var mailText = GetMailText(request.EmailTemplate);

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
			var mailText = GetMailText(request.EmailTemplate);

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

		private static string GetMailText(EmailTemplate template)
		{
			var filePath = Directory.GetCurrentDirectory() + $"{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}{template}.html";

			var streamReader = new StreamReader(filePath);

			var mailText = streamReader.ReadToEnd();

			streamReader.Close();

			return mailText;
		}
	}
}

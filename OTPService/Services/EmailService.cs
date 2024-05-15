using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using OTPService.Models;

namespace OTPService.Services
{
	public class EmailService : IEmailService
	{
		private readonly EmailConfig _emailConfig;

		public EmailService(EmailConfig emailConfig)
		{
			_emailConfig = emailConfig;
		}

		public void SendEmail(Mail mail)
		{
			var emailMessage = CreateEmail(mail);
			SendUsingMailKit(emailMessage);
		}
		private MimeMessage CreateEmail(Mail mail)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_emailConfig.From));
			email.To.AddRange(mail.Receivers);
			email.Subject = mail.Subject;
			email.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = mail.Body };
			return email;
		}
		private void SendUsingMailKit(MimeMessage emailMessage)
		{
			using var smtp = new SmtpClient();
			try
			{
				smtp.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
				smtp.Authenticate(_emailConfig.Username, _emailConfig.Password);
				smtp.Send(emailMessage);
			}
			catch (Exception ex)
			{
				throw;
			}
			finally 
			{ 
				smtp.Disconnect(true);
				smtp.Dispose(); 
			}
		}
	}
}

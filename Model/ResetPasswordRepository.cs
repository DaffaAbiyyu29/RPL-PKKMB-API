using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace PKKMB_API.Model
{
	public class ResetPasswordRepository
	{
		private readonly IConfiguration _configuration;

		public ResetPasswordRepository(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public bool SendEmail(EmailModel emailData)
		{
			try
			{
				var emailConfig = _configuration.GetSection("MailSettings").Get<EmailSettingModel>();

				var message = new MimeMessage();
				message.From.Add(new MailboxAddress(emailConfig.SenderEmail, "rakaalfawaz@gmail.com")); // Ganti "Your Name" sesuai kebutuhan
				message.To.Add(new MailboxAddress(emailData.RecipientEmail, "daffaabiyyu04@gmail.com")); // Ganti "Recipient Name" sesuai kebutuhan
				message.Subject = emailData.Subject;

				message.Body = new TextPart("html")
				{
					Text = emailData.Content
				};

				using (var client = new SmtpClient())
				{
					client.Connect(emailConfig.SmtpServer, emailConfig.SmtpPort, SecureSocketOptions.StartTls);

					// Note: since we don't have an OAuth2 token, disable
					// the XOAUTH2 authentication mechanism.
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					client.Authenticate(emailConfig.SmtpUsername, emailConfig.SmtpPassword);

					client.Send(message);
					client.Disconnect(true);
				}

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
		}
	}
}

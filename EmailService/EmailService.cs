using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace PKKMB_API.EmailService
{
	public class EmailService : IEmailService
	{
		private readonly EmailSettings emailSettings;

		public EmailService(IOptions<EmailSettings> options)
		{
			this.emailSettings = options.Value;
		}

		public async Task SendEmailAsync(Mailrequest mailrequest)
		{
			var email = new MimeMessage();
			email.Sender = MailboxAddress.Parse(emailSettings.Email);
			email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
			email.Subject = mailrequest.Subject;

			var builder = new BodyBuilder();
			builder.HtmlBody = mailrequest.Body;
			email.Body = builder.ToMessageBody();

			using (var smtp = new SmtpClient())
			{
				smtp.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

				try
				{
					smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
					smtp.Authenticate(emailSettings.Email, emailSettings.Password);
					await smtp.SendAsync(email);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"SMTP Error: {ex}");
				}
				finally
				{
					smtp.Disconnect(true);
				}
			}
		}
	}
}

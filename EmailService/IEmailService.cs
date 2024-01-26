namespace PKKMB_API.EmailService
{
	public interface IEmailService
	{
		Task SendEmailAsync(Mailrequest mailrequest);
	}
}

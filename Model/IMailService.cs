namespace PKKMB_API.Model
{
	public interface IMailService
	{
		Task SendEmailAsync(EmailModel emailRequest);
	}
}

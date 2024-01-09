using MailKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKKMB_API.Model;
using IMailService = PKKMB_API.Model.IMailService;

namespace PKKMB_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResetPasswordController : ControllerBase
	{
		private readonly ResetPasswordRepository _emailRepository;

		public ResetPasswordController(IConfiguration configuration)
		{
			_emailRepository = new ResetPasswordRepository(configuration);
		}

		[HttpPost("send")]
		public IActionResult SendEmail([FromBody] EmailModel emailData)
		{
			try
			{
				bool isEmailSent = _emailRepository.SendEmail(emailData);

				if (isEmailSent)
				{
					return Ok(new { Status = 200, Message = "Email successfully sent." });
				}
				return Ok(new { Status = 200, Message = isEmailSent });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Status = 500, Message = $"Error: {ex.Message}" });
			}
		}
	}
}

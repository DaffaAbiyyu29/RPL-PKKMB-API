using IdentityServer4.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PKKMB_API.EmailService;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResetPasswordController : ControllerBase
	{
		private readonly MahasiswaBaruRepository _mhsBaruRepo;
		private readonly PanitiaKesekretariatanRepository _kskRepo;
		private readonly PicPkkmbRepository _picRepo;
		private readonly LoginRepository _loginRepo;
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IEmailService emailService;
		public ResetPasswordController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
		{
			_mhsBaruRepo = new MahasiswaBaruRepository(configuration);
			_kskRepo = new PanitiaKesekretariatanRepository(configuration);
			_picRepo = new PicPkkmbRepository(configuration);
			_loginRepo = new LoginRepository(configuration);
			_configuration = configuration;
			_webHostEnvironment = webHostEnvironment;
			this.emailService = emailService;
		}

		[HttpGet("/api/images/{imageName}")]
		public IActionResult GetImage(string imageName)
		{
			string filePath = Path.Combine("Gambar", imageName);

			if (imageName != null)
			{
				if (System.IO.File.Exists(filePath))
				{
					var imageFileStream = System.IO.File.OpenRead(filePath);
					return File(imageFileStream, "image/png");
				}
				else
				{
					return NotFound();
				}
			}
			return NotFound();
		}

		[HttpPost("/SendEmail", Name = "SendEmail")]
		public async Task<IActionResult> SendEmail([FromHeader] string token, string email)
		{
			try
			{
				Mailrequest mailrequest = new Mailrequest();
				mailrequest.ToEmail = email;
				mailrequest.Subject = "PKKMB Politeknik Astra";
				mailrequest.Body = GetHtmlContent(token);
				await emailService.SendEmailAsync(mailrequest);
				string message = "Link Untuk Pemulihan Kata Sandi Telah Terkirim Pada Email Anda.";

				return Ok(new { message });
			}
			catch (SmtpProtocolException ex)
			{
				throw;
			}
		}

		private string GetHtmlContent(string token)
		{
			string Response = "<div style=\"width:100%;background-color:#ffffff;text-align:center;margin:10px\">";
			Response += "<h1>PKKMB Politeknik Astra</h1>";
			/*Response += "<img src=\"https://localhost:7138/api/images/logo_astratech.png\r\n\" width=\"100\" height=\"100\">\r\n";*/
			Response += "<img src=\"https://localhost:7138/api/images/logo_astratech.png\" width=\"100\" height=\"100\" />";
			Response += "<h2>Pemulihan Kata Sandi</h2>";
			/*Response += "<a href=\"https://localhost:7087/ValidateKodeVerifikasiEmail?token=" + token + "\">Klik Link Untuk Memulihkan Kata Sandi</a>";*/
			Response += "<a href=\"https://localhost:7240/Akun/ResetKataSandi\">Klik Link Untuk Memulihkan Kata Sandi</a>";
			Response += "</div>";
			return Response;
		}

		[HttpPost("/GenerateKodeVerifikasiEmail", Name = "GenerateKodeVerifikasiEmail")]
		//[AllowAnonymous]
		public IActionResult GenerateKodeVerifikasiEmail(string email)
		{
			MahasiswaBaruModel mhsBaru = _mhsBaruRepo.getMahasiswaByEmail(email);
			PanitiaKesekretariatanModel ksk = _kskRepo.getKskByEmail(email);

			try
			{
				if (mhsBaru != null && ksk == null)
				{
					var token = _loginRepo.GenerateJwtToken(mhsBaru);

					Response.Cookies.Append("VerifyEmail", token, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.None,
						Expires = DateTime.Now.AddMinutes(10)
					});

					return Ok(new { Status = 200, Messages = "Kode Verifikasi Berhasil Terkirim. Silakan Cek Email Anda.", Token = token });
				}
				else if (mhsBaru == null && ksk != null)
				{
					var token = _loginRepo.GenerateJwtToken(ksk);

					Response.Cookies.Append("VerifyEmail", token, new CookieOptions
					{
						HttpOnly = true,
						Secure = true,
						SameSite = SameSiteMode.None,
						Expires = DateTime.Now.AddMinutes(10)
					});

					return Ok(new { Status = 200, Messages = "Kode Verifikasi Berhasil Terkirim. Silakan Cek Email Anda.", Token = token });
				}
				else
				{
					// Account not found
					return NotFound(new { Status = 404, Messages = "Akun Tidak Ditemukan", Data = new Object() });
				}
			}
			catch (Exception ex)
			{
				// General error
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Mengirim Kode Verifikasi = " + ex.Message, Data = new Object() });
			}
		}

		[HttpGet("/ValidateKodeVerifikasiEmail", Name = "ValidateKodeVerifikasiEmail")]
		public IActionResult ValidateKodeVerifikasiEmail(string token)
		{
			if (token == null)
			{
				return BadRequest(new { Status = 401, Message = "Token is missing or empty." });
			}

			bool isValid = _loginRepo.ValidateJwtToken(token);

			if (isValid)
			{
				return Ok(new { Status = 200, Message = "Kode Verifikasi Valid." });
			}
			else
			{
				return Unauthorized(new { Status = 401, Message = "Kode Verifikasi Tidak Valid." });
			}
		}
	}
}

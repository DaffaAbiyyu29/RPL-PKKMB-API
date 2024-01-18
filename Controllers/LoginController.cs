using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PKKMB_API.Model;

namespace PKKMB_API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LoginController : ControllerBase
	{
		private readonly MahasiswaBaruRepository _mhsBaruRepo;
		private readonly PanitiaKesekretariatanRepository _kskRepo;
		private readonly PicPkkmbRepository _picRepo;
		private readonly LoginRepository _loginRepo;
		private readonly IConfiguration _configuration;

		public LoginController(IConfiguration configuration)
		{
			_mhsBaruRepo = new MahasiswaBaruRepository(configuration);
			_kskRepo = new PanitiaKesekretariatanRepository(configuration);
			_picRepo = new PicPkkmbRepository(configuration);
			_loginRepo = new LoginRepository(configuration);
			_configuration = configuration;
		}


		[HttpPost("/loginAkun", Name = "loginAkun")]
		//[AllowAnonymous]
		public IActionResult loginAkun([FromBody] LoginModel login)
		{
			MahasiswaBaruModel mhsBaru = _loginRepo.loginMahasiswa(login.username, login.password);
			PanitiaKesekretariatanModel ksk = _loginRepo.loginKSK(login.username, login.password);
			PicPkkmbModel pic = _loginRepo.loginPIC(login.username, login.password);
			try
			{
				if (mhsBaru != null && ksk == null && pic == null)
				{
					if (mhsBaru.mhs_password != null)
					{
						// login.password is correct, login successful
						var token = _loginRepo.GenerateJwtToken(mhsBaru);

						// Set JWT token in cookies
						Response.Cookies.Append("token", token, new CookieOptions
						{
							HttpOnly = true,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						Response.Cookies.Append("Role", "Mahasiswa", new CookieOptions
						{
							HttpOnly = false,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						return Ok(new { Status = 200, Messages = "Login berhasil", Data = mhsBaru, Role = "Mahasiswa", Token = token });
					}
					else
					{
						// login.password is incorrect
						return Unauthorized(new { Status = 401, Messages = "Kata Sandi Salah", Data = new Object() });
					}
				}
				else if (mhsBaru == null && ksk != null && pic == null)
				{
					if (ksk.ksk_password != null)
					{
						// login.password is correct, login successful
						var token = _loginRepo.GenerateJwtToken(ksk);

						// Set JWT token in cookies
						Response.Cookies.Append("token", token, new CookieOptions
						{
							HttpOnly = true,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						Response.Cookies.Append("Role", "Panitia Kesekretariatan", new CookieOptions
						{
							HttpOnly = false,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						return Ok(new { Status = 200, Messages = "Login berhasil", Data = ksk, Role = "Panitia Kesekretariatan", Token = token });
					}
					else
					{
						// login.password is incorrect
						return Unauthorized(new { Status = 401, Messages = "Kata Sandi Salah", Data = new Object() });
					}
				}
				else if (mhsBaru == null && ksk == null && pic != null)
				{
					if (pic.pic_password != null)
					{
						// login.password is correct, login successful
						var token = _loginRepo.GenerateJwtToken(pic);

						// Set JWT token in cookies
						Response.Cookies.Append("token", token, new CookieOptions
						{
							HttpOnly = true,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						Response.Cookies.Append("Role", "PIC PKKMB", new CookieOptions
						{
							HttpOnly = false,
							Secure = true,  // Set to true in a production environment (requires HTTPS)
							SameSite = SameSiteMode.None,  // Adjust according to your needs
							Expires = DateTime.Now.AddMinutes(10)  // Set the expiration time as needed
						});

						return Ok(new { Status = 200, Messages = "Login berhasil", Data = pic, Role = "PIC PKKMB", Token = token });
					}
					else
					{
						// login.password is incorrect
						return Unauthorized(new { Status = 401, Messages = "Kata Sandi Salah", Data = new Object() });
					}
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
				return StatusCode(500, new { Status = 500, Messages = "Terjadi Kesalahan Saat Login = " + ex.Message, Data = new Object() });
			}
		}

		[HttpPost("/ValidateToken", Name = "ValidateToken")]
		public IActionResult ValidateToken([FromHeader] string token)
		{
			if (token == null)
			{
				return BadRequest(new { Status = 401, Message = "Token is missing or empty." });
			}

			bool isValid = _loginRepo.ValidateJwtToken(token);

			if (isValid)
			{
				return Ok(new { Status = 200, Message = "Authorized." });
			}
			else
			{
				return Unauthorized(new { Status = 401, Message = "Unauthorized." });
			}
		}
	}
}
